using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Domain
{
    public class FuncionarioConfiguration : IEntityTypeConfiguration<Funcionario>
    {
        public void Configure(EntityTypeBuilder<Funcionario> builder)
        {
            builder.ToTable("funcionarios");
            builder.HasKey(f => f.Id);
            builder.Property(f => f.Nome).HasMaxLength(150).IsRequired();
            builder.Property(f => f.CPF).HasMaxLength(14).IsRequired();
            builder.HasIndex(f => f.CPF).IsUnique().HasDatabaseName("ix_funcionarios_cpf");
            builder.Property(f => f.Cargo).HasMaxLength(100);
            builder.Property(f => f.Salario).HasColumnType("numeric(12,2)");
            builder.Property(f => f.DataAdmissao).HasColumnType("date").IsRequired();
        }
    }
}
