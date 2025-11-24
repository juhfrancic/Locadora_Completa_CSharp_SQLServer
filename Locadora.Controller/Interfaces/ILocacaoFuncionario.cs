using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Controller.Interfaces
{
    public interface ILocacaoFuncionario
    {
        public void AdicionarLocacaoFuncionario(int locacaoId, int funcionarioId, SqlConnection connection, SqlTransaction transaction);
        public void AdicionarLocacaoFuncionario(int locacaoId, int funcionarioId);
        public List<Locacao> ListarLocacoesPorFuncionario(string email);
        public List<Funcionario> ListarFuncionariosPorLocacao(int locacaoId);
        public void DeletarLocacaoFuncionario(int locacaoId, int funcionarioId);
    }
}