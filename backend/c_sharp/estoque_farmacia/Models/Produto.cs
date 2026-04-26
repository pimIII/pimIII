namespace estoque_farmacia.Models;

//Produto.cs: Id, Nome, PrecoVenda, IdFornecedor.


public class Produto
{
    public string NomeProduto {get;set;}
    public decimal PrecoVenda {get;set;}
    public decimal PrecoCusto {get;set;}
    public int IdFornecedor {get;set;}
    public bool RequerReceita {get;set;}

}


