using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Domain;
using Domain.Models;
using Domain.Services;

class Program
{
    static async Task Main()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase("testdb")
            .Options;

        using var context = new ApplicationDbContext(options);
        var service = new FuncionarioService(context);

        var f = new Funcionario
        {
            Nome = "João Silva",
            CPF = "12345678909",
            Cargo = "Analista",
            Salario = 3000m,
            DataAdmissao = DateTime.Today.AddYears(-1)
        };

        var created = await service.CreateAsync(f);
        Console.WriteLine($"Criado: {created.Nome} (CPF: {created.CPF})");

        try
        {
            var fInvalid = new Funcionario
            {
                Nome = "Maria",
                CPF = "11111111111",
                Cargo = "Auxiliar",
                Salario = 1200m,
                DataAdmissao = DateTime.Today
            };

            await service.CreateAsync(fInvalid);
            Console.WriteLine("Erro: CPF inválido aceito (deveria falhar)");
        }
        catch (ArgumentException ex)
        {
            Console.WriteLine("CPF inválido corretamente rejeitado: " + ex.Message);
        }
    }
}
