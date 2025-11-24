using Locadora.Controller.Interfaces;
using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Utils.Databases;

namespace Locadora.Controller
{
    public class FuncionarioController : IFuncionarioController
    {
        public void AdicionarFuncionario(Funcionario funcionario)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            using (SqlTransaction transaction = connection.BeginTransaction())
            {
                try
                {
                    SqlCommand command = new SqlCommand(Funcionario.INSERTFUNCIONARIO, connection, transaction);
                    command.Parameters.AddWithValue("@Nome", funcionario.Nome);
                    command.Parameters.AddWithValue("@CPF", funcionario.CPF);
                    command.Parameters.AddWithValue("@Email", funcionario.Email);
                    command.Parameters.AddWithValue("@Salario", funcionario.Salario ?? (object)DBNull.Value);
                    int funcionarioID = Convert.ToInt32(command.ExecuteScalar());
                    funcionario.setFuncionarioID(funcionarioID);
                    transaction.Commit();
                }
                catch (SqlException ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro ao adicionar funcionário: " + ex.Message);
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Erro inesperado ao adicionar funcionário: " + ex.Message);
                }
                finally
                {
                    connection.Close();
                }
            }
        }
        public void AtualizarFuncionario(string email, string nome, decimal salario)
        {
            var funcionarioEncontrado = BuscarFuncionarioPorEmail(email);
            if (funcionarioEncontrado is null)
                throw new Exception("Não existe funcionário cadastrado com esse email!");
            funcionarioEncontrado.setNome(nome);
            funcionarioEncontrado.setSalario(salario);
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(Funcionario.UPDATEFUNCIONARIO, connection);
                command.Parameters.AddWithValue("@Nome", funcionarioEncontrado.Nome);
                command.Parameters.AddWithValue("@Salario", funcionarioEncontrado.Salario);
                command.Parameters.AddWithValue("@IdFuncionario", funcionarioEncontrado.FuncionarioID);
                command.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao atualizar dados do funcionário: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao atualizar funcionário: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public Funcionario BuscarFuncionarioPorEmail(string email)
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                SqlCommand command = new SqlCommand(Funcionario.SELECTFUNCIONARIOPOREMAIL, connection);
                command.Parameters.AddWithValue("@Email", email);
                SqlDataReader reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var funcionario = new Funcionario(
                        reader["Nome"].ToString(),
                        reader["CPF"].ToString(),
                        reader["Email"].ToString(),
                        reader["Salario"] != DBNull.Value ? Convert.ToDecimal(reader["Salario"]) : 0m
                    );
                    funcionario.setFuncionarioID(Convert.ToInt32(reader["FuncionarioID"]));
                    return funcionario;
                }
                return null;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao buscar funcionário por email: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao buscar funcionário por email: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public void DeletarFuncionario(string email)
        {
            var funcionarioEncontrado = BuscarFuncionarioPorEmail(email);
            if (funcionarioEncontrado is null)
                throw new Exception("Funcionário não encontrado!");
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            connection.Open();
            try
            {
                // verifica locacoes que estao ativas
                SqlCommand checkCommand = new SqlCommand(Funcionario.CHECKLOCACOESATIVAS, connection);
                checkCommand.Parameters.AddWithValue("@IdFuncionario", funcionarioEncontrado.FuncionarioID);
                int count = Convert.ToInt32(checkCommand.ExecuteScalar());
                if (count > 0)
                    throw new Exception("Funcionário esta sendo associado com locaçoes ativas e não pode ser deletado.");
                // aqui deleta
                SqlCommand deleteCommand = new SqlCommand(Funcionario.DELETEFUNCIONARIO, connection);
                deleteCommand.Parameters.AddWithValue("@IdFuncionario", funcionarioEncontrado.FuncionarioID);
                deleteCommand.ExecuteNonQuery();
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao deletar funcionário: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro inesperado ao deletar funcionário: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
        public List<Funcionario> ListarTodosFuncionarios()
        {
            SqlConnection connection = new SqlConnection(ConnectionDB.GetConnectionString());
            try
            {
                connection.Open();
                SqlCommand command = new SqlCommand(Funcionario.SELECTALLFUNCIONARIOS, connection);
                SqlDataReader reader = command.ExecuteReader();
                List<Funcionario> listaFuncionario = new List<Funcionario>();
                while (reader.Read())
                {
                    var funcionario = new Funcionario(
                        reader["Nome"].ToString(),
                        reader["CPF"].ToString(),
                        reader["Email"].ToString(),
                        reader["Salario"] != DBNull.Value ? Convert.ToDecimal(reader["Salario"]) : 0m
                    );
                    funcionario.setFuncionarioID(Convert.ToInt32(reader["FuncionarioID"]));
                    listaFuncionario.Add(funcionario);
                }
                return listaFuncionario;
            }
            catch (SqlException ex)
            {
                throw new Exception("Erro ao listar funcionários: " + ex.Message);
            }
            catch (Exception ex)
            {
                throw new Exception("Erro ao listar funcionários: " + ex.Message);
            }
            finally
            {
                connection.Close();
            }
        }
    }
}