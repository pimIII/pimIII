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
                        for(int i = 0; i < lista.Count; i++)
                        {
                            //@ pra pular linha
                            Console.WriteLine($"ID: {i+1} | Produto: {lista[i].NomeProduto} | Preço venda: {lista[i].PrecoVenda} | Preco Custo: {lista[i].PrecoCusto} | Requer Receita: {lista[i].RequerReceita} ");
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





    public void CadastrarProduto()
    {
        Console.Write("\n");
        Console.Write("Digite o nome do produto: ");
        string nome = Console.ReadLine();

        // Enquanto o nome for nulo, vazio ou só espaços
        while (string.IsNullOrWhiteSpace(nome))
        {Console.Write("\nO nome não pode ser vazio! Digite o nome: ");
        nome = Console.ReadLine();}         


             
        Console.Write("Digite o preço de venda: R$");
        decimal precoVenda;
        while (! decimal.TryParse(Console.ReadLine(), out precoVenda))
        { Console.Write("\nValor inválido! Digite o preço novamente: R$ ");}
        
        
        Console.Write("Digite o preço de custo: R$");
        decimal precoCusto;
        while (! decimal.TryParse(Console.ReadLine(), out precoCusto))
        { Console.Write("\nValor inválido! Digite o preço novamente: R$ ");}
        

        
        Console.Write("Digite o Id do fornecedor: ");
        int idFornecedor;
        while (! int.TryParse(Console.ReadLine(), out idFornecedor))
        {Console.Write("\nSomente números são aceitos: ");}
        
        bool requerReceita = false;
        int requerReceitaInt;
        while(true){
        Console.Write("Requer receita [0 ou 1]: ");
            while (! int.TryParse(Console.ReadLine(), out requerReceitaInt))
            {Console.Write("\nSomente números são aceitos: ");}

        if (requerReceitaInt == 0)
            {requerReceita = false;
            break;}

        else if (requerReceitaInt == 1)
            {requerReceita = true;
            break;}

            else
            {Console.Write("Digite somente 1 ou 2. ");}


        
        
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


        
        Console.WriteLine("\n");
        Console.WriteLine("Produto cadastrado com sucesso!\n");


    }


    public void RemoverProduto()
    {
        Console.Write("Digite o ID do produto a ser removido: ");
        
        if (int.TryParse(Console.ReadLine(), out int idDigitado))
        {
            int indice = idDigitado - 1; //ajusta o indice
            if (_produtoService.Remover(indice) == true) {
            Console.WriteLine("Produto removido com sucesso!");}

            else
            {
                Console.WriteLine("Produto não encontrado.");
            }

        }

        else
        {   Console.Write("\n");
            Console.WriteLine("Digite apenas valores numéricos.");
        }



    }





}