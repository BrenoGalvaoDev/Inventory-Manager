using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.OleDb; // Necessary for interacting with the Access database
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    /// <summary>
    /// Handles the business logic for registering new products and updating existing product stock quantities.
    /// It manages the interaction with the Access database.
    /// </summary>
    public class RegisterNewProduct
    {
        // Internal Product object used to hold temporary data before saving to the database.
        Product product = new Product();

        // --- Database Path Configuration ---
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");
        static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        // The OLE DB connection string for connecting to the Access database.
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

        /// <summary>
        /// Inserts a new product record into the 'Produtos' table using the data stored in the internal Product object.
        /// </summary>
        public void AddNewProduct()
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    // Ensure the culture is set to pt-BR for consistent decimal/currency handling, especially with Access/OLEDB.
                    System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("pt-BR");

                    conn.Open();
                    string query = "INSERT INTO Produtos (CodBarras, Nome, UF, Preco, Validade, EstoqueMinimo, QuantidadeAtual) " +
                                   "VALUES (@CodBarras, @Nome, @UF, @Preco, @Validade, @EstoqueMin, @Quantidade)";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Add parameters based on the properties of the internal product object
                        cmd.Parameters.AddWithValue("@CodBarras", product.Barcode);
                        cmd.Parameters.AddWithValue("@Nome", product.Name);
                        cmd.Parameters.AddWithValue("@UF", product.UF);

                        // Price parameter, specifically typed as Currency for accuracy in Access
                        var valorParam = cmd.Parameters.Add("@Preco", OleDbType.Currency);
                        valorParam.Value = product.Value;

                        cmd.Parameters.AddWithValue("@Validade", product.Validate);

                        // Minimum stock parameter, typed as Double
                        var estoqueMinParam = cmd.Parameters.Add("@EstoqueMin", OleDbType.Double);
                        estoqueMinParam.Value = product.minStock;

                        // Current quantity parameter, typed as Double
                        var quantidadeParam = cmd.Parameters.Add("@Quantidade", OleDbType.Double);
                        quantidadeParam.Value = product.Amount;

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
                    // Ensure the connection is closed
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Updates the current stock quantity (QuantidadeAtual) for an existing product by adding the specified amount.
        /// </summary>
        /// <param name="codBar">The barcode of the product to update.</param>
        /// <param name="amount">The quantity to be added to the current stock.</param>
        public void UpdateProduct(string codBar, decimal amount)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();

                    // SQL query to increment the existing quantity
                    string query = "UPDATE Produtos SET QuantidadeAtual = QuantidadeAtual + @Quantidade " +
                                   "WHERE CodBarras = @CodBarras";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // The amount to be added
                        var newAmount = cmd.Parameters.Add("@Quantidade", OleDbType.Double);
                        newAmount.Value = amount;

                        // The identifying barcode
                        cmd.Parameters.AddWithValue("@CodBarras", codBar);

                        int linhasAfetadas = cmd.ExecuteNonQuery(); // Get number of rows affected

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
                    // Ensure the connection is closed
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Changes the internal product's Barcode and checks the database for an existing product with that ID.
        /// If found, it populates the associated UI controls and sets the form mode to Update.
        /// </summary>
        /// <param name="value">The new barcode value.</param>
        /// <param name="nameTB">The TextBox for the product name.</param>
        /// <param name="priceNUD">The NumericUpDown for the price.</param>
        /// <param name="minStockNUD">The NumericUpDown for the minimum stock.</param>
        /// <param name="validateDTP">The DateTimePicker for the expiration date.</param>
        public void ChangeID(string value, TextBox nameTB, NumericUpDown priceNUD,
                             NumericUpDown minStockNUD, DateTimePicker validateDTP)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    // Query to retrieve product details based on barcode
                    string query = "SELECT Nome, Preco, Validade, EstoqueMinimo " +
                                   "FROM Produtos WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@CodBarras", value);

                        using (OleDbDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                // Product found: populate internal object and UI controls
                                product.Name = reader["Nome"].ToString();
                                // Handle potential DBNull values and conversion
                                product.Value = reader["preco"] != DBNull.Value ? Convert.ToDecimal(reader["Preco"]) : 0;
                                product.minStock = reader["EstoqueMinimo"] != DBNull.Value ? Convert.ToDecimal(reader["EstoqueMinimo"]) : 0;
                                product.Validate = reader["Validade"] != DBNull.Value ? Convert.ToDateTime(reader["Validade"]) : DateTime.Now;

                                nameTB.Text = product.Name;
                                priceNUD.Value = product.Value;
                                minStockNUD.Value = product.minStock;
                                validateDTP.Value = product.Validate;

                                // Set form flag to indicate update mode
                                RegisterForm.instance.isNewProduct = false;
                            }
                            else
                            {
                                // Product not found: store the ID for new registration
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
                    // Ensure the connection is closed
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                }
            }
        }

        /// <summary>
        /// Sets the name of the internal product object if the value is not empty.
        /// </summary>
        public void ChangeName(string value)
        {
            if (value != string.Empty) product.Name = value;
        }

        /// <summary>
        /// Sets the value/price of the internal product object if the value is greater than zero.
        /// </summary>
        public void ChangeValue(decimal value)
        {
            if (value > 0) product.Value = value;
        }

        /// <summary>
        /// Sets the expiration date of the internal product object if the date is in the future.
        /// </summary>
        public void ChangeValidate(DateTime date)
        {
            if (date > DateTime.Now) product.Validate = date;
        }

        /// <summary>
        /// Sets the minimum stock level of the internal product object if the value is greater than zero.
        /// </summary>
        public void ChangeStockMin(decimal value)
        {
            if (value > 0) product.minStock = value;
        }

        /// <summary>
        /// Sets the current stock amount of the internal product object if the value is greater than zero.
        /// </summary>
        public void ChangeAmount(decimal value)
        {
            if (value > 0) product.Amount = value;
        }

        /// <summary>
        /// Sets the Unit of Measure (UF) of the internal product object if the value is not empty.
        /// </summary>
        public void ChangeUF(string value)
        {
            if (value != string.Empty) product.UF = value;
        }
    }
}