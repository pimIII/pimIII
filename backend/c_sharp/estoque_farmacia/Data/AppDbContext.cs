using estoque_farmacia.Models;
using Microsoft.EntityFrameworkCore;

namespace estoque_farmacia.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Funcionario> Funcionarios { get; set; } = null!;
    public DbSet<Produto> Produtos { get; set; } = null!;
    public DbSet<Fornecedor> Fornecedores { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Funcionario>(entity =>
        {
            entity.HasKey(f => f.Id);
            entity.Property(f => f.Nome).IsRequired().HasMaxLength(150);
            entity.Property(f => f.CPF).IsRequired().HasMaxLength(14);
            entity.Property(f => f.Cargo).IsRequired().HasMaxLength(50);
            entity.Property(f => f.SenhaHash).IsRequired().HasMaxLength(256);
            entity.Property(f => f.DataAdmissao).IsRequired();
            entity.Property(f => f.Ativo).IsRequired().HasDefaultValue(true);
            entity.Property(f => f.DataDemissao).IsRequired(false);
            entity.HasIndex(f => f.CPF).IsUnique().HasName("UX_Funcionario_CPF");
            entity.ToTable("Funcionarios");
        });
    }

    public override int SaveChanges()
    {
        return base.SaveChanges();
    }
}
