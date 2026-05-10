using estoque_farmacia.Models;
using estoque_farmacia.Data;
using System.Collections.Generic;
using System.Linq;

namespace estoque_farmacia.Services;

public class FornecedorService
{
    public string UltimoErro { get; private set; } = string.Empty;

    private readonly AppDbContext context;

    public FornecedorService(AppDbContext context)
    {
        this.context = context;
    }

    public bool Salvar(Fornecedor novoFornecedor)
    {
        try
        {
            if (novoFornecedor == null || string.IsNullOrWhiteSpace(novoFornecedor.NomeEmpresa))
            {
                UltimoErro = "  ERRO: Nome da empresa e obrigatorio."; Console.WriteLine(UltimoErro);
                return false;
            }

            context.Fornecedores.Add(novoFornecedor);
            int registros = context.SaveChanges();

            if (registros > 0)
            {
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n  Fornecedor '{novoFornecedor.NomeEmpresa}' salvo com sucesso! ID: {novoFornecedor.Id}");
                Console.ResetColor();
                return true;
            }

            return false;
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
        return context.Fornecedores.ToList();
    }

    public Fornecedor BuscarPorId(int id)
    {
        return context.Fornecedores.FirstOrDefault(f => f.Id == id);
    }

    public bool Remover(int id)
    {
        try
        {
            var fornecedor = context.Fornecedores.FirstOrDefault(f => f.Id == id);
            if (fornecedor == null) return false;

            context.Fornecedores.Remove(fornecedor);
            context.SaveChanges();
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