namespace estoque_farmacia.Services;
using estoque_farmacia.Models;
using estoque_farmacia.UI;

public class FornecedorService
{
    private List<Fornecedor> listaFornecedores = new List<Fornecedor>();

     public void Salvar(Fornecedor novoFornecedor)
    {
        listaFornecedores.Add(novoFornecedor);
    }

    public List<Fornecedor> ListarTodos()
    {
        return listaFornecedores;
    }

    public bool Remover(int indice)
    {
    //verifica se o indice existe na lista para não dar erro
    if (indice >= 0 && indice < listaFornecedores.Count)
    {
        listaFornecedores.RemoveAt(indice);
        return true;
    }

        else
        {
            return false;
        }



    }




}