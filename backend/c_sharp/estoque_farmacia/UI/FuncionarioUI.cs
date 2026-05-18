using estoque_farmacia.Models;
using estoque_farmacia.Services;
using System;
using System.Collections.Generic;

namespace estoque_farmacia.UI;

public class FuncionarioUI
{
    // REFERÊNCIA AO SERVICE
    // private = só esta classe usa
    // readonly = não pode ser reatribuída depois de inicializar
    private readonly FuncionarioService _funcionarioService;

    // ========================================================================
    // CONSTRUTOR
    // ========================================================================

    public FuncionarioUI(FuncionarioService funcionarioService)
    {
        this._funcionarioService = funcionarioService;
    }

    // ========================================================================
    // MENU PRINCIPAL DE FUNCIONÁRIOS
    // ========================================================================

    public static void MostrarMenu()
    {
        Console.Clear();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n  CONTROLE DE FUNCIONARIOS");
        Console.WriteLine("  ================================\n");
        Console.ResetColor();

        Console.WriteLine("  [1] Cadastrar novo funcionario");
        Console.WriteLine("  [2] Listar todos os funcionarios");
        Console.WriteLine("  [3] Buscar funcionario por ID");
        Console.WriteLine("  [4] Atualizar funcionario");
        Console.WriteLine("  [5] Inativar funcionario");
        Console.ForegroundColor = ConsoleColor.DarkGray;
        Console.WriteLine("  [6] Voltar");
        Console.ResetColor();

        Console.Write("\n  Opcao: ");
    }

    // ========================================================================
    // MÉTODO: PROCESSAR MENU
    // ========================================================================

    public void ProcessarMenuFuncionario()
    {
        // VARIÁVEL DE CONTROLE
        // bool = pode ser true ou false
        // continuar = true = fica mostrando o menu
        // continuar = false = sai do menu
        bool continuar = true;

        // LOOP INFINITO (enquanto continuar for true)
        while (continuar)
        {
            // Mostra o menu
            MostrarMenu();

            // Pega a opção do usuário
            string opcao = Console.ReadLine();

            // SWITCH = escolhe qual ação executar baseado na opção
            // Como se fosse múltiplos "if/else"
            switch (opcao)
            {
                // OPÇÃO 1: CADASTRAR
                case "1":
                    CadastrarFuncionario();
                    break; // sai do switch, volta ao menu

                // OPÇÃO 2: LISTAR TODOS
                case "2":
                    ListarTodosFuncionarios();
                    break;

                // OPÇÃO 3: BUSCAR POR ID
                case "3":
                    BuscarFuncionarioPorId();
                    break;

                // OPÇÃO 4: ATUALIZAR
                case "4":
                    AtualizarFuncionario();
                    break;

                // OPÇÃO 5: INATIVAR
                case "5":
                    InativarFuncionario();
                    break;

                // OPÇÃO 6: VOLTAR
                case "6":
                    continuar = false; // sai do loop
                    Console.WriteLine("\nVoltando ao menu principal...");
                    Thread.Sleep(1000); // aguarda 1 segundo
                    break;

                // OPÇÃO INVÁLIDA
                default:
                    Console.WriteLine("\nOpção inválida! Tente novamente.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

    // ========================================================================
    // MÉTODO: CADASTRAR NOVO FUNCIONÁRIO
    // ========================================================================

    private void CadastrarFuncionario()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║    CADASTRAR NOVO FUNCIONÁRIO        ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        try // try/catch = captura erros para não derrubar o programa
        {
            // PEDE O NOME
            Console.Write("Nome completo: ");
            string nome = Console.ReadLine();

            // VALIDAÇÃO SIMPLES
            if (string.IsNullOrWhiteSpace(nome))
            {
                Console.WriteLine("\nNome não pode ser vazio!");
                PauseMenu();
                return; // volta sem fazer nada
            }

            // PEDE O CPF
            Console.Write("CPF (formato: 000.000.000-00): ");
            string cpf = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cpf))
            {
                Console.WriteLine("\nCPF não pode ser vazio!");
                PauseMenu();
                return;
            }

            // PEDE O CARGO
            Console.Write("Cargo (Gerente/Farmacêutico/Atendente/Estoquista): ");
            string cargo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cargo))
            {
                Console.WriteLine("\nCargo não pode ser vazio!");
                PauseMenu();
                return;
            }

            // PEDE A SENHA
            Console.Write("Senha: ");
            string senha = Console.ReadLine() ?? "";

            if (string.IsNullOrWhiteSpace(senha))
            {
                Console.WriteLine("\nSenha não pode ser vazia!");
                PauseMenu();
                return;
            }

            // CRIA UM NOVO OBJETO FUNCIONARIO
            // new = cria uma nova instância (cópia) da classe
            Funcionario novoFunc = new Funcionario
            {
                // Inicialização de propriedades
                // Nome = valor que o usuário digitou
                Nome = nome,
                CPF = cpf,
                Cargo = cargo,
                SenhaHash = SenhaHasher.HashSha256(senha)
                // Note: Id, DataAdmissao e Ativo são atribuídos pelo Service!
            };

            // CHAMA O SERVICE PARA SALVAR
            bool sucesso = _funcionarioService.Salvar(novoFunc);

            if (sucesso)
            {
                Console.WriteLine("\nFuncionário cadastrado com sucesso!");
            }
            else
            {
                Console.WriteLine("\nErro ao cadastrar funcionário!");
            }
        }
        catch (Exception ex) // se acontecer um erro
        {
            Console.WriteLine($"\nErro: {ex.Message}");
        }

