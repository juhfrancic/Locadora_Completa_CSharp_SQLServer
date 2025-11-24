using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class LocacaoFuncionario
    {
        public readonly static string INSERTLOCACAOFUNCIONARIO = @"INSERT INTO tblLocacaoFuncionarios (LocacaoID, FuncionarioID) 
                                                              VALUES (@LocacaoID, @FuncionarioID);";

        public readonly static string DELETELOCACAOFUNCIONARIO = @"DELETE FROM tblLocacaoFuncionarios 
                                                              WHERE LocacaoId = @LocacaoId AND FuncionarioId = @FuncionarioId;";

        public readonly static string SELECTLOCACAOPORFUNCIONARIO = @"SELECT  l.LocacaoID, f.Nome, v.Modelo, l.DataLocacao, l.Status
                                                                              FROM tblLocacaoFuncionarios lf
                                                                              INNER JOIN tblFuncionarios f ON lf.FuncionarioID = f.FuncionarioID
                                                                              INNER JOIN tblLocacoes l ON lf.LocacaoID = l.LocacaoID
                                                                              INNER JOIN tblClientes c ON l.ClienteID = c.ClienteID
                                                                              INNER JOIN tblVeiculos v ON l.VeiculoID = v.VeiculoID
                                                                              WHERE f.Email = @Email;";

        public readonly static string SELECTFUNCIONARIOPORLOCACAO = @"SELECT f.FuncionarioID, f.Nome, f.Email
                                                            FROM tblLocacaoFuncionarios lf
                                                            INNER JOIN tblFuncionarios f ON lf.FuncionarioID = f.FuncionarioID
                                                            INNER JOIN tblLocacoes l ON lf.LocacaoID = l.LocacaoID
                                                            WHERE l.LocacaoID = @LocacaoID;";

        public int LocacaoFuncionarioId { get; private set; }
        public int LocacaoId { get; private set; }
        public int FuncionarioId { get; private set; }
        public LocacaoFuncionario(int locacaoId, int funcionarioId)
        {
            LocacaoId = locacaoId;
            FuncionarioId = funcionarioId;
        }


    }
}