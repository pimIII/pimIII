using estoque_farmacia.Models;
using estoque_farmacia.Services;
using System;
using System.Collections.Generic;

namespace estoque_farmacia.UI;

/// <summary>
/// CLASSE FUNCIONARIOUI (USER INTERFACE / CAMADA DE APRESENTAÇÃO)
/// ================================================================
///
/// O QUE É UM UI?
/// UI (User Interface) é a camada que INTERAGE COM O USUÁRIO.
/// Ela mostra menus, pede dados e chama os Services para fazer o trabalho.
///
/// ARQUITETURA EM 3 CAMADAS:
///
/// 1. MODEL (Funcionario.cs)
///    ↑ Define: "O QUE SÃO os dados"
///    └ Propriedades: Nome, CPF, Cargo, etc.
///
/// 2. SERVICE (FuncionarioService.cs)
///    ↑ Define: "O QUE FAZER COM os dados"
///    └ Métodos: Salvar, Listar, Atualizar, Deletar
///
/// 3. UI (FuncionarioUI.cs) ← VOCÊ ESTÁ AQUI
///    ↑ Define: "COMO MOSTRAR ao usuário"
///    └ Menus, formulários, validação de entrada
///
/// VANTAGEM:
/// Se mudarmos a interface (console para Web), só mexemos aqui!
/// O Service continua o mesmo.
///
/// FLUXO:
/// Usuário digita algo → UI valida → UI chama Service → Service processa → UI mostra resultado
/// </summary>
public class FuncionarioUI
{
    // REFERÊNCIA AO SERVICE
    // private = só esta classe usa
    // readonly = não pode ser reatribuída depois de inicializar
    /// <summary>
    /// Instância do FuncionarioService.
    /// Esse é o objeto que contém a lógica de negócio (CRUD).
    /// A UI usa este objeto para fazer operações de funcionários.
    ///
    /// PADRÃO USADO: INJEÇÃO DE DEPENDÊNCIA
    /// Em vez de criar o Service DENTRO da UI:
    /// private FuncionarioService service = new FuncionarioService(); // RUIM
    ///
    /// Recebemos pelo CONSTRUTOR:
    /// public FuncionarioUI(FuncionarioService svc) { ... } // BOM
    ///
    /// POR QUÊ?
    /// - Flexibilidade: podemos trocar o Service sem mexer no código
    /// - Testabilidade: facilita escrever testes automatizados
    /// - Reutilizabilidade: um Service pode ser usado em múltiplas UIs
    /// </summary>
    private readonly FuncionarioService _funcionarioService;

    // ========================================================================
    // CONSTRUTOR
    // ========================================================================

    /// <summary>
    /// Construtor da classe FuncionarioUI.
    ///
    /// O QUE É UM CONSTRUTOR?
    /// É um método especial que EXECUTA quando você cria um novo objeto.
    ///
    /// EXEMPLO:
    /// FuncionarioUI ui = new FuncionarioUI(meuService);
    /// ↑ Aqui o construtor é chamado automaticamente
    ///
    /// PARÂMETRO:
    /// - funcionarioService: o Service que será usado por esta UI
    ///
    /// ATRIBUIÇÃO:
    /// this._funcionarioService = funcionarioService;
    /// "this" = refere-se ao OBJETO ATUAL
    /// Assim diferenciamos o parâmetro da variável de classe
    /// </summary>
    public FuncionarioUI(FuncionarioService funcionarioService)
    {
        this._funcionarioService = funcionarioService;
    }

    // ========================================================================
    // MENU PRINCIPAL DE FUNCIONÁRIOS
    // ========================================================================

    /// <summary>
    /// Exibe o menu principal de funcionários.
    ///
    /// Este método mostra as opções disponíveis:
    /// 1. Cadastrar novo funcionário
    /// 2. Listar todos os funcionários
    /// 3. Buscar funcionário por ID
    /// 4. Atualizar funcionário
    /// 5. Inativar funcionário
    /// 6. Voltar ao menu anterior
    ///
    /// O "static void" aqui significa:
    /// - static: não precisa de um objeto para chamar (Menu.MostrarMenu())
    /// - void: não retorna nada, só exibe texto
    /// </summary>
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

