using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Locadora.Models
{
    public class Categoria
    {
        public static readonly string INSERTCATEGORIA = @"INSERT INTO dbo.tblCategorias (Nome, Descricao) 
                                                        VALUES (@Nome, @Descricao)
                                                        SELECT SCOPE_IDENTITY();";

        public static readonly string SELECTALLCATEGORIAS = @"SELECT CategoriaId, Nome, Descricao FROM dbo.tblCategorias";

        public static readonly string BUSCARCATEGORIAPORID = @"SELECT CategoriaId, Nome, Descricao 
                                                            FROM dbo.tblCategorias 
                                                            WHERE CategoriaId = @CategoriaId";

        public static readonly string UPDATECATEGORIA = @"UPDATE dbo.tblCategorias 
                                                        SET Nome = @Nome, 
                                                            Descricao = @Descricao 
                                                        WHERE CategoriaId = @CategoriaId";

        public static readonly string DELETECATEGORIA = @"DELETE FROM dbo.tblCategorias 
                                                        WHERE CategoriaId = @CategoriaId";

        public int CategoriaId { get; private set; }
        public string Nome { get; private set; }
        public string? Descricao { get; private set; }
        public Categoria(string nome, string descricao = "")
        {
            Nome = nome;
            Descricao = descricao;
        }
        public void setCategoriaId(int categoriaId)
        {
            CategoriaId = categoriaId;
        }

        public void setNome(string nome)
        {
            Nome = nome;
        }

        public void setDescricao(string descricao)
        {
            Descricao = descricao;
        }

        public override string? ToString()
        {
            return $"CategoriaId: {CategoriaId},\n " +
                   $"NomeCategoria: {Nome},\n" +
                   $"Descrição: {Descricao}\n";
        }
    }
}