using Locadora.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Controller.Interfaces
{
    public interface IFuncionarioController
    {
        public interface IFuncionarioController
        {
            public void AdicionarFuncionario(Funcionario funcionario);
            public List<Funcionario> ListarTodosFuncionarios();
            public Funcionario BuscarFuncionarioPorEmail(string email);
            public void AtualizarFuncionario(string email, string nome, decimal salario);
            public void DeletarFuncionario(string email);
        }
    }
}