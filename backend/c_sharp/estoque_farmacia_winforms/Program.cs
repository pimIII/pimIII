using estoque_farmacia.Data;
using estoque_farmacia.Services;
using estoque_farmacia_winforms.Forms;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace estoque_farmacia_winforms;

/// <summary>
/// PONTO DE ENTRADA DA APLICACAO WINFORMS
/// =======================================
///
/// Esta classe inicia o programa quando o usuario abre o executavel.
/// Ela monta a injecao de dependencia, configura o EF Core para PostgreSQL
/// e abre a tela de login.
///
/// O fluxo principal e:
///   Login (LoginForm)
///      \-> se autenticado -> abre MenuForm
///          MenuForm -> botoes que abrem cadastros (Produto, Funcionario,
///                       Fornecedor) e a tela de Venda
///
/// O codigo de Models/Services/Data e compartilhado com o projeto Console.
/// Ver os "Link=" no estoque_farmacia_winforms.csproj.
/// </summary>
internal static class Program
{
    /// <summary>
    /// Container de servicos da aplicacao.
    /// Fica acessivel a partir de qualquer Form via Program.Services.
    /// </summary>
    public static IServiceProvider Services { get; private set; } = null!;

    [STAThread]
    static void Main()
    {
        // Inicializacao padrao do WinForms (visual styles, DPI, fonte padrao).
        ApplicationConfiguration.Initialize();

        // Habilita o comportamento antigo do Npgsql para DateTime sem Kind=Utc.
        // Sem isso, gravar DateTime.Now em colunas de data falha porque o driver
        // exige UTC explicito. Este switch e o caminho oficial recomendado quando
        // o projeto nao quer reescrever todos os DateTime.Now como DateTime.UtcNow.
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        // Le a string de conexao do appsettings.json.
        // O arquivo e copiado para a pasta de build via "CopyToOutputDirectory"
        // configurado no csproj.
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Monta o container de injecao de dependencia.
        // Cada vez que pedimos um servico, ele cria a instancia automaticamente
        // e injeta as dependencias (como o AppDbContext).
        var services = new ServiceCollection();

        services.AddDbContext<AppDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection");
            // UseNpgsql = provedor PostgreSQL (atende ao requisito do PIM III).
            options.UseNpgsql(connectionString);
        });

        // Registro dos servicos de negocio.
        services.AddScoped<FuncionarioService>();
        services.AddScoped<ProdutoService>();
        services.AddScoped<FornecedorService>();

        // Os Forms tambem entram no container para receberem injecao automatica.
        services.AddTransient<LoginForm>();
        services.AddTransient<MenuForm>();
        services.AddTransient<ProdutoForm>();
        services.AddTransient<FuncionarioForm>();
        services.AddTransient<FornecedorForm>();
        services.AddTransient<VendaForm>();

        Services = services.BuildServiceProvider();

        // Abre a primeira tela. O Application.Run trava aqui ate o usuario fechar.
        var loginForm = Services.GetRequiredService<LoginForm>();
        Application.Run(loginForm);
    }
}
