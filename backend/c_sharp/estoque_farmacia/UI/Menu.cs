using estoque_farmacia.Models;
using estoque_farmacia.Services;
namespace estoque_farmacia.UI;

public class Menu
{
    private readonly FornecedorUI _fornecedorUI;
    private readonly ProdutoUI _produtoUI;
    private readonly FuncionarioUI _funcionarioUI;

    public Menu(FuncionarioUI funcionarioUI, ProdutoUI produtoUI, FornecedorUI fornecedorUI)
    {
        _funcionarioUI = funcionarioUI;
        _produtoUI = produtoUI;
        _fornecedorUI = fornecedorUI;
    }

    
    public bool ValidarLogin()
    {
        while (true)
        {
            Console.Clear();

            // Header
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine("\n╔════════════════════════════════════╗");
            Console.WriteLine("║     💊 SISTEMA DE FARMÁCIA 💊      ║");
            Console.WriteLine("╚════════════════════════════════════╝\n");

            // Login
            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("👤 Login: ");
            Console.ResetColor();
            string login = Console.ReadLine();

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.Write("🔐 Senha: ");
            Console.ResetColor();
            string senha = Console.ReadLine();

            if (login == "admin" && senha == "123")
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("\n✓ Login realizado com sucesso!");
                Console.ResetColor();
                Thread.Sleep(1500);
                Console.Clear();
                return true;
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n✗ Usuário ou senha incorretos.");
                Console.ResetColor();
                Console.WriteLine("Pressione ENTER para tentar novamente...");
                Console.ReadLine();
            }
        }
    }

    /// <summary>
    /// Exibe as opções do menu principal no console.
    /// </summary>
    public void MostrarMenu()
    {
        Console.Clear();

        // Header
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔════════════════════════════════════╗");
        Console.WriteLine("║       MENU PRINCIPAL FARMÁCIA       ║");
        Console.WriteLine("╚════════════════════════════════════╝\n");

        // Menu items
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("  1 📦 Controle de Produto");
        Console.ForegroundColor = ConsoleColor.Blue;
        Console.WriteLine("  2 👥 Controle de Funcionário");
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("  3 💳 Controle de Venda");
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("  4 🏢 Controle de Fornecedor");
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("  7 ❌ Sair");

        Console.ForegroundColor = ConsoleColor.White;
        Console.Write("\n➜ Escolha uma opção: ");
        Console.ResetColor();
    }

    /// <summary>
    /// Loop principal que processa a opção selecionada pelo usuário até sair.
    /// </summary>
    public void ProcessarMenu()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenu();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":
                _produtoUI.ProcessarMenuProduto();
                break;

                case "2":
                _funcionarioUI.ProcessarMenuFuncionario();
                break;

                case "3":
                //ControledeVenda;
                break;

                case "4":
                _fornecedorUI.ProcessarMenuFornecedor();
                break;

                case "7":
                continuar = false;
                MostrarMensagemEncerramento();
                break;

                default:
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("\n✗ Opção inválida. Pressione ENTER para continuar...");
                Console.ResetColor();
                Console.ReadLine();
                break;

            }
        }
    }

    public void MostrarMensagemEncerramento()
    {
        Console.Clear();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n╔════════════════════════════════════╗");
        Console.WriteLine("║     Obrigado por usar o Sistema     ║");
        Console.WriteLine("║          Até logo! 👋             ║");
        Console.WriteLine("╚════════════════════════════════════╝\n");
        Console.ResetColor();
    }




}