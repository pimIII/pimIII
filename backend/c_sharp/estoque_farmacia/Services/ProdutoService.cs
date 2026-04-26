using estoque_farmacia.Models;


//id_produto INT
// nome VARCHAR(100)
// FK id_fornecedor INT
// preco_custo DECIMAL(10,2)
// preco_venda DECIMAL(10,2)
// requer_receita 0 ou 1



public class ProdutoService
{
    private List<Produto> listaProdutos = new List<Produto> ();


    public void Salvar(Produto novoProduto)
    {
        listaProdutos.Add(novoProduto);

    }

    public List<Produto> ListarTodos()
    {
        return listaProdutos;
    }


    public bool Remover(int indice)
    {
        if (indice >= 0 && indice < listaProdutos.Count)
        {
            listaProdutos.RemoveAt(indice); //removeat = remove especificamente no indice
            return true;
        }

        else
        {
            return false;
        }

    }




}