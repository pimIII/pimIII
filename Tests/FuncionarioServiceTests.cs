using System;
using System.Threading.Tasks;
using Domain;
using Domain.Models;
using Domain.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace Tests
{
    public class FuncionarioServiceTests
    {
        private ApplicationDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            return new ApplicationDbContext(options);
        }

        [Fact]
        public async Task CreateAsync_Should_Add_Funcionario()
        {
            using var context = CreateInMemoryContext();
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
            var fromDb = await context.Funcionarios.FirstOrDefaultAsync(x => x.CPF == f.CPF);
            Assert.NotNull(fromDb);
            Assert.Equal("João Silva", fromDb.Nome);
        }

        [Fact]
        public async Task CreateAsync_InvalidCpf_Should_Throw()
        {
            using var context = CreateInMemoryContext();
            var service = new FuncionarioService(context);

            var f = new Funcionario
            {
                Nome = "Maria",
                CPF = "11111111111",
                Cargo = "Auxiliar",
                Salario = 1200m,
                DataAdmissao = DateTime.Today
            };

            await Assert.ThrowsAsync<ArgumentException>(() => service.CreateAsync(f));
        }
    }
}
