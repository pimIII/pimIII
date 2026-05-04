using estoque_farmacia.Models;
using System.Collections.Generic;
using estoque_farmacia.UI;

namespace estoque_farmacia.Services;

/// <summary>
/// Serviço responsável pelas operações em memória sobre fornecedores.
/// </summary>
public class FornecedorService
{
    private List<Fornecedor> listaFornecedores = new List<Fornecedor>();

    /// <summary>
    /// Adiciona um novo fornecedor à lista em memória.
    /// </summary>
    /// <param name="novoFornecedor">Fornecedor a ser adicionado.</param>
    public void Salvar(Fornecedor novoFornecedor)
    {
        listaFornecedores.Add(novoFornecedor);
    }

    /// <summary>
    /// Retorna todos os fornecedores cadastrados em memória.
    /// </summary>
    /// <returns>Lista de fornecedores.</returns>
    public List<Fornecedor> ListarTodos()
    {
        return listaFornecedores;
    }

    /// <summary>
    /// Remove um fornecedor pelo índice da lista.
    /// </summary>
    /// <param name="indice">Índice do fornecedor na lista.</param>
    /// <returns>True se removido; caso contrário false.</returns>
    public bool Remover(int indice)
    {
        // verifica se o indice existe na lista para não dar erro
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