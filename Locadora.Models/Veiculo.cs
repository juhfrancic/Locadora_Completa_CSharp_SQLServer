using Locadora.Models.Enums;

namespace Locadora.Models
{
    public class Veiculo
    {

        public readonly static string INSERTVEICULO = @"INSERT INTO tblVeiculos (CategoriaID, Placa, Marca, Modelo, Ano, StatusVeiculo)
                                                        VALUES (@CategoriaID, @Placa, @Marca, @Modelo, 
                                                                @Ano, @StatusVeiculo)";

        public readonly static string SELECTALLVEICULOS = @"SELECT CategoriaID, 
                                                            Placa, Marca, Modelo, Ano, StatusVeiculo
                                                            FROM tblVeiculos";

        public readonly static string SELECTVEICULOBYPLACA = @"SELECT VeiculoID, CategoriaID, 
                                                            Placa, Marca, Modelo, Ano, StatusVeiculo
                                                            FROM tblVeiculos
                                                            WHERE Placa = @Placa";

        public readonly static string SELECTVEICULOBYID = @"SELECT VeiculoID, CategoriaID, 
                                                            Placa, Marca, Modelo, Ano, StatusVeiculo
                                                            FROM tblVeiculos
                                                            WHERE VeiculoID = @VeiculoID";


        public readonly static string UPDATESTATUSVEICULO = @"UPDATE tblVeiculos 
                                                            SET StatusVeiculo = @StatusVeiculo
                                                            WHERE VeiculoID = @IdVeiculo";

        public readonly static string DELETEVEICULO = @"DELETE FROM tblVeiculos
                                                        WHERE VeiculoID = @IdVeiculo";

        public readonly static string SELECTVEICULOSBYCATEGORIA = @"SELECT VeiculoID, CategoriaID,
                                                    Placa, Marca, Modelo, Ano, StatusVeiculo
                                                    FROM tblVeiculos
                                                    WHERE CategoriaID = @CategoriaID";

        public int VeiculoID { get; private set; }
        public int CategoriaID { get; private set; }
        public string? NomeCategoria { get; private set; }
        public string Placa { get; private set; }
        public string Marca { get; private set; }
        public string Modelo { get; private set; }
        public int Ano { get; private set; }
        public EStatusVeiculo StatusVeiculo { get; private set; }

        public Veiculo(int categoriaID, string placa,
                        string marca, string modelo,
                        int ano, EStatusVeiculo status)
        {
            CategoriaID = categoriaID;
            Placa = placa;
            Marca = marca;
            Modelo = modelo;
            Ano = ano;
            StatusVeiculo = status;
        }

        public void setVeiculoID(int veiculoID)
        {
            VeiculoID = veiculoID;
        }

        public void setNomeCategoria(string nomeCategoria)
        {
            NomeCategoria = nomeCategoria;
        }

        public void setStatusVeiculo(EStatusVeiculo statusVeiculo)
        {
            StatusVeiculo = statusVeiculo;
        }
        public override string? ToString()
        {
            return $"Placa: {Placa}\nMarca: {Marca}\nModelo: {Modelo}\n" +
                $"Ano: {Ano}\nStatus: {StatusVeiculo}\nCategoria: {NomeCategoria}\n";
        }
    }
}