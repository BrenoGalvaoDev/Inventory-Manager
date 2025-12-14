using System;
using System.Collections.Generic;
using System.Data.OleDb; // Necessary for interacting with the Access database
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
    /// This class handles all database communication related to product management,
    /// including updating, changing names, and deleting items from the 'Produtos' table.
    /// Operations are encapsulated within asynchronous Tasks for better responsiveness.
    /// </summary>
    public class ManageItems
    {
        // --- Database Path Configuration ---

        /// <summary>
        /// Gets the path to the current user's local application data folder.
        /// </summary>
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);

        /// <summary>
        /// Defines the subfolder where the Access database is located.
        /// </summary>
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");

        /// <summary>
        /// Full path to the Access database file.
        /// </summary>
        static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        /// <summary>
        /// The OLE DB connection string for connecting to the Access database.
        /// </summary>
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

        /// <summary>
        /// Constructor for ManageItems. Sets the application's culture.
        /// </summary>
        public ManageItems()
        {
            // Set the thread culture to Brazilian Portuguese for consistent data handling.
            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        /// <summary>
        /// Asynchronously updates all editable fields for an existing product in the database 
        /// based on its Barcode (CodBarras).
        /// </summary>
        /// <param name="product">The Product object containing the updated data.</param>
        /// <returns>A Task representing the operation, returning true if the update was successful (1 or more rows affected).</returns>
        public Task<bool> UpdateItem(Product product)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    // SQL query to update all relevant fields, identified by the product's barcode
                    string query = "UPDATE Produtos SET Nome = @Nome, UF = @UF, Preco = @Preco, Validade = @Validade, EstoqueMinimo = @EstoqueMinimo, QuantidadeAtual = @QuantidadeAtual " +
                                   "WHERE CodBarras = @CodBarras";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Add parameters for the fields being updated
                        cmd.Parameters.AddWithValue("@Nome", product.Name);
                        cmd.Parameters.AddWithValue("@UF", product.UF);

                        // Price parameter, specifically typed as Currency for accuracy in Access
                        var valorParam = cmd.Parameters.Add("@Preco", OleDbType.Currency);
                        valorParam.Value = product.Value;

                        cmd.Parameters.AddWithValue("@Validade", product.Validate);

                        // Minimum stock parameter, typed as Double to support decimal values
                        var estoqueMinParam = cmd.Parameters.Add("@EstoqueMin", OleDbType.Double);
                        estoqueMinParam.Value = product.minStock;

                        // Current quantity parameter, typed as Double to support decimal values
                        var quantidadeParam = cmd.Parameters.Add("@Quantidade", OleDbType.Double);
                        quantidadeParam.Value = product.Amount;

                        // Barcode parameter used for the WHERE clause
                        cmd.Parameters.AddWithValue("@CodBarras", product.Barcode);

                        // Execute the command, retrieving the number of rows affected
                        int index = cmd.ExecuteNonQuery();

                        // Determine the result based on the number of rows affected
                        if (index > 0)
                        {
                            return Task.FromResult(true); // Update successful
                        }
                        else
                        {
                            return Task.FromResult(false); // No rows affected (item not found or no change)
                        }

                    }
                }
                catch (OleDbException ex)
                {
                    // Handle and display database errors during the update
                    MessageBox.Show($"Erro ao atualizar produto: {ex.Message}");
                    return Task.FromResult(false); // Update failed due to error
                }
            }
        }

        /// <summary>
        /// Asynchronously changes only the name of a product identified by its Barcode (CodBarras).
        /// </summary>
        /// <param name="newName">The new name for the product.</param>
        /// <param name="id">The barcode of the product to update.</param>
        /// <returns>A Task representing the operation, returning true if the update was successful.</returns>
        public Task<bool> ChangeItemName(string newName, string id)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    // SQL query to update only the Nome field
                    string query = "UPDATE Produtos SET Nome = @Nome " + "WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Parameters for the new name and the identifying barcode
                        cmd.Parameters.AddWithValue("@Nome", newName);
                        cmd.Parameters.AddWithValue("@CodBarras", id);

                        int index = cmd.ExecuteNonQuery();

                        // Check if the operation affected any rows
                        if (index > 0)
                        {
                            return Task.FromResult(true);
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle and display database errors during the name update
                    MessageBox.Show($"Erro ao atualizar Nome: {ex.Message}");
                    return Task.FromResult(false);
                }
            }
        }

        /// <summary>
        /// Asynchronously deletes a product from the database using its Barcode (CodBarras).
        /// </summary>
        /// <param name="id">The barcode of the product to delete.</param>
        /// <returns>A Task representing the operation, returning true if the deletion was successful.</returns>
        public Task<bool> DeleteItem(string id)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    // SQL query to delete the record identified by CodBarras
                    string query = "DELETE FROM Produtos WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        // Parameter for the identifying barcode
                        cmd.Parameters.AddWithValue("CodBarras", id);

                        int index = cmd.ExecuteNonQuery();

                        // Check if the operation affected any rows
                        if (index > 0)
                        {
                            return Task.FromResult(true);
                        }
                        else
                        {
                            return Task.FromResult(false);
                        }
                    }
                }
                catch (OleDbException ex)
                {
                    // Handle and display database errors during the deletion
                    MessageBox.Show($"Erro de banco de dados: {ex.Message}");
                    return Task.FromResult(false);
                }
            }
        }
    }
}