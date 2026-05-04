using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    /// <summary>
    /// Representa um funcionário da empresa.
    /// Comentários detalhados foram adicionados para auxiliar no relatório do PIM.
    /// </summary>
    [Table("funcionarios")]
    public class Funcionario
    {
        [Key]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        [Column("nome")]
        public string Nome { get; set; }

        [Required]
        [MaxLength(14)]
        [Column("cpf")]
        public string CPF { get; set; }

        [MaxLength(100)]
        [Column("cargo")]
        public string Cargo { get; set; }

        [Column("salario", TypeName = "numeric(12,2)")]
        public decimal Salario { get; set; }

        [Required]
        [Column("data_admissao", TypeName = "date")]
        public DateTime DataAdmissao { get; set; }

        public void ValidarDominio()
        {
            if (string.IsNullOrWhiteSpace(Nome))
                throw new ArgumentException("Nome é obrigatório.");

            if (string.IsNullOrWhiteSpace(CPF))
                throw new ArgumentException("CPF é obrigatório.");

            if (Salario < 0)
                throw new ArgumentException("Salário não pode ser negativo.");

            if (DataAdmissao == default)
                throw new ArgumentException("Data de admissão inválida.");
        }

        public override string ToString()
        {
            return $"[{Id}] {Nome} - {Cargo} - CPF: {CPF} - Salário: {Salario:C} - Admissão: {DataAdmissao:yyyy-MM-dd}";
        }
    }
}
