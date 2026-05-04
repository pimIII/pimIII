using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain.Models
{
    public class Funcionario
    {
        [Key]
        public int Id { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string CPF { get; set; } = string.Empty;
        public string Cargo { get; set; } = string.Empty;
        public decimal Salario { get; set; }
        public DateTime DataAdmissao { get; set; }

        public void ValidarDominio()
        {
            if (string.IsNullOrWhiteSpace(Nome)) throw new ArgumentException("Nome é obrigatório.");
            if (string.IsNullOrWhiteSpace(CPF)) throw new ArgumentException("CPF é obrigatório.");
            if (Salario < 0) throw new ArgumentException("Salário não pode ser negativo.");
            if (DataAdmissao == default) throw new ArgumentException("Data de admissão inválida.");
        }
    }
}
