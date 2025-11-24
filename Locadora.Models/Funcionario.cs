using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Funcionario
    {
        public readonly static string INSERTFUNCIONARIO = @"INSERT INTO tblFuncionarios (Nome, CPF, Email, Salario)
                                                            VALUES (@Nome, @CPF, @Email, @Salario);
                                                            SELECT SCOPE_IDENTITY()";
        public readonly static string SELECTALLFUNCIONARIOS = @"SELECT f.FuncionarioID, f.Nome, f.CPF, f.Email, f.Salario
                                                                FROM tblFuncionarios f";
        public readonly static string SELECTFUNCIONARIOPOREMAIL = @"SELECT f.FuncionarioID, f.Nome, f.CPF, f.Email, f.Salario
                                                                    FROM tblFuncionarios f
                                                                    WHERE f.Email = @Email";
        public readonly static string UPDATEFUNCIONARIO = @"UPDATE tblFuncionarios
                                                            SET Nome = @Nome, Salario = @Salario
                                                            WHERE FuncionarioID = @IdFuncionario";
        public readonly static string DELETEFUNCIONARIO = @"DELETE FROM tblFuncionarios
                                                            WHERE FuncionarioID = @IdFuncionario";
        public readonly static string CHECKLOCACOESATIVAS = @"SELECT COUNT(*)
                                                            FROM tblLocacoes l
                                                            JOIN tblLocacaoFuncionarios lf ON l.LocacaoID = lf.LocacaoID
                                                            WHERE lf.FuncionarioID = @IdFuncionario AND l.Status = 'Ativa'";
        public int FuncionarioID { get; private set; }
        public string Nome { get; private set; }
        public string CPF { get; private set; }
        public string Email { get; private set; }
        public List<Locacao> LocacoesGerenciadas { get; set; } = new List<Locacao>();
        public decimal? Salario { get; private set; } = 0.0m;
        public Funcionario(string nome, string cPF, string email)
        {
            Nome = nome;
            CPF = cPF;
            Email = email;
        }
        public Funcionario(string nome, string cPF, string email, decimal salario)
            : this(nome, cPF, email)
        {
            Salario = salario;
        }
        public void setFuncionarioID(int funcionarioID)
        {
            FuncionarioID = funcionarioID;
        }
        public void setNome(string nome)
        {
            Nome = nome;
        }
        public void setSalario(decimal salario)
        {
            Salario = salario;
        }
        public override string? ToString()
        {
            return $"Nome: {Nome}\nCPF: {CPF}\nEmail: {Email}\nSalário: {Salario}";
        }
    }
}