        PauseMenu(); // aguarda Enter do usuário
    }

    // ========================================================================
    // MÉTODO: LISTAR TODOS OS FUNCIONÁRIOS
    // ========================================================================

    private void ListarTodosFuncionarios()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║     LISTA DE FUNCIONÁRIOS           ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        try
        {
            // Chama o Service para obter a lista
            List<Funcionario> funcionarios = _funcionarioService.ListarTodos();

            // VERIFICA SE TEM FUNCIONÁRIOS
            if (funcionarios.Count == 0)
            {
                Console.WriteLine("Nenhum funcionário cadastrado.");
            }
            else
            {
                // CABEÇALHO DA TABELA
                Console.WriteLine($"{"ID",-5} {"Nome",-25} {"CPF",-15} {"Cargo",-15} {"Status",-10}");
                Console.WriteLine(new string('-', 70));

                // FOREACH = para cada funcionário na lista
                foreach (var func in funcionarios)
                {
                    // OPERADOR TERNÁRIO: (condição) ? se_verdadeiro : se_falso
                    // Se ativo, mostra "Ativo", senão "Inativo"
                    string status = func.Ativo ? "Ativo" : "Inativo";

                    // Mostra a linha da tabela
                    Console.WriteLine($"{func.Id,-5} {func.Nome,-25} {func.CPF,-15} {func.Cargo,-15} {status,-10}");
                }

                // ESTATÍSTICAS
                Console.WriteLine(new string('-', 70));
                int total = _funcionarioService.Contar();
                int ativos = _funcionarioService.ContarAtivos();
                Console.WriteLine($"\nTotal: {total} | Ativos: {ativos} | Inativos: {total - ativos}");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO: BUSCAR FUNCIONÁRIO POR ID
    // ========================================================================

    private void BuscarFuncionarioPorId()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║   BUSCAR FUNCIONÁRIO POR ID          ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        try
        {
            // Pede o ID
            Console.Write("Digite o ID do funcionário: ");

            // int.TryParse = tenta converter texto para número
            // Retorna true se conseguir, false se não conseguir
            // Exemplo: "5" → 5 (sucesso), "abc" → erro
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido!");
                PauseMenu();
                return;
            }

            // Chama o Service
            Funcionario func = _funcionarioService.BuscarPorId(id);

            if (func != null) // se encontrou
            {
                Console.WriteLine("\n╔══════════════════════════════════════╗");
                Console.WriteLine("║      DETALHES DO FUNCIONÁRIO         ║");
                Console.WriteLine("╚══════════════════════════════════════╝\n");
                Console.WriteLine($"ID: {func.Id}");
                Console.WriteLine($"Nome: {func.Nome}");
                Console.WriteLine($"CPF: {func.CPF}");
                Console.WriteLine($"Cargo: {func.Cargo}");
                Console.WriteLine($"Admissão: {func.DataAdmissao:dd/MM/yyyy}");
                Console.WriteLine($"Status: {(func.Ativo ? "Ativo" : "Inativo")}");

                if (func.DataDemissao.HasValue) // HasValue = se não é nulo
                {
                    Console.WriteLine($"Demissão: {func.DataDemissao?.ToString("dd/MM/yyyy")}");
                }
            }
            else
            {
                Console.WriteLine($"\nFuncionário com ID {id} não encontrado!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO: ATUALIZAR FUNCIONÁRIO
    // ========================================================================

    private void AtualizarFuncionario()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║    ATUALIZAR FUNCIONÁRIO             ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        try
        {
            // Pede o ID
            Console.Write("Digite o ID do funcionário a atualizar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido!");
                PauseMenu();
                return;
            }

            // Busca o funcionário
            Funcionario funcExistente = _funcionarioService.BuscarPorId(id);

            if (funcExistente == null)
            {
                Console.WriteLine($"\nFuncionário com ID {id} não encontrado!");
                PauseMenu();
                return;
            }

            // Mostra os dados atuais
            Console.WriteLine($"\nDados atuais:");
            Console.WriteLine($"Nome: {funcExistente.Nome}");
            Console.WriteLine($"CPF: {funcExistente.CPF}");
            Console.WriteLine($"Cargo: {funcExistente.Cargo}");

            // Pede os dados novos
            Console.WriteLine("\nDigite os novos dados (deixe em branco para manter):");

            Console.Write("Novo nome: ");
            string novoNome = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoNome))
                funcExistente.Nome = novoNome;

            Console.Write("Novo CPF: ");
            string novoCpf = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoCpf))
                funcExistente.CPF = novoCpf;

            Console.Write("Novo cargo: ");
            string novoCargo = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(novoCargo))
                funcExistente.Cargo = novoCargo;

            // Chama o Service
            bool sucesso = _funcionarioService.Atualizar(funcExistente);

            if (!sucesso)
            {
                Console.WriteLine("\nErro ao atualizar!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO: INATIVAR FUNCIONÁRIO
    // ========================================================================

    private void InativarFuncionario()
    {
        Console.Clear();
        Console.WriteLine("\n╔══════════════════════════════════════╗");
        Console.WriteLine("║    INATIVAR FUNCIONÁRIO              ║");
        Console.WriteLine("╚══════════════════════════════════════╝\n");

        try
        {
            Console.Write("Digite o ID do funcionário a inativar: ");
            if (!int.TryParse(Console.ReadLine(), out int id))
            {
                Console.WriteLine("\nID inválido!");
                PauseMenu();
                return;
            }

            // Busca para confirmar
            Funcionario func = _funcionarioService.BuscarPorId(id);
            if (func == null)
            {
                Console.WriteLine($"\nFuncionário com ID {id} não encontrado!");
                PauseMenu();
                return;
            }

            // PEDE CONFIRMAÇÃO (segurança)
            Console.WriteLine($"\nDeseja realmente inativar '{func.Nome}'?");
            Console.Write("Confirme digitando 'SIM': ");
            string confirmacao = Console.ReadLine();

            if (confirmacao.ToUpper() != "SIM")
            {
                Console.WriteLine("\nOperação cancelada.");
                PauseMenu();
                return;
            }

            // Chama o Service
            bool sucesso = _funcionarioService.Inativar(id);

            if (!sucesso)
            {
                Console.WriteLine("\nErro ao inativar!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\nErro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO AUXILIAR: PAUSAR MENU
    // ========================================================================

    private void PauseMenu()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

}
