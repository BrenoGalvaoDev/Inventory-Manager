using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    public class ManageItems
    {
        static string localAppData = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
        static string pastaBanco = Path.Combine(localAppData, "GerenciadorDeEstoque");
        static string dbPath = Path.Combine(pastaBanco, "EstoquePaiol.accdb");

        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={dbPath};";

        public ManageItems()
        {
            CultureInfo culture = new CultureInfo("pt-BR");
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
        }

        public Task<bool> UpdateItem(Product product)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Produtos SET Nome = @Nome, UF = @UF, Preco = @Preco, Validade = @Validade, EstoqueMinimo = @EstoqueMinimo, QuantidadeAtual = @QuantidadeAtual " +
                                   "WHERE CodBarras = @CodBarras";
                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", product.Name);
                        cmd.Parameters.AddWithValue("@UF", product.UF);

                        var valorParam = cmd.Parameters.Add("@Preco", OleDbType.Currency);
                        valorParam.Value = product.Value;

                        cmd.Parameters.AddWithValue("@Validade", product.Validate);

                        var estoqueMinParam = cmd.Parameters.Add("@EstoqueMin", OleDbType.Double);
                        estoqueMinParam.Value = product.minStock;

                        var quantidadeParam = cmd.Parameters.Add("@Quantidade", OleDbType.Double);
                        quantidadeParam.Value = product.Amount;

                        cmd.Parameters.AddWithValue("@CodBarras", product.Barcode);
                        cmd.ExecuteNonQuery();


                        int index = cmd.ExecuteNonQuery();
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
                    MessageBox.Show($"Erro ao atualizar produto: {ex.Message}");
                    return Task.FromResult(false);
                }
            }
        }

        public Task<bool> ChangeItemName(string newName, string id)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "UPDATE Produtos SET Nome = @Nome " + "WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@Nome", newName);
                        cmd.Parameters.AddWithValue("@CodBarras", id);

                        int index = cmd.ExecuteNonQuery();
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
                    MessageBox.Show($"Erro ao atualizar Nome: {ex.Message}");
                    return Task.FromResult(false);
                }
            }
        }

        public Task<bool> DeleteItem(string id)
        {
            using (OleDbConnection conn = new OleDbConnection(connString))
            {
                try
                {
                    conn.Open();
                    string query = "DELETE FROM Produtos WHERE CodBarras = @CodBarras";

                    using (OleDbCommand cmd = new OleDbCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("CodBarras", id);
                        int index = cmd.ExecuteNonQuery();
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
                    MessageBox.Show($"Erro de banco de dados: {ex.Message}");
                    return Task.FromResult(false);
                }
            }
        }
    }
}
