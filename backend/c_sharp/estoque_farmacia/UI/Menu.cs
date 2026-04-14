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
        Console.WriteLine("4 - Sair");
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




