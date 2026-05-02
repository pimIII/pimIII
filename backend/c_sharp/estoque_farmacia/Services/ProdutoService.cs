using estoque_farmacia.Models;
using System.Collections.Generic;

/// <summary>
/// Serviço responsável pelas operações de CRUD em memória para produtos.
/// Este serviço mantém uma lista simples de produtos durante a execução.
/// </summary>
public class ProdutoService
{
    private List<Produto> listaProdutos = new List<Produto>();

    /// <summary>
    /// Salva um novo produto na lista em memória.
    /// </summary>
    /// <param name="novoProduto">Objeto Produto a ser adicionado.</param>
    public void Salvar(Produto novoProduto)
    {
        listaProdutos.Add(novoProduto);

    }

    /// <summary>
    /// Retorna todos os produtos atualmente salvos em memória.
    /// </summary>
    /// <returns>Lista de produtos.</returns>
    public List<Produto> ListarTodos()
    {
        return listaProdutos;
    }

    /// <summary>
    /// Remove um produto pelo índice da lista.
    /// </summary>
    /// <param name="indice">Índice do produto na lista.</param>
    /// <returns>True se removido com sucesso; caso contrário false.</returns>
    public bool Remover(int indice)
    {
        if (indice >= 0 && indice < listaProdutos.Count)
        {
            listaProdutos.RemoveAt(indice); // remove especificamente no índice
            return true;
        }

        else
        {
            return false;
        }

    }



}
