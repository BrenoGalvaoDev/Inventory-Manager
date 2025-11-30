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
    public partial class Main : Form
    {
        List<Product> lowStockList = new List<Product>();
        List<Product> lowValidateList = new List<Product>();

        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string pastaBanco = Path.Combine(localAppData ,"GerenciadorDeEstoque");
        string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");
        

        string connString;
        string query = "SELECT Nome, UF, Preco, Validade, EstoqueMinimo, QuantidadeAtual FROM Produtos";

        PDFGenerator generator = new PDFGenerator();

        public Main()
        {

            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;

            InitializeComponent();

            if (!Directory.Exists(pastaBanco))
            {
                Directory.CreateDirectory(pastaBanco);
            }

            string origemModelo = Path.Combine(Application.StartupPath, "EstoquePaiol.accdb");
            if (!File.Exists(dbPath))
            {
                File.Copy(origemModelo, dbPath);
            }

            connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

            EnsureColumnExists();

            LoadProducts();

        }

        private void Main_Load(object sender, EventArgs e)
        {

        }

        private void openRegisterBtn_Click(object sender, EventArgs e)
        {
            new RegisterForm().Show();
            this.Hide();
        }
        private void LoadProducts()
        {
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
                            while (reader.Read())
                            {
                                Product product = new Product();

                                product.Name = reader["Nome"].ToString();
                                product.UF = reader["UF"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Validate = Convert.ToDateTime(reader["Validade"]);
                                product.minStock = Convert.ToDecimal(reader["EstoqueMinimo"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                if (MinStock(product.Amount, product.minStock) && product.Amount > 0) lowStockList.Add(product);
                                if (CloseToDueDate(product.Validate, 60) && product.Amount > 0) lowValidateList.Add(product);
                            }
                        }
                    }

                    foreach (Product prod in lowStockList)
                    {
                        ListViewItem item = new ListViewItem(prod.Name);
                        item.SubItems.Add(prod.UF);
                        item.SubItems.Add(prod.Value.ToString());
                        item.SubItems.Add(prod.minStock.ToString("0.#####"));
                        item.SubItems.Add(prod.Amount.ToString("0.#####"));
                        lowStockListView.Items.Add(item);
                    }

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
                catch(OleDbException ex)
                {
                    MessageBox.Show($"Erro de banco de dados: {ex.Message}");
                }
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        public void EnsureColumnExists()
        {
            try
            {
                using (var conn = new OleDbConnection(connString))
                {
                    conn.Open();

                    DataTable schemaTable = conn.GetOleDbSchemaTable(
                        OleDbSchemaGuid.Columns,
                        new object[] { null, null, "Produtos", null }
                    );

                    bool columnExists = false;
                    if (schemaTable != null)
                    {
                        columnExists = schemaTable.Rows
                            .Cast<DataRow>()
                            .Any(row => string.Equals(
                                row["COLUMN_NAME"].ToString(),
                                "UF",
                                StringComparison.OrdinalIgnoreCase));
                    }

                    if (!columnExists)
                    {
                        using (var cmd = new OleDbCommand("ALTER TABLE [Produtos] ADD COLUMN [UF] TEXT(2);", conn))
                        {
                            cmd.ExecuteNonQuery();
                        }
                    }
                }
            }
            catch (OleDbException ex)
            {
                MessageBox.Show($"Erro ao garantir coluna UF: {ex.Message}", "Erro de Banco", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro inesperado: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private bool CloseToDueDate(DateTime dueDate, int daysUntilExpiration)
        {
            DateTime today = DateTime.Now.Date;
            TimeSpan difference = dueDate - today;
            return difference.TotalDays <= daysUntilExpiration && difference.TotalDays >= 0;
        }

        private bool MinStock(decimal amount, decimal min)
        {
            return amount <= min;
        }

        private void exitBtn_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void ShowStockBtn_Click(object sender, EventArgs e)
        {
            new EstoqueForm().Show();
            this.Hide();
        }

        private void sellFormBtn_Click(object sender, EventArgs e)
        {
            new SellForm().Show();
            this.Hide();

        }

        private void Main_Load_1(object sender, EventArgs e)
        {

        }

        private void expiringBtn_Click(object sender, EventArgs e)
        {
            if (lowValidateList.Count <= 0)
            {
                MessageBox.Show("Sem Gêneros Próximos da validade");
                return;
            }
            generator.GenerateReport(lowValidateList, "Produtos Próximos da Validade ", "", "", ReportType.Expiring);
        }

        private void criticalStockBtn_Click(object sender, EventArgs e)
        {
            if (lowStockList.Count <= 0)
            {
                MessageBox.Show("Sem Gêneros com estoque Critico");
                return;
            }
            generator.GenerateReport(lowStockList, "Produtos com Estoque Crítico", "", "", ReportType.CriticalStock);
        }
    }
}
