using estoque_farmacia.Models;

namespace estoque_farmacia.Services;

public class ProdutoService
{
    private static readonly object Sync = new();
    private static readonly List<Produto> Itens = new();
    private static int _nextId = 1;

    private readonly FornecedorService _fornecedorService;

    public string UltimoErro { get; private set; } = string.Empty;

    public ProdutoService(FornecedorService fornecedorService)
    {
        _fornecedorService = fornecedorService;
    }

    public bool Salvar(Produto novoProduto)
    {
        try
        {
            if (novoProduto == null || string.IsNullOrWhiteSpace(novoProduto.NomeProduto))
            {
                UltimoErro = "  ERRO: Nome do produto e obrigatorio."; Console.WriteLine(UltimoErro);
                return false;
            }

            if (!Produto.CodigoBarrasValido(novoProduto.CodigoBarras))
            {
                UltimoErro = "  ERRO: Codigo de barras deve ser um numero inteiro."; Console.WriteLine(UltimoErro);
                return false;
            }

            if (novoProduto.IdFornecedor <= 0)
            {
                UltimoErro = "  ERRO: ID do fornecedor e obrigatorio."; Console.WriteLine(UltimoErro);
                return false;
            }

            if (_fornecedorService.BuscarPorId(novoProduto.IdFornecedor) == null)
            {
                UltimoErro = "  ERRO: Fornecedor nao encontrado."; Console.WriteLine(UltimoErro);
                return false;
            }

            lock (Sync)
            {
                if (Itens.Any(p => p.CodigoBarras == novoProduto.CodigoBarras))
                {
                    UltimoErro = "  ERRO: Ja existe produto com este codigo de barras."; Console.WriteLine(UltimoErro);
                    return false;
                }

                novoProduto.Id = _nextId++;
                Itens.Add(novoProduto);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Produto '{novoProduto.NomeProduto}' salvo com sucesso! ID: {novoProduto.Id}");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao salvar produto: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }

    public List<Produto> ListarTodos()
    {
        lock (Sync) return Itens.ToList();
    }

    public Produto? BuscarPorId(int id)
    {
        lock (Sync) return Itens.FirstOrDefault(p => p.Id == id);
    }

    public bool Remover(int id)
    {
        try
        {
            lock (Sync)
            {
                var p = Itens.FirstOrDefault(x => x.Id == id);
                if (p == null) return false;
                Itens.Remove(p);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao remover produto: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }
}
