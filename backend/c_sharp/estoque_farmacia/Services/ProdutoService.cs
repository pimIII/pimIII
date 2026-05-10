using estoque_farmacia.Models;
using estoque_farmacia.Data;
using System.Collections.Generic;
using System.Linq;

namespace estoque_farmacia.Services;

public class ProdutoService
{
    public string UltimoErro { get; private set; } = string.Empty;

    private readonly AppDbContext context;

    public ProdutoService(AppDbContext context)
    {
        this.context = context;
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

            context.Produtos.Add(novoProduto);
            int registros = context.SaveChanges();

            if (registros > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  Produto '{novoProduto.NomeProduto}' salvo com sucesso! ID: {novoProduto.Id}");
                Console.ResetColor();
                return true;
            }

            return false;
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
        return context.Produtos.ToList();
    }

    public Produto BuscarPorId(int id)
    {
        return context.Produtos.FirstOrDefault(p => p.Id == id);
    }

    public bool Remover(int id)
    {
        try
        {
            var produto = context.Produtos.FirstOrDefault(p => p.Id == id);
            if (produto == null) return false;

            context.Produtos.Remove(produto);
            context.SaveChanges();
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
