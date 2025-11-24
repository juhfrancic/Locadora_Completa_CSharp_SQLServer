using Locadora.Controller;
using Locadora.Controller.Interfaces;
using Locadora.Models;
using Locadora.Models.Enums;
using System.Linq.Expressions;
using System.Reflection.Metadata;
using System.Xml.Linq;
using static Locadora.Controller.Interfaces.IFuncionarioController;
int opcao;



int opcaoCliente;

void MenuCliente()
{
    do
    {
        Console.WriteLine("\n---- MENU CLIENTE ----");
        Console.WriteLine("1 - Cadastrar cliente");
        Console.WriteLine("2 - Listar todos os clientes");
        Console.WriteLine("3 - Buscar cliente por email");
        Console.WriteLine("4 - Atualizar telefone cliente");
        Console.WriteLine("5 - Atualizar documento cliente");
        Console.WriteLine("6 - Deletar cliente");
        Console.WriteLine("0 - Voltar ao menu principal");
        Console.WriteLine("Opção: ");
        opcaoCliente = int.Parse(Console.ReadLine());

        ClienteController clienteController = new ClienteController();
        string email;
        switch (opcaoCliente)
        {
            case 1:
                Console.WriteLine("Qual o nome do cliente?");
                string nome = Console.ReadLine();
                Console.WriteLine("Qual o email do cliente?");
                email = Console.ReadLine();
                var cliente = new Cliente(nome, email);

                Console.WriteLine("Qual o tipo do documento do cliente?");
                string tipoDocumento = Console.ReadLine();
                Console.WriteLine("Qual o número do documento do cliente?");
                string numeroDocumento = Console.ReadLine();
                Console.WriteLine("Qual a data de emissão do documento?");
                DateOnly dataEmissao = DateOnly.Parse(Console.ReadLine());
                Console.WriteLine("Qual a data de validade do documento?");
                DateOnly dataValidade = DateOnly.Parse(Console.ReadLine());
                var documento = new Documento(tipoDocumento, numeroDocumento, dataEmissao, dataValidade);


                try
                {
                    clienteController.AdicionarCliente(cliente, documento);
                    Console.WriteLine("Cliente adicionado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar cliente: " + ex.Message);
                }
                break;

            case 2:
                try
                {
                    Console.WriteLine("---Lista de Clientes---");
                    List<Cliente> clientes = clienteController.ListarTodosClientes();
                    foreach (var c in clientes)
                    {
                        Console.WriteLine(c.ToString());
                        Console.WriteLine("-----------------------\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar clientes: " + ex.Message);
                }
                break;

            case 3:
                Console.WriteLine("Qual o email do cliente que deseja buscar?");
                email = Console.ReadLine();
                try
                {
                    cliente = clienteController.BuscaClientePorEmail(email);
                    Console.WriteLine(cliente);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao buscar cliente: " + ex.Message);
                }
                break;

            case 4:
                Console.WriteLine("Qual o email do cliente que atualizar o telefone?");
                email = Console.ReadLine();
                Console.WriteLine("Qual o novo telefone do cliente?");
                string telefone = Console.ReadLine();
                try
                {
                    clienteController.AtualizarTelefoneCliente(telefone, email);
                    Console.WriteLine("Telefone atualizado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar telefone: " + ex.Message);
                }
                break;

            case 5:
                Console.WriteLine("Qual o email do cliente que deseja atualizar o documento?");
                email = Console.ReadLine();

                Console.WriteLine("Qual o tipo do documento do cliente?");
                tipoDocumento = Console.ReadLine();
                Console.WriteLine("Qual o número do documento do cliente?");
                numeroDocumento = Console.ReadLine();
                Console.WriteLine("Qual a data de emissão do documento?");
                dataEmissao = DateOnly.Parse(Console.ReadLine());
                Console.WriteLine("Qual a data de validade do documento?");
                dataValidade = DateOnly.Parse(Console.ReadLine());
                documento = new Documento(tipoDocumento, numeroDocumento, dataEmissao, dataValidade);

                try
                {
                    clienteController.AtualizarDocumentoCliente(email, documento);
                    Console.WriteLine("Documento atualizado com sucesso!");

                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar documento: " + ex.Message);
                }
                break;
            case 6:
                Console.WriteLine("Qual o email do cliente que deseja deletar?");
                email = Console.ReadLine();

                try
                {
                    clienteController.DeletarClientePorEmail(email);
                    Console.WriteLine("Cliente deletado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao deletar cliente: " + ex.Message);
                }
                break;
        }

    } while (opcaoCliente != 0);
}

void MenuFuncionario()
{
    int opcaoFuncionario;
    do
    {
        FuncionarioController funcionarioController = new FuncionarioController();

        Console.WriteLine("\n---- MENU FUNCIONÁRIO ----");
        Console.WriteLine("1 - Cadastrar funcionário");
        Console.WriteLine("2 - Listar todos os funcionários");
        Console.WriteLine("3 - Atualizar funcionário");
        Console.WriteLine("4 - Deletar funcionário");
        Console.WriteLine("0 - Voltar ao menu principal");
        Console.WriteLine("Opção: ");
        opcaoFuncionario = int.Parse(Console.ReadLine());
        switch (opcaoFuncionario)
        {
            case 1:
                Console.WriteLine("Qual o nome do funcionário?");
                string nome = Console.ReadLine();
                Console.WriteLine("Qual o CPF do funcionário?");
                string cpf = Console.ReadLine();
                Console.WriteLine("Qual o email do funcionário?");
                string email = Console.ReadLine();
                Console.WriteLine("Qual o salário do funcionário?");
                decimal salario = decimal.Parse(Console.ReadLine());
                var funcionario = new Funcionario(nome, cpf, email, salario);


                try
                {
                    funcionarioController.AdicionarFuncionario(funcionario);
                    Console.WriteLine("Funcionário adicionado com sucesso");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar funcionário: " + ex.Message);
                }
                break;
            case 2:
                try
                {
                    Console.WriteLine("---Lista de Funcionários---");
                    List<Funcionario> funcionarios = funcionarioController.ListarTodosFuncionarios();
                    foreach (var f in funcionarios)
                    {
                        Console.WriteLine(f.ToString());
                        Console.WriteLine("-----------------------\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar funcionários: " + ex.Message);
                }
                break;
            case 3:
                Console.WriteLine("Qual email do funcionário que desja atualizar?");
                email = Console.ReadLine();
                Console.WriteLine("Qual o novo nome do funcionário?");
                nome = Console.ReadLine();
                Console.WriteLine("Qual o novo salário do funcionário?");
                salario = decimal.Parse(Console.ReadLine());
                try
                {
                    funcionarioController.AtualizarFuncionario(email, nome, salario);
                    Console.WriteLine("Funcionário atualizado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar funcionário: " + ex.Message);
                }
                break;
            case 4:
                Console.WriteLine("Qual email do funcionário que desja deletar?");
                email = Console.ReadLine();

                try
                {
                    funcionarioController.DeletarFuncionario(email);
                    Console.WriteLine("Funcionário deletado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao deletar funcionário: " + ex.Message);
                }
                break;
        }

    } while (opcaoFuncionario != 0);
}

void MenuLocacao()
{
    int opcaoLocacao;
    do
    {

        LocacaoController locacaoController = new LocacaoController();
        LocacaoFuncionarioController locacaoFuncionarioController = new LocacaoFuncionarioController();

        Console.WriteLine("\n---- MENU LOCAÇÃO ----");
        Console.WriteLine("1 - Registrar nova locação");
        Console.WriteLine("2 - Associar funcionário a locação");
        Console.WriteLine("3 - Cancelar locaçõ");
        Console.WriteLine("4 - Finalizar locação");
        Console.WriteLine("5 - Buscar locações por cliente");
        Console.WriteLine("6 - Listar locações por funcionário");
        Console.WriteLine("7 - Listar funcionários de uma locação");
        Console.WriteLine("8 - Histórico de locações");
        Console.WriteLine("0 - Voltar ao menu principal");
        Console.WriteLine("Opção: ");
        opcaoLocacao = int.Parse(Console.ReadLine());
        switch (opcaoLocacao)
        {
            case 1:
                Console.WriteLine("Qual o ID do cliente que vai realizar a locação?");
                int clienteID = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual o ID do veículo que será locado?");
                int veiculoID = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual o valor da diária?");
                decimal valorDiaria = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Por quantos dias será a locação?");
                int diasLocacao = int.Parse(Console.ReadLine());

                var locacao = new Locacao(clienteID, veiculoID, valorDiaria, diasLocacao);

                List<string> emailsFuncionarios = new List<string>();
                Console.WriteLine("Digite os emails dos funcionários envolvidos (digite 'fim' para encerrar):");
                while (true)
                {
                    string email = Console.ReadLine();
                    if (email.ToLower() == "fim")
                        break;
                    emailsFuncionarios.Add(email);
                }
                try
                {
                    locacaoController.AdicionarLocacao(locacao, emailsFuncionarios);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao registrar nova locação: " + ex.Message);
                }
                break;
            case 2:
                Console.WriteLine("Qual o id do funcionário que deseja associar?");
                int funcionarioID = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual o id da locação que deseja associar?");
                int locacaoID = int.Parse(Console.ReadLine());

                try
                {
                    locacaoFuncionarioController.AdicionarLocacaoFuncionario(locacaoID, funcionarioID);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao associar funcionário à locação:" + ex);
                }
                break;
            case 3:
                Console.WriteLine("Qual o id da locação que deseja cancelar?");
                locacaoID = int.Parse(Console.ReadLine());
                try
                {
                    locacaoController.CancelarLocacao(locacaoID);
                    Console.WriteLine("Locação cancelada com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cancelar locação: " + ex.Message);
                }
                break;
            case 4:
                Console.WriteLine("Qual o id da locação que deseja finalizar?");
                locacaoID = int.Parse(Console.ReadLine());
                try
                {
                    locacaoController.ProcessarDevolucao(locacaoID, DateTime.Now);
                    Console.WriteLine("Locação finalizada cm sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao finalizar locação: " + ex.Message);
                }
                break;
            case 5:
                Console.WriteLine("Qual o id do cliente que deseja buscar as locações?");
                clienteID = int.Parse(Console.ReadLine());
                try
                {
                    Console.WriteLine("---Lista de Locações por Cliente---");
                    List<Locacao> locacoes = locacaoController.BuscarLocacoesPorClienteID(clienteID);
                    foreach (var l in locacoes)
                    {
                        Console.WriteLine(l.ToString());
                        Console.WriteLine("-----------------------\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar locações do cliente: " + ex.Message);
                }
                break;
            case 6:
                Console.WriteLine("Qual email do funcionário que deseja listar suas locações?");
                string emailFuncionario = Console.ReadLine();
                try
                {
                    Console.WriteLine("---Lista de Locações por Funcionário---");
                    List<Locacao> locacoesPorFuncionario = locacaoFuncionarioController.ListarLocacoesPorFuncionario(emailFuncionario);
                    foreach (var l in locacoesPorFuncionario)
                    {
                        Console.WriteLine(l.ToString());
                        Console.WriteLine("-----------------------\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar locações: " + ex.Message);
                }
                break;
            case 7:
                Console.WriteLine("Qual o id da locação que deseja listar os funcionários??");
                int idLocacao = int.Parse(Console.ReadLine());
                try
                {
                    Console.WriteLine("---Lista de Funcionários por Locação---");
                    List<Funcionario> funcionariosPorLocacao = locacaoFuncionarioController.ListarFuncionariosPorLocacao(idLocacao);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar locações: " + ex.Message);
                }
                break;
            case 8:
                try
                {
                    Console.WriteLine("---Lista de Locações---");
                    List<Locacao> locacoes = locacaoController.ListarTodasLocacoes();
                    foreach (var l in locacoes)
                    {
                        Console.WriteLine(l.ToString());
                        Console.WriteLine("-----------------------\n");
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar locações: " + ex.Message);
                }
                break;
        }
    } while (opcaoLocacao != 0);
}

void MenuVeiculo()
{
    int opcaoVeiculo;
    do
    {
        Console.WriteLine("\n---- MENU VEÍCULO E CATEGORIA ----");
        Console.WriteLine("1 - Cadastrar Categoria");
        Console.WriteLine("2 - Atualizar Categoria");
        Console.WriteLine("3 - Deletar Categoria");
        Console.WriteLine("4 - Cadastrar Veículo");
        Console.WriteLine("5 - Consultar Veículos por Categoria");
        Console.WriteLine("6 - Atualizar Status do Veículo");
        Console.WriteLine("0 - Voltar ao Menu Principal");
        Console.WriteLine("Opção: ");
        opcaoVeiculo = int.Parse(Console.ReadLine());
        VeiculoController veiculoController = new VeiculoController();
        CategoriaController categoriaController = new CategoriaController();
        string placa;
        string status;
        switch (opcaoVeiculo)
        {
            case 1:
                Console.WriteLine("Qual o nome do categoria?");
                string nome = Console.ReadLine();
                Console.WriteLine("Gostaria de adicionar uma descrição para a categoria?\nAperte 1 para Sim e qualquer outra tecla para Não");
                int escolha = int.Parse(Console.ReadLine());
                string descricao = null;
                if (escolha == 1)
                {
                    Console.WriteLine("Escreve uma breve descrição da categoria.\n");
                    descricao = Console.ReadLine();
                }
                var categoria = new Categoria(nome, descricao);
                try
                {
                    categoriaController.AdicionarCategoria(categoria);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar categoria: " + ex.Message);
                }
                break;
          
            case 2:
                Console.WriteLine("Qual o id da categoria a ser atualizada?");
                int categoriaIdAtualizada = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual o nome do categoria?");
                string nomeAtualizada = Console.ReadLine();
                Console.WriteLine("Gostaria de adicionar uma descrição para a categoria?\nAperte 1 para Sim e qualquer outra tecla para Não");
                int escolha2 = int.Parse(Console.ReadLine());
                string descricaoAtualizada = null;
                if (escolha2 == 1)
                {
                    Console.WriteLine("Escreve uma breve descrição da categoria.\n");
                    descricaoAtualizada = Console.ReadLine();
                }
                try
                {
                    categoriaController.UpdateCategoria(categoriaIdAtualizada, nomeAtualizada, descricaoAtualizada);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar categoria: " + ex.Message);
                }
                break;
            case 3:
                Console.WriteLine("Qual o id da categoria a ser deletada?");
                int categoriaIdDeletado = int.Parse(Console.ReadLine());
                try
                {
                    categoriaController.DeleteCategoria(categoriaIdDeletado);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao deletar categoria: " + ex.Message);
                }
                break;
            case 4:
                Console.WriteLine("Qual o id da categoria do veículo?");
                int categoriaId = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual é a placa do veículo?");
                placa = Console.ReadLine();
                Console.WriteLine("Qual é a marca do veículo?");
                string marca = Console.ReadLine();
                Console.WriteLine("Qual é o modelo do veículo?");
                string modelo = Console.ReadLine();
                Console.WriteLine("Qual é o ano do veículo?");
                int ano = int.Parse(Console.ReadLine());
                Console.WriteLine("Qual é o status do veículo");
                EStatusVeiculo eStatus = (EStatusVeiculo)Enum.Parse(typeof(EStatusVeiculo), Console.ReadLine());
                try
                {
                    var veiculo = new Veiculo(categoriaId, placa, marca, modelo, ano, eStatus);
                    veiculoController.AdicionarVeiculo(veiculo);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao cadastrar veículo: " + ex.Message);
                }
                break;
            case 5:
                Console.WriteLine("Qual é o id categoria a ser consultada?");
                int categoriaIdBuscar = int.Parse(Console.ReadLine());
                try
                {
                    var veiculosPorCategoria = veiculoController.BuscarVeiculosPorCategoria(categoriaIdBuscar);
                    foreach (var veiculo in veiculosPorCategoria)
                    {
                        Console.WriteLine(veiculo);
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao listar veículos por categoria: " + ex.Message);
                }
                break;
            case 6:
                Console.WriteLine("Qual a placa do veículo a atualizar?");
                placa = Console.ReadLine();
                Console.WriteLine("Quer atualizar para:\n1 - Manutenção\n2 - Reservado");
                status = Console.ReadLine();
                if(status == "1")
                    status = EStatusVeiculo.Manutencao.ToString();
                else if (status == "2")
                    status = EStatusVeiculo.Reservado.ToString();
                try
                {
                    veiculoController.AtualizarStatusVeiculo(status, placa);
                    Console.WriteLine("Veículo atualizado com sucesso!");
                }
                catch (Exception ex)
                {
                    Console.WriteLine("Erro ao atualizar status do veículo: " + ex.Message);
                }
                break;
        }
    } while (opcaoVeiculo != 0);
}


do
{
    Console.WriteLine("\n---- MENU PRINCIPAL ----");
    Console.WriteLine("Para qual área gostaria de ir?");
    Console.WriteLine("1 - Clientes");
    Console.WriteLine("2 - Funcionários");
    Console.WriteLine("3 - Locações");
    Console.WriteLine("4 - Veículos e Categoria");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");
    opcao = int.Parse(Console.ReadLine());

    switch (opcao)
    {
        case 1:
            MenuCliente();
            break;
        case 2:
            MenuFuncionario();
            break;
        case 3:
            MenuLocacao();
            break;
        case 4:
            MenuVeiculo();
            break;

    }

} while (opcao != 0);



