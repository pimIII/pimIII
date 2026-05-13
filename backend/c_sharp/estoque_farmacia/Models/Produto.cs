using System.Globalization;

namespace estoque_farmacia.Models;

public class Produto
{
    public int Id { get; set; }

    public string NomeProduto { get; set; }

    public string CodigoBarras { get; set; } = string.Empty;

    public decimal PrecoVenda { get; set; }

    public decimal PrecoCusto { get; set; }

    public int IdFornecedor { get; set; }

    public bool RequerReceita { get; set; }

    public static bool CodigoBarrasValido(string? codigo)
    {
        if (string.IsNullOrWhiteSpace(codigo))
            return false;
        return int.TryParse(codigo.Trim(), NumberStyles.Integer, CultureInfo.InvariantCulture, out _);
    }
}
