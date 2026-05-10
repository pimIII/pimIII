using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace estoque_farmacia.Data;

/// <summary>
/// Fabrica do AppDbContext em tempo de design.
///
/// PARA QUE SERVE:
/// O Entity Framework Core, quando executa comandos de migration
/// (dotnet ef migrations add / database update), precisa instanciar
/// o DbContext fora do fluxo normal da aplicacao. Esta classe ensina
/// ao EF como criar o contexto nessa situacao.
///
/// Sem esta classe, o comando de migration nao consegue ler o
/// appsettings.json e falha por nao saber para qual banco se conectar.
/// </summary>
public class AppDbContextFactory : IDesignTimeDbContextFactory<AppDbContext>
{
    public AppDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<AppDbContext>();

        // String de conexao com PostgreSQL.
        //   Host     - endereco do servidor (localhost = mesma maquina)
        //   Port     - porta padrao do PostgreSQL: 5432
        //   Database - nome do banco criado
        //   Username - usuario administrador padrao do PostgreSQL
        //   Password - senha definida na instalacao
        var connectionString = "Host=localhost;Port=5432;Database=estoque_farmacia;Username=postgres;Password=postgres123";

        // UseNpgsql() registra o provedor PostgreSQL no EF Core,
        // conforme o requisito do manual de definir PostgreSQL como SGBD.
        optionsBuilder.UseNpgsql(connectionString);

        return new AppDbContext(optionsBuilder.Options);
    }
}
