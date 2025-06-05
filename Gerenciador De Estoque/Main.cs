using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;


namespace Gerenciador_De_Estoque
{
    public partial class Main : Form
    {
        List<Product> lowStockList = new List<Product>();
        List<Product> lowValidateList = new List<Product>();

        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Application.StartupPath}\EstoquePaiol.accdb;";
        string query = "SELECT Nome, Validade, EstoqueMinimo, QuantidadeAtual FROM Produtos";

        public Main()
        {
            InitializeComponent();
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
                                product.Validate = Convert.ToDateTime(reader["Validade"]);
                                product.minStock = Convert.ToDecimal(reader["EstoqueMinimo"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                if(MinStock(product.Amount, product.minStock) && product.Amount > 0) lowStockList.Add(product);
                                if (CloseToDueDate(product.Validate, 60) && product.Amount > 0) lowValidateList.Add(product);
                            }
                        }
                    }

                    foreach (Product prod in lowStockList)
                    {
                        ListViewItem item = new ListViewItem(prod.Name);
                        item.SubItems.Add(prod.minStock.ToString());
                        item.SubItems.Add(prod.Amount.ToString());
                        lowStockListView.Items.Add(item);
                    }

                    foreach (Product prod in lowValidateList)
                    {
                        ListViewItem item = new ListViewItem(prod.Name);
                        item.SubItems.Add(prod.Validate.ToShortDateString());
                        item.SubItems.Add(prod.Amount.ToString());
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
    }
}
