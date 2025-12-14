using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// The Main Form (Dashboard) of the Inventory Manager.
    /// This form is responsible for initialization, displaying critical stock alerts,
    /// and navigating to other sections (Registration, Stock View, Sales, Reports).
    /// </summary>
    public partial class Main : Form
    {
        // --- Stock Alert Lists ---

        /// <summary>
        /// List to store products that are at or below the defined minimum stock level.
        /// </summary>
        List<Product> lowStockList = new List<Product>();

        /// <summary>
        /// List to store products that are close to their expiration date based on 'validateMin' days.
        /// </summary>
        List<Product> lowValidateList = new List<Product>();

        // --- Database Path Configuration ---

        /// <summary>
        /// Gets the path to the current user's local application data folder (AppData\Local).
        /// </summary>
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        /// <summary>
        /// Defines the subfolder where the Access database will be located.
        /// </summary>
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");

        /// <summary>
        /// Full path to the Access database file (EstoquePaiol.accdb).
        /// </summary>
        string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        /// <summary>
        /// Threshold in days to determine if a product is 'close to expiration'. Default is 60 days.
        /// This value is updated by the ComboBox.
        /// </summary>
        int validateMin = 60;


        // --- Connection and Query Declarations ---

        /// <summary>
        /// The OLE DB connection string for the Access database.
        /// </summary>
        string connString;

        /// <summary>
        /// SQL query to retrieve product data required for the dashboard alerts and lists.
        /// </summary>
        string query = "SELECT Nome, UF, Preco, Validade, EstoqueMinimo, QuantidadeAtual FROM Produtos";

        /// <summary>
        /// Instance of the PDFGenerator class used to create reports.
        /// </summary>
        PDFGenerator generator = new PDFGenerator();

        /// <summary>
        /// Constructor for the Main form. Performs initial setup and data loading.
        /// </summary>
        public Main()
        {
            // Set the thread culture to Brazilian Portuguese for proper date/currency formatting.
            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();

            // Check if the application data directory exists, and create it if not.
            if (!Directory.Exists(pastaBanco))
            {
                Directory.CreateDirectory(pastaBanco);
            }

            // Define the path to the template database file in the application's startup directory.
            string origemModelo = Path.Combine(Application.StartupPath, "EstoquePaiol.accdb");

            // Check if the database file exists in the AppData folder.
            if (!File.Exists(dbPath))
            {
                // If the file doesn't exist, copy the template to the application data path.
                File.Copy(origemModelo, dbPath);
            }

            // Define the connection string using the final database path.
            connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

            // Method to ensure the required database columns (like 'UF') exist in the schema.
            EnsureColumnExists();

            // Initial loading of product data and alerts.
            LoadProducts();

        }

        private void Main_Load(object sender, EventArgs e)
        {
            // Form load event handler (Currently empty)
        }

        /// <summary>
        /// Event handler for the button to open the Product Registration form.
        /// Hides the current form and displays the new one.
        /// </summary>
        private void openRegisterBtn_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            this.Hide();
        }

        /// <summary>
        /// Loads all product data from the database, filters for critical stock and expiration alerts,
        /// and populates the respective ListViews.
        /// </summary>
        private void LoadProducts()
        {
            // Clear existing data before reloading
            lowStockList.Clear();
            lowStockListView.Items.Clear();
            lowValidateList.Clear();
            closeToDueDateListView.Items.Clear();

            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            // Read each record from the database
                            while (reader.Read())
                            {
                                Product product = new Product();

                                // Map database fields to Product object properties
                                product.Name = reader["Nome"].ToString();
                                product.UF = reader["UF"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Validate = Convert.ToDateTime(reader["Validade"]);
                                product.minStock = Convert.ToDecimal(reader["EstoqueMinimo"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                // Check alert conditions and add to the respective list (only for items with quantity > 0)
                                if (MinStock(product.Amount, product.minStock) && product.Amount > 0) lowStockList.Add(product);
                                if (CloseToDueDate(product.Validate, validateMin) && product.Amount > 0) lowValidateList.Add(product);
                            }
                        }
                    }

                    // Sort the alert lists alphabetically by product name
                    lowStockList = lowStockList.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList();
                    lowValidateList = lowValidateList.OrderBy(x => x.Name, StringComparer.OrdinalIgnoreCase).ToList();


                    // Populate the Low Stock ListView
                    foreach (Product prod in lowStockList)
                    {
                        ListViewItem item = new ListViewItem(prod.Name);
                        item.SubItems.Add(prod.UF);
                        item.SubItems.Add(prod.Value.ToString());
                        item.SubItems.Add(prod.minStock.ToString("0.#####"));
                        item.SubItems.Add(prod.Amount.ToString("0.#####"));
                        lowStockListView.Items.Add(item);
                    }

                    // Populate the Close to Due Date ListView
                    foreach (Product prod in lowValidateList)
                    {
                        ListViewItem item = new ListViewItem(prod.Name);
                        item.SubItems.Add(prod.UF);
                        item.SubItems.Add(prod.Value.ToString());
                        item.SubItems.Add(prod.Validate.ToShortDateString());
                        item.SubItems.Add(prod.Amount.ToString("0.#####"));
                        closeToDueDateListView.Items.Add(item);
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle and display database-specific errors
                    MessageBox.Show($"Erro de banco de dados: {ex.Message}");
                }
                finally
                {
                    // Ensure the database connection is closed safely
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Checks the 'Produtos' table schema to ensure the 'UF' (Unidade de Fornecimento) column exists.
        /// If the column is missing, it adds it using an ALTER TABLE command.
        /// </summary>
        public void EnsureColumnExists()
        {
            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();

                    // Retrieve schema information for columns in the "Produtos" table
                    DataTable schemaTable = conn.GetOleDbSchemaTable(
                        OleDbSchemaGuid.Columns,
                        new object[] { null, null, "Produtos", null }
                    );

                    bool columnExists = false;
                    if (schemaTable != null)
                    {
                        // Check if a column named "UF" exists
                        columnExists = schemaTable.Rows
                            .Cast<DataRow>()
                            .Any(row => string.Equals(
                                row["COLUMN_NAME"].ToString(),
                                "UF",
                                StringComparison.OrdinalIgnoreCase));
                    }

                    if (!columnExists)
                    {
                        // If "UF" column is not found, add it as TEXT(2)
                        using (var cmd = new OleDbCommand("ALTER TABLE [Produtos] ADD COLUMN [UF] TEXT(2);", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                // Handle database errors during schema check/update
                MessageBox.Show($"Erro ao garantir coluna UF: {ex.Message}", "Erro de Banco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                // Handle general unexpected errors
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Determines if the product's due date is within the specified number of days from today.
        /// </summary>
        /// <param name="dueDate">The product's expiration date.</param>
        /// <param name="daysUntilExpiration">The maximum number of days remaining to trigger the alert.</param>
        /// <returns>True if the product is expiring soon but has not expired yet; otherwise, False.</returns>
        private bool CloseToDueDate(DateTime dueDate, int daysUntilExpiration)
        {
            DateTime today = DateTime.Now.Date;
            TimeSpan difference = dueDate - today;
            // Check if the difference in days is less than or equal to the threshold and not negative (i.e., not expired)
            return difference.TotalDays <= daysUntilExpiration && difference.TotalDays >= 0;
        }

        /// <summary>
        /// Determines if the current stock quantity is at or below the minimum stock level.
        /// </summary>
        /// <param name="amount">The current stock quantity.</param>
        /// <param name="min">The defined minimum stock quantity.</param>
        /// <returns>True if the current amount indicates low stock; otherwise, False.</returns>
        private bool MinStock(decimal amount, decimal min)
        {
            return amount <= min;
        }

        /// <summary>
        /// Event handler for the exit button. Closes the application.
        /// </summary>
        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        /// <summary>
        /// Event handler for the button to show the full stock/inventory list.
        /// </summary>
        private void ShowStockBtn_Click(object sender, EventArgs e)
        {
            new EstoqueForm().Show();
            this.Hide();
        }

        /// <summary>
        /// Event handler for the button to open the sales/movement form.
        /// </summary>
        private void sellFormBtn_Click(object sender, EventArgs e)
        {
            new SellForm().Show();
            this.Hide();

        }

        private void Main_Load_1(object sender, EventArgs e)
        {
            // Another form load event handler (Often auto-generated and unused)
        }

        /// <summary>
        /// Generates a PDF report for products that are close to their expiration date.
        /// Shows an alert if the list of expiring products is empty.
        /// </summary>
        private void expiringBtn_Click(object sender, EventArgs e)
        {
            if (lowValidateList.Count <= 0)
            {
                MessageBox.Show("Sem Gêneros Próximos da validade");
                return;
            }
            // Generate report using the filtered list and report type
            generator.GenerateReport(lowValidateList, "Produtos Próximos da Validade ", "", "", ReportType.Expiring);
        }

        /// <summary>
        /// Generates a PDF report for products that have reached a critical low stock level.
        /// Shows an alert if the critical stock list is empty.
        /// </summary>
        private void criticalStockBtn_Click(object sender, EventArgs e)
        {
            if (lowStockList.Count <= 0)
            {
                MessageBox.Show("Sem Gêneros com estoque Critico");
                return;
            }
            // Generate report using the filtered list and report type
            generator.GenerateReport(lowStockList, "Produtos com Estoque Crítico", "", "", ReportType.CriticalStock);
        }

        /// <summary>
        /// Event handler for the ComboBox selection changing.
        /// Updates the 'validateMin' threshold based on the selected index and reloads the product data.
        /// </summary>
        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            switch (comboBox1.SelectedIndex)
            {
                // Map ComboBox index to day threshold for expiration alert
                case 0:
                    validateMin = 30;
                    break;
                case 1:
                    validateMin = 60;
                    break;
                case 2:
                    validateMin = 90;
                    break;
                case 3:
                    validateMin = 120;
                    break;
                case 4:
                    validateMin = 150;
                    break;
                case 5:
                    validateMin = 180;
                    break;
                default:
                    validateMin = 60; // Default fallback
                    break;
            }

            // Reload products to apply the new validation threshold filter
            LoadProducts();
        }
    }
}