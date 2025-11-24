using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;
using Utils.Databases;

namespace Locadora.Controller
{
    public class LocacaoController : ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao, List<string> emailsFuncionarios)
        {
            var funcionarioController = new FuncionarioController();

            foreach (var email in emailsFuncionarios)
            {
                var funcionario = funcionarioController.BuscarFuncionarioPorEmail(email);

                if (funcionario is null)
                    throw new Exception($"Funcionario com o email {email} não encontrado!");

                locacao.FuncionariosEnvolvidos.Add(funcionario);
            }
            var veiculoController = new VeiculoController();
            Veiculo veiculo = veiculoController.BuscarVeiculoPorId(locacao.VeiculoID);

            if (veiculo is null)
                throw new Exception("Veiculo não encontrado!");

            if (veiculo.StatusVeiculo != EStatusVeiculo.Disponivel)
                throw new Exception("Veículo não está disponível para a locação!");

            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Locacao.INSERTLOCACAO, connection, transaction);


                    command.Parameters.AddWithValue("@ClienteID", locacao.ClienteID);
                    command.Parameters.AddWithValue("@VeiculoID", locacao.VeiculoID);
                    command.Parameters.AddWithValue("@DataLocacao", locacao.DataLocacao);
                    command.Parameters.AddWithValue("@DataDevolucaoPrevista", locacao.DataDevolucaoPrevista);
                    command.Parameters.AddWithValue("@ValorDiaria", locacao.ValorDiaria);
                    command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                    command.Parameters.AddWithValue("@Status", locacao.Status.ToString());
                    command.Parameters.AddWithValue("@Multa", locacao.Multa);

                    int locacaoId = Convert.ToInt32(command.ExecuteScalar());
                    locacao.setLocacaoId(locacaoId);


                    var lfController = new LocacaoFuncionarioController();

                    foreach (var funcionario in locacao.FuncionariosEnvolvidos)
                    {
                        lfController.AdicionarLocacaoFuncionario(
                                locacaoId,
                                funcionario.FuncionarioID,
                                connection,
                                transaction
                        );
                    }

                    veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Alugado.ToString(), veiculo.Placa, connection, transaction);
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar locação: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar locação: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }

        public List<Locacao> ListarTodasLocacoes()
        {
            List<Locacao> locacoes = new List<Locacao>();
            var clienteController = new ClienteController();
            var veiculoController = new VeiculoController();

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(Locacao.LISTARLOCACOES, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Locacao locacao = new Locacao(
                            Convert.ToInt32(reader["LocacaoID"]),
                            Convert.ToInt32(reader["ClienteID"]),
                            Convert.ToInt32(reader["VeiculoID"]),
                            Convert.ToDateTime(reader["DataLocacao"]),
                            Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                            reader["DataDevolucaoReal"] == DBNull.Value ? null : Convert.ToDateTime(reader["DataDevolucaoReal"]),
                            Convert.ToDecimal(reader["ValorDiaria"]),
                            Convert.ToDecimal(reader["ValorTotal"]),
                            Convert.ToDecimal(reader["Multa"]),
                            Enum.Parse<EStatusLocacao>(reader["Status"].ToString())
                        );
                        locacoes.Add(locacao);
                    }
                    reader.Close();

                    foreach (var locacao in locacoes)
                    {
                        var cliente = clienteController.BuscarClientePorId(locacao.ClienteID);
                        if (cliente != null)
                            locacao.setNomeCliente(cliente.Nome);

                        var veiculo = veiculoController.BuscarVeiculoPorId(locacao.VeiculoID);
                        if (veiculo != null)
                            locacao.setModeloVeiculo(veiculo.Modelo);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar locações: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro insperado ao listar locacoes: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return locacoes;

            }
        }

        public Locacao BuscarLocacaoPorID(int locacaoID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                Locacao locacao = null;
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(Locacao.BUSCARLOCACAOPORID, connection);
                    command.Parameters.AddWithValue("@LocacaoID", locacaoID);

                    SqlDataReader reader = command.ExecuteReader();

                    if (reader.Read())
                    {
                        locacao = new Locacao(
                            Convert.ToInt32(reader["LocacaoID"]),
                            Convert.ToInt32(reader["ClienteID"]),
                            Convert.ToInt32(reader["VeiculoID"]),
                            Convert.ToDateTime(reader["DataLocacao"]),
                            Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                            reader["DataDevolucaoReal"] == DBNull.Value ? null : Convert.ToDateTime(reader["DataDevolucaoReal"]),
                            Convert.ToDecimal(reader["ValorDiaria"]),
                            Convert.ToDecimal(reader["ValorTotal"]),
                            Convert.ToDecimal(reader["Multa"]),
                            Enum.Parse<EStatusLocacao>(reader["Status"].ToString())
                        );
                    }

                    reader.Close();

                    if (locacao != null)
                    {
                        var clienteController = new ClienteController();
                        var cliente = clienteController.BuscarClientePorId(locacao.ClienteID);
                        if (cliente != null)
                            locacao.setNomeCliente(cliente.Nome);

                        var veiculoController = new VeiculoController();
                        var veiculo = veiculoController.BuscarVeiculoPorId(locacao.VeiculoID);
                        if (veiculo != null)
                            locacao.setModeloVeiculo(veiculo.Modelo);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar locação por id: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro insperado ao buscar locação por id: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return locacao;
            }
        }

        public List<Locacao> ListarLocacoesAtivas()
        {
            List<Locacao> locacoes = new List<Locacao>();
            var clienteController = new ClienteController();
            var veiculoController = new VeiculoController();

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(Locacao.LISTARLOCACOESATIVAS, connection);

                    SqlDataReader reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        Locacao locacao = new Locacao(
                            Convert.ToInt32(reader["LocacaoID"]),
                            Convert.ToInt32(reader["ClienteID"]),
                            Convert.ToInt32(reader["VeiculoID"]),
                            Convert.ToDateTime(reader["DataLocacao"]),
                            Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                            reader["DataDevolucaoReal"] == DBNull.Value ? null : Convert.ToDateTime(reader["DataDevolucaoReal"]),
                            Convert.ToDecimal(reader["ValorDiaria"]),
                            Convert.ToDecimal(reader["ValorTotal"]),
                            Convert.ToDecimal(reader["Multa"]),
                            Enum.Parse<EStatusLocacao>(reader["Status"].ToString())
                        );
                        locacoes.Add(locacao);
                    }
                    reader.Close();

                    foreach (var locacao in locacoes)
                    {
                        var cliente = clienteController.BuscarClientePorId(locacao.ClienteID);
                        if (cliente != null)
                            locacao.setNomeCliente(cliente.Nome);
                        var veiculo = veiculoController.BuscarVeiculoPorId(locacao.VeiculoID);
                        if (veiculo != null)
                            locacao.setModeloVeiculo(veiculo.Modelo);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao listar locações ativas: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro insperado ao listar locações ativas: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
            return locacoes;
        }
        public List<Locacao> BuscarLocacoesPorClienteID(int clienteID)
        {
            List<Locacao> locacoes = new List<Locacao>();
            var clienteController = new ClienteController();
            var veiculoController = new VeiculoController();

            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                try
                {
                    connection.Open();
                    SqlCommand command = new SqlCommand(Locacao.BUSCARLOCACOESPORCLIENTEID, connection);
                    command.Parameters.AddWithValue("@ClienteID", clienteID);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Locacao locacao = new Locacao(
                            Convert.ToInt32(reader["LocacaoID"]),
                            Convert.ToInt32(reader["ClienteID"]),
                            Convert.ToInt32(reader["VeiculoID"]),
                            Convert.ToDateTime(reader["DataLocacao"]),
                            Convert.ToDateTime(reader["DataDevolucaoPrevista"]),
                            reader["DataDevolucaoReal"] == DBNull.Value ? null : Convert.ToDateTime(reader["DataDevolucaoReal"]),
                            Convert.ToDecimal(reader["ValorDiaria"]),
                            Convert.ToDecimal(reader["ValorTotal"]),
                            Convert.ToDecimal(reader["Multa"]),
                            Enum.Parse<EStatusLocacao>(reader["Status"].ToString())
                        );
                        locacoes.Add(locacao);
                    }
                    reader.Close();

                    foreach (var locacao in locacoes)
                    {
                        var cliente = clienteController.BuscarClientePorId(locacao.ClienteID);
                        if (cliente != null)
                            locacao.setNomeCliente(cliente.Nome);
                        var veiculo = veiculoController.BuscarVeiculoPorId(locacao.VeiculoID);
                        if (veiculo != null)
                            locacao.setModeloVeiculo(veiculo.Modelo);
                    }
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao buscar locações por cliente id: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro insperado ao buscar locações por cliente id: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
                return locacoes;
            }
        }

        public void CancelarLocacao(int locacaoID)
        {
            using (SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString()))
            {
                connection.Open();
                using (var transaction = connection.BeginTransaction())
                {
                    try
                    {
                        SqlCommand command = new SqlCommand(Locacao.UPDATESTATUSLOCACAOCANCELADA, connection, transaction);
                        command.Parameters.AddWithValue("@LocacaoID", locacaoID);
                        command.Parameters.AddWithValue("@Status", EStatusLocacao.Cancelada.ToString());

                        command.ExecuteNonQuery();
                        transaction.Commit();
                    }
                    catch (SqlException ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao cancelar locacao: " + ex.Message);
                    }
                    catch (Exception ex)
                    {
                        transaction.Rollback();
                        throw new Exception("Erro ao cancelar locacao: " + ex.Message);
                    }
                }
            }
        }

        public void AtualizarLocacao(Locacao locacao, SqlConnection connection, SqlTransaction transaction)
        {
            {
                try
                {
                    SqlCommand command = new SqlCommand(Locacao.ATUALIZARLOCACAO, connection, transaction);

                    command.Parameters.AddWithValue("@LocacaoID", locacao.LocacaoID);
                    command.Parameters.AddWithValue("@DataDevolucaoReal", locacao.DataDevolucaoReal
                                                    ?? (object)DBNull.Value);
                    command.Parameters.AddWithValue("@ValorTotal", locacao.ValorTotal);
                    command.Parameters.AddWithValue("@Multa", locacao.Multa);
                    command.Parameters.AddWithValue("@Status", locacao.Status.ToString());

                    command.ExecuteNonQuery();
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao atualizar locacao: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao atualizar locacao: " + ex.Message);
                }
            }
        }

        public void ProcessarDevolucao(int locacaoID, DateTime dataDevolucao)
        {
            Locacao locacao = BuscarLocacaoPorID(locacaoID);
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();

            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    if (locacao is null)
                        throw new Exception("Locacão não encontrada para este id!");

                    if (locacao.Status != EStatusLocacao.Ativa)
                        throw new Exception("Locacação ja foi concluida ou cancelada!");

                    locacao.RegistrarDevolucao(dataDevolucao);


                    AtualizarLocacao(locacao, connection, transaction);

                    var veiculoController = new VeiculoController();
                    Veiculo veiculo = veiculoController.BuscarVeiculoPorId(locacao.VeiculoID);
                    veiculoController.AtualizarStatusVeiculo(EStatusVeiculo.Disponivel.ToString(), veiculo.Placa, connection, transaction);

                    transaction.Commit();
                    Console.WriteLine($"Processo de devolução realizado com sucesso! Valor Total: {locacao.ValorTotal:C}.\nValor da multa: {locacao.Multa:C}");
                }
                catch (SqlException ex)
                {
                    throw new Exception("Erro ao processar devolução da locacao: " + ex.Message);
                }
                catch (Exception ex)
                {
                    throw new Exception("Erro inesperado ao processar a devolução da locacao: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
    }
}