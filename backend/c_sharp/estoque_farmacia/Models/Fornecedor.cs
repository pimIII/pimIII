namespace estoque_farmacia.Models;

/// <summary>
/// Representa um fornecedor de produtos para a farmácia.
/// </summary>
public class Fornecedor
{
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

