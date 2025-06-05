using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    public class RegisterNewProduct
    {
        Product product = new Product();

        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Application.StartupPath}\EstoquePaiol.accdb;";

        public void AddNewProduct()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "INSERT INTO Produtos (CodBarras, Nome, Preco, Validade, EstoqueMinimo, QuantidadeAtual) " +
                    "VALUES (@CodBarras, @Nome, @Preco, @Validade, @EstoqueMin, @Quantidade)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodBarras", product.Barcode);
                        cmd.Parameters.AddWithValue("@Nome", product.Name);
                        cmd.Parameters.AddWithValue("@Preco", product.Value);
                        cmd.Parameters.AddWithValue("@Validade", product.Validate);
                        cmd.Parameters.AddWithValue("@EstoqueMin", product.minStock);
                        cmd.Parameters.AddWithValue("@Quantidade", product.Amount);

                        cmd.ExecuteNonQuery();

                        MessageBox.Show("Produto Adicionado Com Sucesso!");
                    }
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show($"Erro de banco de dados[Adicionar Produto]: {ex.Message}");
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

        public void UpdateProduct(string codBar, decimal amount)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                { 
                    conn.Open();

                    string query = "UPDATE Produtos SET QuantidadeAtual = QuantidadeAtual + @Quantidade " +
                                    "WHERE CodBarras = @CodBarras";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Quantidade", amount);
                        cmd.Parameters.AddWithValue("@CodBarras", codBar);

                        int linhasAfetadas = cmd.ExecuteNonQuery();

                        if (linhasAfetadas == 0)
                        {
                            MessageBox.Show("Produto não encontrado para atualizar a quantidade.");
                        }
                        else
                        {
                            MessageBox.Show("Quantidade adicionada com sucesso!");
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show($"Erro ao atualizar quantidade: {ex.Message}");
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

        public void ChangeID(string value, TextBox nameTB, NumericUpDown priceNUD,
                                NumericUpDown minStockNUD, DateTimePicker validateDTP)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "SELECT Nome, Preco, Validade, EstoqueMinimo " +
                                               "FROM Produtos WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodBarras", value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                product.Name = reader["Nome"].ToString();
                                product.Value = reader["preco"] != DBNull.Value ? Convert.ToDecimal(reader["Preco"]) : 0;
                                product.minStock = reader["EstoqueMinimo"] != DBNull.Value ? Convert.ToDecimal(reader["EstoqueMinimo"]) : 0;
                                product.Validate = reader["Validade"] != DBNull.Value ? Convert.ToDateTime(reader["Validade"]) : DateTime.Now;

                                nameTB.Text = product.Name;
                                priceNUD.Value = product.Value;
                                minStockNUD.Value = product.minStock;
                                validateDTP.Value = product.Validate;

                                RegisterForm.instance.isNewProduct = false;
                            }
                            else
                            {
                                product.Barcode = value;
                            }
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    MessageBox.Show($"Erro de banco de dados[buscar ID]: {ex.Message}");
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
        public void ChangeName(string value)
        {
            if (value != string.Empty) product.Name = value;
        }
        public void ChangeValue(decimal value)
        {
            if(value > 0) product.Value = value;
        }
        public void ChangeValidate(DateTime date)
        {
            if(date > DateTime.Now) product.Validate = date;
        }
        public void ChangeStockMin(decimal value)
        {
            if(value > 0) product.minStock = value;
        }
        public void ChangeAmount(decimal value)
        {
            if (value > 0) product.Amount = value;
        }
    }
}