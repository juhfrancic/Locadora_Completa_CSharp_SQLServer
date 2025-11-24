
using Locadora.Models;
using Microsoft.Data.SqlClient;
using Utils.Databases;


namespace Locadora.Controller
{
    public class CategoriaController
    {
        public void AdicionarCategoria(Categoria categoria)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    var command = connection.CreateCommand();
                    command.Transaction = transaction;
                    command.CommandText = @"INSERT INTO dbo.tblCategorias (Nome, Descricao) 
                                            VALUES (@Nome, @Descricao)
                                            SELECT SCOPE_IDENTITY()";

                    command.Parameters.AddWithValue("@Nome", categoria.Nome);
                    command.Parameters.AddWithValue("@Descricao", categoria.Descricao ?? (object)DBNull.Value);
                    int categoriaId = Convert.ToInt32(command.ExecuteScalar());
                    categoria.setCategoriaId(categoriaId);
                    transaction.Commit();
                }
                catch (Exception)
                {
                    transaction.Rollback();
                    throw;
                }
            }


        }

        public List<Categoria> ListarCategorias()
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            try
            {
                connection.Open();

                SqlCommand command = new SqlCommand(Categoria.SELECTALLCATEGORIAS, connection);

                SqlDataReader reader = command.ExecuteReader();
                List<Categoria> categorias = new List<Categoria>();
                while (reader.Read())
                {
                    Categoria categoria = new Categoria(
                        reader["Nome"].ToString(),
                        reader["Descricao"].ToString());
                    categoria.setCategoriaId(Convert.ToInt32(reader["CategoriaId"]));

                    categorias.Add(categoria);
                }
                reader.Close();
                return categorias;
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar categorias: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        public Categoria BuscarCategoriaPorId(int categoriaId)
        {
            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand("SELECT CategoriaId, Nome, Descricao FROM dbo.tblCategorias WHERE CategoriaId = @CategoriaId;", connection);
                command.Parameters.AddWithValue("@CategoriaId", categoriaId);

                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    Categoria categoria = new Categoria(
                        reader["Nome"].ToString(),
                        reader["Descricao"].ToString());
                    categoria.setCategoriaId(Convert.ToInt32(reader["CategoriaId"]));
                    reader.Close();
                    return categoria;
                }
                else
                {
                    reader.Close();
                    return null;
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao buscar categoria por ID: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }


        public void UpdateCategoria(int categoriaID, string nomeAtualizada, string descricaoAtualizada)
        {
            var categoriaEncontrada = BuscarCategoriaPorId(categoriaID);
            if (categoriaEncontrada is null)
                throw new Exception("Não existe categoria cadastrada com esse ID!");

            categoriaEncontrada.setDescricao(descricaoAtualizada);
            categoriaEncontrada.setNome(nomeAtualizada);

            var connection = new SqlConnection(ConnectionDB.GetConnectionString());


            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Categoria.UPDATECATEGORIA, connection);
                command.Parameters.AddWithValue("@Nome", categoriaEncontrada.Nome);
                command.Parameters.AddWithValue("@Descricao", categoriaEncontrada.Descricao);
                command.Parameters.AddWithValue("@CategoriaId", categoriaEncontrada.CategoriaId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar categoria: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }

        

        public void DeleteCategoria(int categoriaId)
        {
            var categoriaEncontrada = BuscarCategoriaPorId(categoriaId);
            if (categoriaEncontrada is null)
                throw new Exception("Não existe categoria cadastrada com esse ID!");

            var connection = new SqlConnection(ConnectionDB.GetConnectionString());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Categoria.DELETECATEGORIA, connection);
                command.Parameters.AddWithValue("@CategoriaId",categoriaEncontrada.CategoriaId);
                command.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao deletar categoria: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}
