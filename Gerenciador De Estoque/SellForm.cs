using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    public partial class SellForm : Form
    {
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Application.StartupPath}\EstoquePaiol.accdb;";

        Product product = new Product();
        PDFGenerator pdfGenerator = new PDFGenerator();

        List<Product> productsToRemove = new List<Product>();

        public SellForm()
        {
            InitializeComponent();
        }

        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void idTextBox_TextChanged(object sender, EventArgs e)
        {
            GetProduct(idTextBox.Text, nameTextBox, priceTextBox, systemAmountTextBox);
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
        }

        public void GetProduct(string value, TextBox nameTB, TextBox priceTB, TextBox systemAmountTB)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Nome, Preco, QuantidadeAtual " + "FROM Produtos WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodBarras", value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Name = reader["Nome"].ToString();
                                product.Value = Convert.ToDecimal(reader["Preco"]);
                                product.Amount = Convert.ToDecimal(reader["QuantidadeAtual"]);

                                nameTB.Text = product.Name.ToString();
                                priceTB.Text = product.Value.ToString();
                                systemAmountTB.Text = product.Amount.ToString();
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
                            cmd.Parameters.AddWithValue("@Quantidade", prod.Amount);
                            cmd.Parameters.AddWithValue("@CodBarras", prod.Barcode);

                            
                            int linhasAfetadas = cmd.ExecuteNonQuery();
                            if (linhasAfetadas == 0)
                            {
                                MessageBox.Show($"Produto {prod.Name} não tem quantidade suficiente no estoque.");
                            }
                        }
                        pdfGenerator.GeneratePDFReport(productsToRemove);
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

        private void SellForm_Load(object sender, EventArgs e)
        {

        }
    }
}
