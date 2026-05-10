using estoque_farmacia.Models;
using estoque_farmacia.Services;
namespace estoque_farmacia.UI;

public class ProdutoUI
{
    private readonly ProdutoService _produtoService;

    public ProdutoUI(ProdutoService produtoService)
    {
        _produtoService = produtoService;
    }

    /// <summary>
    /// Exibe o menu de operações relacionadas a produtos.
    /// </summary>



    public void MostrarMenuProduto() {
        Console.WriteLine("\n");
        Console.Write("===CONTROLE DE PRODUTO===\n");
        Thread.Sleep(1500);
        Console.Write("\n");
        Console.Write("Escolha uma opção: \n");
        Console.Write("1 - Cadastrar produto\n");
        Console.Write("2 - Listar produtos\n");
        Console.Write("3 - Remover produtos\n");
        Console.Write("4 - Voltar\n");
        Console.Write(">>> ");
        Console.Write("\n");
        }


    /// <summary>
    /// Loop que processa as opções do menu de produto até o usuário retornar.
    /// </summary>
    public void ProcessarMenuProduto()
    {
        bool continuar = true;
        while (continuar)
        {
            MostrarMenuProduto();
            string opcao = Console.ReadLine();

            switch (opcao)
            {
                case "1": 
                CadastrarProduto();  
                break;

                case "2":
                Console.Clear();
                Console.WriteLine("===LISTA DE PRODUTOS===");
                Console.WriteLine("\n");
                var lista = _produtoService.ListarTodos();
                if (lista.Count == 0)
                    {
                        Console.WriteLine("Nenhum produto cadastrado.");
                        Console.WriteLine("\n");
                        Thread.Sleep(3500);
                    }
                    else
                    {
                        foreach (var p in lista)
                        {
                            string receita = p.RequerReceita ? "Sim" : "Nao";
                            Console.WriteLine($"  ID: {p.Id} | {p.NomeProduto} | Venda: R$ {p.PrecoVenda:F2} | Custo: R$ {p.PrecoCusto:F2} | Receita: {receita}");
                        }
                    }

                break;


                case "3":
                RemoverProduto();
                break;

                case "4":
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





    /// <summary>
    /// Lê os dados do produto no console e registra um novo produto.
    /// </summary>
    public void CadastrarProduto()
    {
        Console.WriteLine();

        Console.Write("Nome do produto: ");
        string nome = Console.ReadLine();
        while (string.IsNullOrWhiteSpace(nome))
        {
            Console.Write("  Nome nao pode ser vazio. Digite novamente: ");
            nome = Console.ReadLine();
        }

        Console.Write("Preco de venda (ex: 12,50): R$ ");
        decimal precoVenda;
        while (!decimal.TryParse(Console.ReadLine(), out precoVenda) || precoVenda < 0 || decimal.Round(precoVenda, 2) != precoVenda)
        {
            Console.Write("  Valor invalido. Use virgula e ate 2 casas decimais (ex: 12,50): R$ ");
        }

        Console.Write("Preco de custo (ex: 8,00): R$ ");
        decimal precoCusto;
        while (!decimal.TryParse(Console.ReadLine(), out precoCusto) || precoCusto < 0 || decimal.Round(precoCusto, 2) != precoCusto)
        {
            Console.Write("  Valor invalido. Use virgula e ate 2 casas decimais (ex: 8,00): R$ ");
        }

        Console.Write("ID do fornecedor: ");
        int idFornecedor;
        while (!int.TryParse(Console.ReadLine(), out idFornecedor) || idFornecedor <= 0)
        {
            Console.Write("  Digite um numero valido: ");
        }

        bool requerReceita;
        while (true)
        {
            Console.Write("Requer receita medica? (S/N): ");
            string resposta = Console.ReadLine()?.Trim().ToUpper();
            if (resposta == "S") { requerReceita = true; break; }
            if (resposta == "N") { requerReceita = false; break; }
            Console.WriteLine("  Digite apenas S ou N.");
        }

        Produto novo = new Produto
        {
            NomeProduto = nome,
            PrecoVenda = precoVenda,
            PrecoCusto = precoCusto,
            IdFornecedor = idFornecedor,
            RequerReceita = requerReceita
        };

        _produtoService.Salvar(novo);

        Console.WriteLine("  Pressione ENTER para continuar...");
        Console.ReadLine();
    }


    /// <summary>
    /// Solicita o ID do produto e tenta removê-lo da lista.
    /// </summary>
    public void RemoverProduto()
    {
        Console.Write("\nID do produto a remover: ");

        if (int.TryParse(Console.ReadLine(), out int id) && id > 0)
        {
            if (_produtoService.Remover(id))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine("  Produto removido com sucesso.");
                Console.ResetColor();
            }
            else
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("  Produto nao encontrado.");
                Console.ResetColor();
            }
        }
        else
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("  ID invalido.");
            Console.ResetColor();
        }

        Console.WriteLine("  Pressione ENTER para continuar...");
        Console.ReadLine();
    }





}