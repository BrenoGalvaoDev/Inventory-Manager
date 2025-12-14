using System;
using System.Globalization;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb; // Essential namespace for Access connection (OLE DB Provider)
using System.Drawing;
using System.IO; // Used for file path manipulation, especially for the database path
using System.Linq; // Used for LINQ queries, such as product ordering
using System.Reflection;
using System.Text;
using System.Threading; // Used to set the current thread's culture
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// The main inventory management form (EstoqueForm).
    /// Responsible for displaying the list of products, loading data from the Access database,
    /// and providing access to management functionalities (Delete, Edit, Generate Report).
    /// </summary>
    public partial class EstoqueForm : Form
    {
        // --- Static Declarations and Database Path Configuration ---

        /// <summary>
        /// Gets the path to the current user's local application data folder (usually AppData\Local).
        /// Used to store the database in a secure, user-specific location.
        /// </summary>
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        /// <summary>
        /// Defines the subfolder name within AppData\Local where the database will be stored.
        /// </summary>
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");

        /// <summary>
        /// Full path to the Access database file.
        /// Filename: EstoquePaiol.accdb.
        /// </summary>
        static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        /// <summary>
        /// OLE DB connection string for the Access database (.accdb).
        /// Uses the Microsoft.ACE.OLEDB.12.0 provider.
        /// </summary>
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

        /// <summary>
        /// SQL query to load all necessary product data.
        /// Selects: Barcode, Name, Unit of Supply (UF), Price, Expiry Date, Minimum Stock, and Current Quantity.
        /// </summary>
        string query = "SELECT CodBarras, Nome, UF, Preco, Validade, EstoqueMinimo, QuantidadeAtual FROM Produtos";


        /// <summary>
        /// List that stores 'Product' objects loaded from the database.
        /// Serves as the data source for the ListView and for edit/delete operations.
        /// </summary>
        List<Product> products = new List<Product>();

        /// <summary>
        /// Instance of the class containing the logic for item manipulation operations
        /// in the database (e.g., changing name, deleting item).
        /// </summary>
        ManageItems manageItems = new ManageItems();

        /// <summary>
        /// Instance of the class responsible for generating PDF reports.
        /// </summary>
        PDFGenerator generator = new PDFGenerator();

        /// <summary>
        /// Index of the item currently selected in the 'stockListView'.
        /// Primarily used to identify the product to be edited or deleted via context menu.
        /// </summary>
        int itemIndex;

        /// <summary>
        /// Static instance of the form, implementing a simple Singleton pattern.
        /// Allows other forms or classes to access methods and data from this instance.
        /// </summary>
        public static EstoqueForm instance;

        /// <summary>
        /// Constructor for the EstoqueForm.
        /// </summary>
        public EstoqueForm()
        {
            // Assigns the static instance for global access
            instance = this;

            // Configures the application culture to Portuguese-Brazil (pt-BR)
            // Essential for correct formatting of dates and currency values.
            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;


            InitializeComponent();
            // Loads the list of products from the database upon form initialization
            LoadProducts();
        }

        #region Buttons

        /// <summary>
        /// Event handler for mouse clicks on the 'stockListView'.
        /// Specifically used to capture the index of the item when right-clicked
        /// (needed for the ContextMenuStrip operations).
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Mouse event arguments.</param>
        private void stockListView_MouseClick(object sender, MouseEventArgs e)
        {
            // Checks if the click was with the right button
            if (e.Button == MouseButtons.Right)
            {
                // Gets the ListView item under the cursor
                ListViewItem item = stockListView.GetItemAt(e.X, e.Y);

                if (item != null)
                {
                    // Stores the index of the clicked item for use in context menu operations
                    itemIndex = item.Index;
                }
            }
        }

        /// <summary>
        /// Event handler for the "Excluir Produto" (Delete Product) context menu item.
        /// Calls the deletion method for the selected product.
        /// </summary>
        private void excluirProdutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        /// <summary>
        /// Event handler for key presses on the 'toolStripTextBox1' (usually used for new name input).
        /// Triggers the change name function when the 'Enter' key is pressed.
        /// </summary>
        /// <param name="sender">Source of the event.</param>
        /// <param name="e">Key event arguments.</param>
        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            // Checks if the Enter key was pressed
            if (e.KeyCode == Keys.Enter)
            {
                string value = toolStripTextBox1.Text;

                // Input validation
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show("Nome Invalido");
                    return;
                }

                // Calls the asynchronous function to change the name
                ChangeName(value);
            }
        }

        /// <summary>
        /// Event handler for the "Back" button ('backBtn').
        /// Hides the current form and displays the main menu form ('Main').
        /// </summary>
        private void backBtn_Click(object sender, EventArgs e)
        {
            new Main().Show();
            this.Hide();
        }
        #endregion

        #region Functions

        /// <summary>
        /// Loads all products from the Access database ('Produtos' table)
        /// and populates the 'stockListView' in the user interface.
        /// The product list is cleared and fully reloaded.
        /// </summary>
        public void LoadProducts()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                // Clears existing lists
                products.Clear();
                stockListView.Items.Clear();

                try
                {
                    conn.Open();

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            // Reads data from the database and adds it to the 'products' list
                            while (reader.Read())
                            {
                                Product newProduct = new Product();

                                // Mapping database fields to the Product class properties
                                newProduct.Barcode = reader["CodBarras"].ToString();
                                newProduct.Name = reader["Nome"].ToString();
                                newProduct.UF = reader["UF"].ToString();
                                newProduct.Value = Convert.ToDecimal(reader["Preco"]);
                                newProduct.Validate = Convert.ToDateTime(reader["Validade"]);
                                newProduct.minStock = Convert.ToDecimal(reader["EstoqueMinimo"]);
                                newProduct.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                products.Add(newProduct);
                            }

                            // Orders the product list alphabetically by name (case-insensitive)
                            products = products.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList();


                            // Populates the ListView with the ordered products
                            foreach (var prod in products)
                            {
                                ListViewItem item = new ListViewItem(prod.Barcode);

                                item.SubItems.Add(prod.Name);
                                item.SubItems.Add(prod.UF);
                                item.SubItems.Add(prod.Value.ToString());
                                item.SubItems.Add(prod.Validate.ToShortDateString()); // Formats date
                                item.SubItems.Add(prod.minStock.ToString("0.#####")); // Formats decimal for minimum stock
                                item.SubItems.Add(prod.Amount.ToString("0.#####")); // Formats decimal for current quantity

                                stockListView.Items.Add(item);
                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Displays an error message in case of failure during database operation
                    MessageBox.Show($"Database Error: {ex.Message}");
                }
                finally
                {
                    // Ensures that the database connection is closed, even in case of an error
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the name of the selected product in the database.
        /// This is an asynchronous operation, dependent on the 'manageItems' class.
        /// </summary>
        /// <param name="value">The new name to be assigned to the product.</param>
        public async void ChangeName(string value)
        {
            // Gets the Barcode (ID) of the selected product
            string id = products[itemIndex].Barcode;

            // Calls the asynchronous method in the item management class
            bool res = await manageItems.ChangeItemName(value, id);

            if (res)
            {
                // If the update is successful
                MessageBox.Show("Nome Atualizado!");

                // Updates the name directly in the ListView (column 1) to avoid a full reload
                stockListView.Items[itemIndex].SubItems[1].Text = value;

                // Clears the textbox and closes the context menu
                toolStripTextBox1.Clear();
                contextMenuStrip1.Close();
            }
        }

        /// <summary>
        /// Deletes the selected product from the Access database.
        /// Requires user confirmation before executing the deletion.
        /// </summary>
        public async void DeleteItem()
        {
            // Gets the Barcode (ID) of the selected product
            string id = products[itemIndex].Barcode;

            // Prompts the user for confirmation
            DialogResult result = MessageBox.Show("Tem Certeza que quer excluir o item da tabela?", "Confirmar",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // Calls the asynchronous method in the item management class
                bool res = await manageItems.DeleteItem(id);

                if (res)
                {
                    // If the deletion is successful
                    MessageBox.Show("Item Deletetado com Sucesso!");

                    // Removes the item from the ListView display
                    stockListView.Items.RemoveAt(itemIndex);

                    // Closes the context menu
                    contextMenuStrip1.Close();
                }
            }
        }

        /// <summary>
        /// Opens the editing form for the currently selected product.
        /// Loads the product data into the new form and schedules the main list
        /// to reload upon the editing form's closure.
        /// </summary>
        public void EditItem()
        {
            EditItemForm editItemForm = new EditItemForm();

            // Gets the full Product object for the selected item
            Product prod = products[itemIndex];

            // Loads the product data into the editing form
            editItemForm.LoadProductData(prod);

            // Subscribes to the 'FormClosed' event to reload the product list after editing is done
            editItemForm.FormClosed += (s, args) => LoadProducts();

            // Displays the editing form as a modal dialog
            editItemForm.ShowDialog();
        }

        #endregion

        /// <summary>
        /// Event handler for the "Full Stock" button ('fullStockBtn').
        /// Calls the PDF generator to create a complete report of all products.
        /// </summary>
        private void fullStockBtn_Click(object sender, EventArgs e)
        {
            // Calls the generator method. Empty strings might represent optional filters/titles.
            generator.GenerateReport(products, " ", " ", "Full Stock", ReportType.FullStock);
        }

        /// <summary>
        /// Event handler for the "Editar Item" (Edit Item) context menu item.
        /// Calls the editing method for the selected product.
        /// </summary>
        private void editarItemToolStripMenuItem_Click(object sender, EventArgs e)
        {
            EditItem();
        }
    }
}