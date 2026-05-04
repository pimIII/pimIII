using System;
namespace estoque_farmacia.Models;

/// <summary>
/// Representa um funcionário do sistema da farmácia.
/// </summary>
public class Funcionario
{
    /// <summary>
    /// Identificador do funcionário.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome completo do funcionário.
    /// </summary>
    public string Nome { get; set; }

    /// <summary>
    /// CPF do funcionário (formato com máscara esperada: 000.000.000-00).
    /// </summary>
    public string CPF { get; set; }

    /// <summary>
    /// Cargo ou função do funcionário.
    /// </summary>
    public string Cargo { get; set; }

    /// <summary>
    /// Hash da senha do usuário (não armazenar em texto claro).
    /// </summary>
    public string SenhaHash { get; set; }

    /// <summary>
    /// Data de admissão no formato DateTime.
    /// </summary>
    public DateTime DataAdmissao { get; set; }

    /// <summary>
    /// Indica se o funcionário está ativo no sistema.
    /// </summary>
    public bool Ativo { get; set; }

    // Sugestões de cargos / permissões:
    // Gerente — acesso total, funcionários, relatórios
    // Farmacêutico — produtos, estoque, receitas
    // Atendente — operações básicas de atendimento

}
