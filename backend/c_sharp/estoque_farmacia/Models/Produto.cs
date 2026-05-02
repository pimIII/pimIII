namespace estoque_farmacia.Models;

/// <summary>
/// Representa um produto no estoque da farmácia.
/// </summary>
public class Produto
{
    /// <summary>
    /// Nome do produto.
    /// </summary>
    public string NomeProduto { get; set; }

    /// <summary>
    /// Preço de venda do produto.
    /// </summary>
    public decimal PrecoVenda { get; set; }

    /// <summary>
    /// Preço de custo do produto.
    /// </summary>
    public decimal PrecoCusto { get; set; }

    /// <summary>
    /// Identificador do fornecedor responsável por este produto.
    /// </summary>
    public int IdFornecedor { get; set; }

    /// <summary>
    /// Indica se o produto requer receita (prescrição médica).
    /// </summary>
    public bool RequerReceita { get; set; }

}


