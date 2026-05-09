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
        bool logado = false;
        int tentativas = 0;
        int maxTentativas = 3;

        while (!logado && tentativas < maxTentativas)
        {
            Console.Clear();
            Console.WriteLine("\n=== SISTEMA FARMÁCIA ===");
            Console.Write("\nDigite seu Login: ");
            string login = Console.ReadLine();
            Console.Write("Digite sua senha: ");
            string senha = Console.ReadLine();

            if (login == "admin" && senha == "123")
            {
                Console.WriteLine("\nLogin realizado com sucesso.");
                Thread.Sleep(1500);
                Console.Clear();
                return true;
            }
            else
            {
                tentativas++;
                int tentativasRestantes = maxTentativas - tentativas;

                if (tentativasRestantes > 0)
                {
                    Console.WriteLine($"\nUsuário ou senha incorretos. Tentativas restantes: {tentativasRestantes}");
                    Console.WriteLine("Pressione ENTER para tentar novamente...");
                    Console.ReadLine();
                }
                else
                {
                    Console.WriteLine("\nMáximo de tentativas atingido. Sistema será encerrado.");
                    Thread.Sleep(2000);
                    return false;
                }
            }
        }

        return false;
    }

    /// <summary>
    /// Exibe as opções do menu principal no console.
    /// </summary>
    public void MostrarMenu()
    {
        Console.Clear(); // limpa o console
        Console.WriteLine("=== SISTEMA FARMÁCIA ===");
        Console.WriteLine("1 - Controle de Produto");
        Console.WriteLine("2 - Controle de Funcionário");
        Console.WriteLine("3 - Controle de Venda");
        Console.WriteLine("4 - Controle de Fornecedor");
        Console.WriteLine("7 - Sair");
        Console.Write("\nOPÇÃO >>> ");
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
                Console.WriteLine("Encerrando o programa.");
                break;

                default:
                Console.WriteLine("Opção inválida.");
                Thread.Sleep(1000); // da um sleep no terminal
                break;

            }


        }

    }




}