    /// <summary>
    /// Loop principal que processa as opções do menu.
    ///
    /// O QUE ESTE MÉTODO FAZ?
    /// 1. Mostra o menu
    /// 2. Pega a opção do usuário
    /// 3. Chama o método correspondente
    /// 4. Volta para o menu (exceto se escolher sair)
    ///
    /// ESTRUTURA: while (loop) + switch (casos)
    /// - while = repete enquanto continuar == true
    /// - switch = escolhe qual método chamar baseado na opção
    /// - break = sai do switch (mas fica no while)
    /// - continuar = false = sai do while
    ///
    /// FLUXO:
    /// Menu → Opção → Ação → Menu novamente (até escolher 6)
    /// </summary>
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
                    Console.WriteLine("\n✗ Opção inválida! Tente novamente.");
                    Thread.Sleep(1500);
                    break;
            }
        }
    }

    // ========================================================================
    // MÉTODO: CADASTRAR NOVO FUNCIONÁRIO
    // ========================================================================

    /// <summary>
    /// Pede os dados ao usuário e cadastra um novo funcionário.
    ///
    /// FLUXO:
    /// 1. Limpa a tela
    /// 2. Pede cada dado ao usuário
    /// 3. Cria um novo objeto Funcionario
    /// 4. Chama o Service para salvar
    /// 5. Mostra mensagem de sucesso/erro
    /// 6. Aguarda o usuário clicar Enter
    ///
    /// PADRÃO: Validação BÁSICA
    /// Aqui validamos se o usuário não deixou em branco.
    /// Validações mais complexas acontecem no Service.
    /// </summary>
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
                Console.WriteLine("\n✗ Nome não pode ser vazio!");
                PauseMenu();
                return; // volta sem fazer nada
            }

            // PEDE O CPF
            Console.Write("CPF (formato: 000.000.000-00): ");
            string cpf = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cpf))
            {
                Console.WriteLine("\n✗ CPF não pode ser vazio!");
                PauseMenu();
                return;
            }

            // PEDE O CARGO
            Console.Write("Cargo (Gerente/Farmacêutico/Atendente/Estoquista): ");
            string cargo = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(cargo))
            {
                Console.WriteLine("\n✗ Cargo não pode ser vazio!");
                PauseMenu();
                return;
            }

            // PEDE A SENHA
            Console.Write("Senha: ");
            string senhaHash = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(senhaHash))
            {
                Console.WriteLine("\n✗ Senha não pode ser vazia!");
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
                SenhaHash = senhaHash
                // Note: Id, DataAdmissao e Ativo são atribuídos pelo Service!
            };

            // CHAMA O SERVICE PARA SALVAR
            bool sucesso = _funcionarioService.Salvar(novoFunc);

            if (sucesso)
            {
                Console.WriteLine("\n✓ Funcionário cadastrado com sucesso!");
            }
            else
            {
                Console.WriteLine("\n✗ Erro ao cadastrar funcionário!");
            }
        }
        catch (Exception ex) // se acontecer um erro
        {
            Console.WriteLine($"\n✗ Erro: {ex.Message}");
        }

        PauseMenu(); // aguarda Enter do usuário
    }

    // ========================================================================
    // MÉTODO: LISTAR TODOS OS FUNCIONÁRIOS
    // ========================================================================

    /// <summary>
    /// Exibe uma lista com TODOS os funcionários cadastrados.
    ///
    /// FLUXO:
    /// 1. Chama o Service para obter a lista
    /// 2. Verifica se tem funcionários
    /// 3. Mostra em formato de tabela
    /// 4. Aguarda o usuário
    ///
    /// CONCEITO: FOREACH
    /// foreach = "para cada" item na lista
    /// var = deixa o C# descobrir o tipo automaticamente
    ///
    /// EXEMPLO:
    /// foreach (var numero in lista) { ... }
    /// // num = 1, depois 2, depois 3, etc.
    /// </summary>
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
            Console.WriteLine($"\n✗ Erro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO: BUSCAR FUNCIONÁRIO POR ID
    // ========================================================================

    /// <summary>
    /// Busca um funcionário específico pelo ID e mostra os detalhes.
    ///
    /// FLUXO:
    /// 1. Pede o ID do usuário
    /// 2. Chama o Service para buscar
    /// 3. Se encontrou, mostra os detalhes
    /// 4. Se não, mostra mensagem de erro
    /// </summary>
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
                Console.WriteLine("\n✗ ID inválido!");
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
                Console.WriteLine($"\n✗ Funcionário com ID {id} não encontrado!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Erro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO: ATUALIZAR FUNCIONÁRIO
    // ========================================================================

    /// <summary>
    /// Atualiza os dados de um funcionário existente.
    ///
    /// FLUXO:
    /// 1. Pede o ID do funcionário a atualizar
    /// 2. Busca o funcionário
    /// 3. Se encontrou, pede os dados novos
    /// 4. Chama o Service para atualizar
    /// 5. Mostra resultado
    /// </summary>
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
                Console.WriteLine("\n✗ ID inválido!");
                PauseMenu();
                return;
            }

            // Busca o funcionário
            Funcionario funcExistente = _funcionarioService.BuscarPorId(id);

            if (funcExistente == null)
            {
                Console.WriteLine($"\n✗ Funcionário com ID {id} não encontrado!");
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
                Console.WriteLine("\n✗ Erro ao atualizar!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Erro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO: INATIVAR FUNCIONÁRIO
    // ========================================================================

    /// <summary>
    /// Inativa um funcionário (marca como não ativo).
    ///
    /// FLUXO:
    /// 1. Pede o ID
    /// 2. Pede confirmação (segurança)
    /// 3. Chama o Service para inativar
    /// 4. Mostra resultado
    ///
    /// IMPORTANTE:
    /// Pedimos CONFIRMAÇÃO antes de deletar.
    /// Isso evita acidentes (usuário clicando errado).
    /// </summary>
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
                Console.WriteLine("\n✗ ID inválido!");
                PauseMenu();
                return;
            }

            // Busca para confirmar
            Funcionario func = _funcionarioService.BuscarPorId(id);
            if (func == null)
            {
                Console.WriteLine($"\n✗ Funcionário com ID {id} não encontrado!");
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
                Console.WriteLine("\n✗ Erro ao inativar!");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"\n✗ Erro: {ex.Message}");
        }

        PauseMenu();
    }

    // ========================================================================
    // MÉTODO AUXILIAR: PAUSAR MENU
    // ========================================================================

    /// <summary>
    /// Aguarda o usuário pressionar Enter antes de continuar.
    ///
    /// FUNÇÃO:
    /// Dar tempo ao usuário de ler as mensagens antes de limpar a tela.
    ///
    /// EXEMPLO:
    /// Console.WriteLine("Operação concluída!");
    /// PauseMenu(); // aguarda Enter
    /// // Agora mostra o próximo menu
    ///
    /// Console.ReadLine() = fica esperando o usuário digitar algo + Enter
    /// </summary>
    private void PauseMenu()
    {
        Console.WriteLine("\nPressione ENTER para continuar...");
        Console.ReadLine();
    }

}