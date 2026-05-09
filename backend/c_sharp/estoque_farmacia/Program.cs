using estoque_farmacia.UI;
using estoque_farmacia.Models;
using estoque_farmacia.Services;
using estoque_farmacia.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Configuration;

// Carregar configurações
var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

// Configurar injeção de dependências
var services = new ServiceCollection();

// Registrar DbContext
services.AddDbContext<AppDbContext>(options =>
{
    var connectionString = configuration.GetConnectionString("DefaultConnection");
    options.UseSqlServer(connectionString);
});

// Registrar Services
services.AddScoped<FuncionarioService>();
services.AddScoped<ProdutoService>();
services.AddScoped<FornecedorService>();

// Registrar UIs
services.AddScoped<FuncionarioUI>();
services.AddScoped<ProdutoUI>();
services.AddScoped<FornecedorUI>();

// Registrar Menu
services.AddScoped<Menu>();

var serviceProvider = services.BuildServiceProvider();

// Iniciar aplicação
try
{
    var menuVariavel = serviceProvider.GetRequiredService<Menu>();

    if (menuVariavel.ValidarLogin())
    {
        menuVariavel.ProcessarMenu();
    }
    else
    {
        Console.WriteLine("Sistema encerrado por não autorização.");
    }
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO ao inicializar aplicação: {ex.Message}");
    Console.WriteLine("\nVerifique se:");
    Console.WriteLine("1. SQL Server está rodando");
    Console.WriteLine("2. Connection string está correta");
    Console.WriteLine("3. Database foi criado com migrations");
}