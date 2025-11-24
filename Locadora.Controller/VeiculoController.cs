using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;

using Utils.Databases;


namespace Locadora.Controller
{
    public class VeiculoController : IVeiculoController
    {
        public void AdicionarVeiculo(Veiculo veiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Veiculo.INSERTVEICULO, connection, transaction);

                    command.Parameters.AddWithValue("@CategoriaID", veiculo.CategoriaID);
                    command.Parameters.AddWithValue("@Placa", veiculo.Placa);
                    command.Parameters.AddWithValue("@Marca", veiculo.Marca);
                    command.Parameters.AddWithValue("@Modelo", veiculo.Modelo);
                    command.Parameters.AddWithValue("@Ano", veiculo.Ano);
                    command.Parameters.AddWithValue("@StatusVeiculo", veiculo.StatusVeiculo.ToString());

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar veículo no banco de dados: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar veículo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public void AtualizarStatusVeiculo(string statusVeiculo, string placa, SqlConnection connection, SqlTransaction transaction)
        {
            Veiculo veiculo = BuscarVeiculoPlaca(placa) ??
                throw new Exception("Veículo não encontrado para atualizar status.");

            {
                SqlCommand command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction);
                try
                {
                    command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                    command.Parameters.AddWithValue("@IdVeiculo", veiculo.VeiculoID);

                    command.ExecuteNonQuery();

                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao atualizar status do veículo no banco de dados: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao atualizar status do veículo: " + ex.Message);
                }
            }
        }

        public Veiculo BuscarVeiculoPlaca(string placa)
        {

            var categoriaController = new CategoriaController();

            Veiculo veiculo = null;

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOBYPLACA, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@Placa", placa);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EStatusVeiculo status = Enum.Parse<EStatusVeiculo>(reader["StatusVeiculo"].ToString());
                            veiculo = new Veiculo(
                                                reader.GetInt32(1),
                                                reader.GetString(2),
                                                reader.GetString(3),
                                                reader.GetString(4),
                                                reader.GetInt32(5),
                                                status
                                                );

                            veiculo.setVeiculoID(reader.GetInt32(0));

                            veiculo.setNomeCategoria(categoriaController.BuscarCategoriaPorId(veiculo.CategoriaID).Nome);
                        }

                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao encontrar veículo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao encontrar veículo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return veiculo ?? throw new Exception("Veículo não encontrado");
            }
        }

        public Veiculo BuscarVeiculoPorId(int veiculoId)
        {
            var categoriaController = new CategoriaController();
            Veiculo veiculo = null;

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOBYID, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@VeiculoID", veiculoId);

                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EStatusVeiculo status = Enum.Parse<EStatusVeiculo>(reader["StatusVeiculo"].ToString());
                            veiculo = new Veiculo(
                                reader.GetInt32(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetInt32(5),
                                status
                            );
                            veiculo.setVeiculoID(reader.GetInt32(0));

                            veiculo.setNomeCategoria(categoriaController.BuscarCategoriaPorId(veiculo.CategoriaID).Nome);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao encontrar veículo: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao encontrar veículo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }

            return veiculo ?? throw new Exception("Veículo não encontrado");
        }

        public List<Veiculo> BuscarVeiculosPorCategoria(int categoriaId)
        {
            var veiculos = new List<Veiculo>();
            var categoriaController = new CategoriaController();
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlCommand command = new SqlCommand(Veiculo.SELECTVEICULOSBYCATEGORIA, connection))
            {
                try
                {
                    command.Parameters.AddWithValue("@CategoriaID", categoriaId);
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EStatusVeiculo status = Enum.Parse<EStatusVeiculo>(reader["StatusVeiculo"].ToString());
                            Veiculo veiculo = new Veiculo(
                                reader.GetInt32(1),
                                reader.GetString(2),
                                reader.GetString(3),
                                reader.GetString(4),
                                reader.GetInt32(5),
                                status
                            );
                            veiculo.setVeiculoID(reader.GetInt32(0));
                            veiculo.setNomeCategoria(categoriaController.BuscarCategoriaPorId(veiculo.CategoriaID).Nome);
                            veiculos.Add(veiculo);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar veículos por categoria: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao buscar veículos por categoria: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return veiculos;
        }

        public void AtualizarStatusVeiculo(string statusVeiculo, string placa)
        {
            Veiculo veiculo = BuscarVeiculoPlaca(placa) ??
                throw new Exception("Veículo não encontrado para atualizar status.");
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Veiculo.UPDATESTATUSVEICULO, connection, transaction);
                try
                {
                    command.Parameters.AddWithValue("@StatusVeiculo", statusVeiculo);
                    command.Parameters.AddWithValue("@IdVeiculo", veiculo.VeiculoID);
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar status do veículo no banco de dados: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao atualizar status do veículo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }







        public void DeletarVeiculo(int idVeiculo)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();


            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                SqlCommand command = new SqlCommand(Veiculo.DELETEVEICULO, connection, transaction);
                try
                {
                    command.Parameters.AddWithValue("@IdVeiculo", idVeiculo);

                    command.ExecuteNonQuery();

                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar veículo do banco de dados: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao deletar veículo: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

            }

        }

        public List<Veiculo> ListarTodosVeiculos()
        {
            var veiculos = new List<Veiculo>();
            var categoriaController = new CategoriaController();

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());

            connection.Open();

            using (SqlCommand command = new SqlCommand(Veiculo.SELECTALLVEICULOS, connection))
            {
                try
                {
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            EStatusVeiculo status = Enum.Parse<EStatusVeiculo>(reader["StatusVeiculo"].ToString());
                            Veiculo veiculo = new Veiculo(
                                    reader.GetInt32(0),
                                    reader.GetString(1),
                                    reader.GetString(2),
                                    reader.GetString(3),
                                    reader.GetInt32(4),
                                    status
                                );

                            veiculo.setNomeCategoria(categoriaController.BuscarCategoriaPorId(veiculo.CategoriaID).Nome);

                            veiculos.Add(veiculo);
                        }
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar veículos do banco de dados: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro ao listar veículos: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }

                return veiculos;
            }

        }

    }
}