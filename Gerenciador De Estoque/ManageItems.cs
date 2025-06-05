using System;
using System.Collections.Generic;
using System.Data.OleDb;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Gerenciador_De_Estoque
{
    public class ManageItems
    {
        string connString = $@"Provider=Microsoft.ACE.OLEDB.12.0;Data Source={Application.StartupPath}\EstoquePaiol.accdb;";

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
