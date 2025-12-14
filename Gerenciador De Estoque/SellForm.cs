using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb; // Necessary for interacting with the Access database
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// Form responsible for handling the product removal (sale/withdrawal) process.
    /// It allows searching products, adding them to a list, updating stock in the database, and generating a report.
    /// </summary>
    public partial class SellForm : Form
    {
        // --- Database Path Configuration ---
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");
        static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        // The OLE DB connection string for connecting to the Access database.
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

        // Object to hold the details of the product currently being queried or added.
        Product product = new Product();
        // Utility class for generating PDF reports.
        PDFGenerator pdfGenerator = new PDFGenerator();

        // List to hold products (and their quantities) that are intended to be removed from stock.
        List<Product> productsToRemove = new List<Product>();

        // Holds the currently selected ListView item, primarily used for right-click removal.
        ListViewItem selectedItem;

        /// <summary>
        /// Constructor for the SellForm.
        /// </summary>
        public SellForm()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Event handler for changes in the Barcode/ID text box.
        /// Triggers a database lookup for the product.
        /// </summary>
        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            GetProductByID(idTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
        }

        /// <summary>
        /// Event handler for the Sell/Confirm button click.
        /// Initiates the process of removing the products listed in the 'productsToRemove' list from the database stock.
        /// </summary>
        private void SellBtn_Click(object sender, EventArgs e)
        {
            RemoveProductToDB();
        }

        /// <summary>
        /// Event handler for the Back button click.
        /// Opens the Main form and hides the current form.
        /// </summary>
        private void backBtn_Click(object sender, EventArgs e)
        {
            new Main().Show();
            this.Hide();
        }

        /// <summary>
        /// Event handler for the Add button click.
        /// Adds the currently queried product (with the specified amount) to the 'productsToRemove' list and the ListView display.
        /// </summary>
        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddProductToList(amountNumericUpDown);
            idTextBox.Focus();
        }

        /// <summary>
        /// Retrieves product details from the database using its Barcode (CodBarras) and populates the UI controls.
        /// </summary>
        public void GetProductByID(string value, TextBox nameTB, TextBox priceTB, TextBox systemAmountTB)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Nome, UF, Preco, QuantidadeAtual " + "FROM Produtos WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodBarras", value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate the internal product object
                                product.Name = reader["Nome"].ToString();
                                product.UF = reader["UF"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                // Populate the UI controls
                                nameTB.Text = product.Name.ToString();
                                priceTB.Text = product.Value.ToString();
                                systemAmountTB.Text = product.Amount.ToString();
                                ufTB.Text = product.UF.ToString();
                            }
                        }
                    }
                }
                catch
                {
                    // Catch-all for database errors or product not found
                    MessageBox.Show("Produto Não Encontrado");
                }
            }
        }

        /// <summary>
        /// Retrieves product details from the database using its Name and populates the UI controls.
        /// Note: This method currently has a potential bug/typo: it uses '@CodBarras' parameter name but searches by '@Nome'. 
        /// The query should use WHERE Nome = @Nome. The current implementation uses CodBarras for the parameter name, which might be incorrect 
        /// but is kept as per the original code structure.
        /// </summary>
        public void GetProductByName(string value, TextBox nameTB, TextBox priceTB, TextBox systemAmountTB)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Query uses WHERE Nome = @Nome, but parameter name in command is @CodBarras
                    string query = "SELECT CodBarras, UF, Preco, QuantidadeAtual " + "FROM Produtos WHERE Nome = @Nome";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Potential parameter name mismatch: should be @Nome if searching by Nome
                        cmd.Parameters.AddWithValue("@CodBarras", value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Populate internal product object and UI controls
                                product.Barcode = reader["CodBarras"].ToString();
                                product.UF = reader["UF"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                // Populate the UI controls
                                idTextBox.Text = product.Barcode.ToString();
                                priceTB.Text = product.Value.ToString();
                                systemAmountTB.Text = product.Amount.ToString();
                                ufTB.Text = product.UF.ToString();
                            }
                        }
                    }
                }
                catch
                {
                    MessageBox.Show("Produto Não Encontrado");
                }
            }
        }

        /// <summary>
        /// Validates the current product and amount, adds a new Product object (representing the removal)
        /// to the list, updates the ListView, and clears the input fields.
        /// </summary>
        public void AddProductToList(NumericUpDown amount)
        {
            // Input validation
            if (string.IsNullOrEmpty(product.Name))
            {
                MessageBox.Show("Produto inválido");
                return;
            }
            if (amount.Value <= 0)
            {
                MessageBox.Show("Quantidade inválida");
                return;
            }
            if (amount.Value > product.Amount)
            {
                MessageBox.Show("Quantidade informada não tem no estoque");
                return;
            }

            // Create a new Product object specifically for the removal transaction
            Product newProduct = new Product()
            {
                Name = product.Name,
                UF = product.UF,
                Value = product.Value,
                Amount = amount.Value, // This is the quantity to be removed
                Barcode = idTextBox.Text
            };

            productsToRemove.Add(newProduct);

            // Add the item to the ListView for display
            ListViewItem item = new ListViewItem(newProduct.Name);
            item.SubItems.Add(newProduct.UF.ToString());
            item.SubItems.Add(newProduct.Amount.ToString());
            sellListView.Items.Add(item);

            // Clear input fields
            nameTextBox.Clear();
            idTextBox.Clear();
            priceTextBox.Clear();
            systemAmountTextBox.Clear();
            amount.Value = 0;
        }

        /// <summary>
        /// Executes the database transaction to remove (decrease) the quantity of all products 
        /// listed in 'productsToRemove', generates a report, and clears the list and display.
        /// </summary>
        public void RemoveProductToDB()
        {
            // Pre-transaction validation
            if (string.IsNullOrEmpty(paioleiroTxt.Text) || string.IsNullOrEmpty(recebedorTxt.Text))
            {
                MessageBox.Show("Nome do Paioleiro/Recebedor Invalido!");
                return;
            }
            if (comboBox1.SelectedItem == null)
            {
                MessageBox.Show("Setor Não Selecionado!");
                return;
            }

            if (productsToRemove.Count <= 0)
            {
                MessageBox.Show("Quantidade de Itens Invalida!");
                return;
            }

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    // SQL query to decrement the current quantity
                    string query = "UPDATE Produtos SET QuantidadeAtual = QuantidadeAtual - @Quantidade " +
                                   "WHERE CodBarras = @CodBarras";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Process each product in the removal list
                        foreach (var prod in productsToRemove)
                        {
                            cmd.Parameters.Clear(); // Clear parameters for the next iteration
                            var newAmount = cmd.Parameters.Add("@Quantidade", OleDbType.Double);
                            newAmount.Value = prod.Amount; // The amount to subtract
                            cmd.Parameters.AddWithValue("@CodBarras", prod.Barcode);

                            // Execute the update
                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            if (linhasAfetadas == 0)
                            {
                                MessageBox.Show($"Produto {prod.Name} não tem quantidade suficiente no estoque.");
                            }
                        }

                        // Generate the report after successful database updates
                        pdfGenerator.GenerateReport(productsToRemove, comboBox1.SelectedItem.ToString(), paioleiroTxt.Text, recebedorTxt.Text, ReportType.Sales);

                        // Clear the transaction list and UI display
                        productsToRemove.Clear();
                        sellListView.Items.Clear();

                        MessageBox.Show("Produtos removidos Com Sucesso!");
                    }
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show($"Erro ao atualizar quantidade: {ex.Message}");

                }
            }
        }

        /// <summary>
        /// Event handler for key down events on the ID text box.
        /// Captures Enter or Tab keys to move focus to the next control and trigger the product lookup.
        /// </summary>
        private void idTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true; // Prevent the default beep/action
                this.SelectNextControl((Control)sender, true, true, true, true); // Manually move focus

                GetProductByID(idTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
            }
        }

        /// <summary>
        /// Event handler for the Amount Numeric Up/Down control gaining focus.
        /// Clears the text for easier input.
        /// </summary>
        private void amountNumericUpDown_Enter(object sender, EventArgs e)
        {
            amountNumericUpDown.Text = "";
        }

        /// <summary>
        /// Event handler for key down events on the Name text box.
        /// Captures Enter or Tab keys to move focus to the next control and trigger the product lookup by name.
        /// </summary>
        private void nameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;

                // Note: The focus selection logic is omitted here in the original code but the lookup is called.
                GetProductByName(nameTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
            }
        }

        /// <summary>
        /// Event handler for mouse clicks on the sellListView.
        /// If the right mouse button is clicked, it stores the selected item for potential removal.
        /// </summary>
        private void sellListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                // Get the ListView item at the click coordinates
                selectedItem = sellListView.GetItemAt(e.X, e.Y);
            }
        }

        /// <summary>
        /// Event handler for the right-click context menu item (toolStripMenuItem1).
        /// Prompts the user for confirmation and, if confirmed, removes the selected item from the ListView and the transaction list.
        /// </summary>
        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (selectedItem == null) return; // Exit if no item was selected via right-click


            DialogResult result = MessageBox.Show("Deseja Excluir esse Produto da Lista?", "Confirmação",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                // Remove from the visible list
                sellListView.Items.Remove(selectedItem);

                // Find and remove the corresponding Product object from the internal list
                var productToRemove = productsToRemove.FirstOrDefault(p => p.Name == selectedItem.Text && p.Amount.ToString() == selectedItem.SubItems[1].Text);
                if (productToRemove != null)
                {
                    productsToRemove.Remove(productToRemove);
                }

            }
        }

    }
}