using estoque_farmacia.Models;

namespace estoque_farmacia.Services;

public class FornecedorService
{
    private static readonly object Sync = new();
    private static readonly List<Fornecedor> Itens = new();
    private static int _nextId = 1;

    public string UltimoErro { get; private set; } = string.Empty;

    public bool Salvar(Fornecedor novoFornecedor)
    {
        try
        {
            if (novoFornecedor == null || string.IsNullOrWhiteSpace(novoFornecedor.NomeEmpresa))
            {
                UltimoErro = "  ERRO: Nome da empresa e obrigatorio."; Console.WriteLine(UltimoErro);
                return false;
            }

            lock (Sync)
            {
                novoFornecedor.Id = _nextId++;
                Itens.Add(novoFornecedor);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"\n  Fornecedor '{novoFornecedor.NomeEmpresa}' salvo com sucesso! ID: {novoFornecedor.Id}");
            Console.ResetColor();
            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao salvar fornecedor: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }

    public List<Fornecedor> ListarTodos()
    {
        lock (Sync) return Itens.ToList();
    }

    public Fornecedor? BuscarPorId(int id)
    {
        lock (Sync) return Itens.FirstOrDefault(f => f.Id == id);
    }

    public bool Remover(int id)
    {
        try
        {
            lock (Sync)
            {
                var f = Itens.FirstOrDefault(x => x.Id == id);
                if (f == null) return false;
                Itens.Remove(f);
            }

            return true;
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            UltimoErro = $"\n  ERRO ao remover fornecedor: {ex.Message}"; Console.WriteLine(UltimoErro);
            Console.ResetColor();
            return false;
        }
    }
}
