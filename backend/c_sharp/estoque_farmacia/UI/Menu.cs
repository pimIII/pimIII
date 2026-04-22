using estoque_farmacia.Models;
using estoque_farmacia.Services;
namespace estoque_farmacia.UI;

public class Menu{
    
    public bool ValidarLogin(){
        Console.WriteLine("\n=== SISTEMA DE ESTOQUE - FARMÁCIA ===");
        Console.Write("\nDigite seu Login: ");
        string login = Console.ReadLine();
        Console.Write("Digite sua senha: ");
        string senha = Console.ReadLine();

        if (login =="admin" && senha == "123")
        {
            Console.WriteLine("Login realizado com sucesso.");
            Thread.Sleep(1500);
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
        Console.WriteLine("== CONTROLE DE ESTOQUE ===");
        Console.WriteLine("1 - Listar Estoque");
        Console.WriteLine("2 - Cadastrar Produto");
        Console.WriteLine("3 - Procurar Produto");
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
                //ListarEstoque();
                break;

                case "2":
                //CadastrarProduto();
                break;

                case "3":
                //ProcurarProduto();
                break;

                case "4":
                ProcessarMenuFornecedor();
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

    public void MostrarMenuFornecedor() {
        Console.Write("--Controle de fornecedor---\n");
        Console.Write("Escolha uma opção: \n");
        Console.Write("1 - Cadastrar fornecedor\n");
        Console.Write("2 - Listar fornecedores\n");
        Console.Write("3 - Remover fornecedor\n");
        Console.Write(">>> ");
        }

    public void ProcessarMenuFornecedor()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenuFornecedor();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1":   
                break;

                case "2":
                break;

                case "3":
                continuar = false;
                Console.WriteLine("Voltando ao menu principal.");
                break;

                default:
                Console.WriteLine("Opção inválida.");
                Thread.Sleep(1000); // da um sleep no terminal
                break;

            }


        }

    }



}




