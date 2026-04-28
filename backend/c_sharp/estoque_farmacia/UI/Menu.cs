using estoque_farmacia.Models;
using estoque_farmacia.Services;
namespace estoque_farmacia.UI;


public class Menu{

    private readonly FornecedorService _fornecedorService = new FornecedorService();
    private readonly ProdutoService _produtoService = new ProdutoService();
    private readonly FornecedorUI _fornecedorUI;
    private readonly ProdutoUI _produtoUI;

      public Menu()
    {
        _fornecedorUI = new FornecedorUI(_fornecedorService);
        _produtoUI = new ProdutoUI(_produtoService);
    }

    
    public bool ValidarLogin(){
        Console.WriteLine("\n=== SISTEMA FARMÁCIA ===");
        Console.Write("\nDigite seu Login: ");
        string login = Console.ReadLine();
        Console.Write("Digite sua senha: ");
        string senha = Console.ReadLine();

        if (login =="admin" && senha == "123")
        {
            Console.WriteLine("\n");
            Console.WriteLine("Login realizado com sucesso.");
            Thread.Sleep(1500);
            Console.Clear();
            return true; // retorna true se o login bater
        }

        else
        {
        Console.WriteLine("Usuário ou senha incorretos.");
        return false;
        }

    }

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
                //ControledeFuncionario;
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