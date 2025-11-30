using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    public partial class SellForm : Form
    {
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");
        static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

        Product product = new Product();
        PDFGenerator pdfGenerator = new PDFGenerator();

        List<Product> productsToRemove = new List<Product>();

        ListViewItem selectedItem; // use to remove item on right click
        public SellForm()
        {
            InitializeComponent();
        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            GetProductByID(idTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
        }

        private void SellBtn_Click(object sender, EventArgs e)
        {
            RemoveProductToDB();
        }

        private void backBtn_Click(object sender, EventArgs e)
        {
            new Main().Show();
            this.Hide();
        }

        private void AddBtn_Click(object sender, EventArgs e)
        {
            AddProductToList(amountNumericUpDown);
            idTextBox.Focus();
        }

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
                                product.Name = reader["Nome"].ToString();
                                product.UF = reader["UF"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

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
                    MessageBox.Show("Produto Não Encontrado");
                }
            }
        }
        public void GetProductByName(string value, TextBox nameTB, TextBox priceTB, TextBox systemAmountTB)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT CodBarras, UF, Preco, QuantidadeAtual " + "FROM Produtos WHERE Nome = @Nome";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodBarras", value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Barcode = reader["CodBarras"].ToString();
                                product.UF = reader["UF"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

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

        public void AddProductToList(NumericUpDown amount)
        {
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

            Product newProduct = new Product()
            {
                Name = product.Name,
                Value = product.Value,
                Amount = amount.Value,
                Barcode = idTextBox.Text
            };

            productsToRemove.Add(newProduct);

            ListViewItem item = new ListViewItem(newProduct.Name);
            item.SubItems.Add(newProduct.Amount.ToString());
            sellListView.Items.Add(item);

            nameTextBox.Clear();
            idTextBox.Clear();
            priceTextBox.Clear();
            systemAmountTextBox.Clear();
            amount.Value = 0;
        }
        public void RemoveProductToDB()
        {
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
                    string query = "UPDATE Produtos SET QuantidadeAtual = QuantidadeAtual - @Quantidade " +
                                        "WHERE CodBarras = @CodBarras";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        foreach (var prod in productsToRemove)
                        {
                            cmd.Parameters.Clear();
                            var newAmount = cmd.Parameters.Add("@Quantidade", OleDbType.Double);
                            newAmount.Value = prod.Amount;
                            cmd.Parameters.AddWithValue("@CodBarras", prod.Barcode);

                            
                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            if (linhasAfetadas == 0)
                            {
                                MessageBox.Show($"Produto {prod.Name} não tem quantidade suficiente no estoque.");
                            }
                        }

                        pdfGenerator.GenerateReport(productsToRemove,comboBox1.SelectedItem.ToString() , paioleiroTxt.Text, recebedorTxt.Text, ReportType.Sales);
                        productsToRemove.Clear();
                        sellListView.Items.Clear();

                        MessageBox.Show("Produtos removidos Com Sucesso!");
                    }
                }
                catch(OleDbException ex)
                {
                    MessageBox.Show($"Erro ao atualizar quantidade: {ex.Message}");

                }
            }
        }

        private void idTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;
                this.SelectNextControl((Control)sender, true, true, true, true);

                GetProductByID(idTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
            }
        }

        private void amountNumericUpDown_Enter(object sender, EventArgs e)
        {
            amountNumericUpDown.Text = "";
        }

        private void nameTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter || e.KeyCode == Keys.Tab)
            {
                e.SuppressKeyPress = true;

                GetProductByName(nameTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
            }
        }

        private void sellListView_MouseClick(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                selectedItem = sellListView.GetItemAt(e.X, e.Y);
            }
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            if (selectedItem == null) return;


            DialogResult result = MessageBox.Show("Deseja Excluir esse Produto da Lista?", "Confirmação",
                MessageBoxButtons.OKCancel, MessageBoxIcon.Question);

            if (result == DialogResult.OK)
            {
                sellListView.Items.Remove(selectedItem);
                var productToRemove = productsToRemove.FirstOrDefault(p => p.Name == selectedItem.Text && p.Amount.ToString() == selectedItem.SubItems[1].Text);
                if (productToRemove != null)
                {
                    productsToRemove.Remove(productToRemove);
                }

            }
        }

    }
}
