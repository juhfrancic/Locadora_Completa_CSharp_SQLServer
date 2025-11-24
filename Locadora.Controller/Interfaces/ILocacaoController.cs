using Locadora.Models;
using Microsoft.Data.SqlClient;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Controller.Interfaces
{
    public interface ILocacaoController
    {
        public void AdicionarLocacao(Locacao locacao, List<string> emailsFuncionarios);
        public List<Locacao> ListarTodasLocacoes();
        public Locacao BuscarLocacaoPorID(int locacaoID);
        public List<Locacao> ListarLocacoesAtivas();
        public List<Locacao> BuscarLocacoesPorClienteID(int clienteID);
        public void AtualizarLocacao(Locacao locacao, SqlConnection connection, SqlTransaction transaction);
        public void ProcessarDevolucao(int locacaoID, DateTime dataDevolucao);
    }
}