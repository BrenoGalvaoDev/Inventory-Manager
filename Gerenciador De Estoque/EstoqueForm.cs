using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static Org.BouncyCastle.Crypto.Digests.SkeinEngine;

namespace Gerenciador_De_Estoque
{
    public partial class EstoqueForm : Form
    {
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Application.StartupPath}\EstoquePaiol.accdb;";
        string query = "SELECT CodBarras, Nome, Validade, EstoqueMinimo, QuantidadeAtual FROM Produtos";

        List<Product> products = new List<Product>();

        ManageItems manageItems = new ManageItems();

        int itemIndex;

        public EstoqueForm()
        {
            InitializeComponent();
            LoadProducts();
        }

        #region Buttons
        private void stockListView_MouseClick(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                ListViewItem item = stockListView.GetItemAt(e.X, e.Y);

                itemIndex = item.Index;
            }
        }

        private void excluirProdutoToolStripMenuItem_Click(object sender, EventArgs e)
        {
            DeleteItem();
        }

        private void toolStripTextBox1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                string value = toolStripTextBox1.Text;
                if (string.IsNullOrEmpty(value))
                {
                    MessageBox.Show("Nome invalido");
                    return;
                }

                ChangeName(value);
            }
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            new Main().Show();
            this.Hide();
        }
        #endregion

        #region Functions
        public void LoadProducts()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                products.Clear();
                stockListView.Items.Clear();
                try
                {
                    conn.Open();

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                Product newProduct = new Product();

                                newProduct.Barcode  = reader["CodBarras"].ToString();
                                newProduct.Name     = reader["Nome"].ToString();
                                newProduct.Validate = Convert.ToDateTime(reader["Validade"]);
                                newProduct.minStock = Convert.ToDecimal(reader["EstoqueMinimo"]);
                                newProduct.Amount   = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                products.Add(newProduct);
                            }

                            foreach (var prod in products)
                            {
                                ListViewItem item = new ListViewItem(prod.Barcode);

                                item.SubItems.Add(prod.Name);
                                item.SubItems.Add(prod.Validate.ToShortDateString());
                                item.SubItems.Add(prod.minStock.ToString());
                                item.SubItems.Add(prod.Amount.ToString());

                                stockListView.Items.Add(item);
                            }
                        }
                    }
                }
                catch (OleDbException ex)
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

        public async void ChangeName(string value)
        {
            string id = products[itemIndex].Barcode;
            bool res = await manageItems.ChangeItemName(value, id);

            if (res)
            {
                MessageBox.Show("Nome Atualizado!");
                stockListView.Items[itemIndex].SubItems[1].Text = value;
                toolStripTextBox1.Clear();
                contextMenuStrip1.Close();
            }
        }

        public async void DeleteItem()
        {
            string id = products[itemIndex].Barcode;

            DialogResult result = MessageBox.Show("Deseja realmente Excluir esse Produto do Banco de Dados?", "Confirmação",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                bool res = await manageItems.DeleteItem(id);

                if (res)
                {
                    MessageBox.Show("Item Excluído com Sucesso!");
                    stockListView.Items.RemoveAt(itemIndex);
                    contextMenuStrip1.Close();
                }
            }
        }

        #endregion
    }
}