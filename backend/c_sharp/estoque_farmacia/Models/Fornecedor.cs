namespace estoque_farmacia.Models;

/// <summary>
/// Representa um fornecedor de produtos para a farmácia.
/// </summary>
public class Fornecedor
{
    /// <summary>
    /// ID ÚNICO do fornecedor (chave primária).
    /// Auto-incrementado no banco de dados.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// Nome da empresa fornecedora.
    /// </summary>
    public string NomeEmpresa { get; set; }

    /// <summary>
    /// CNPJ da empresa fornecedora.
    /// </summary>
    public string Cnpj { get; set; }

    /// <summary>
    /// Telefone de contato do fornecedor.
    /// </summary>
    public string Telefone { get; set; }

}

