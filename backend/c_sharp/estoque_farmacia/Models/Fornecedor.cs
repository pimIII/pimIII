//Fornecedor.cs: Id, NomeEmpresa, CNPJ, Telefone.
namespace estoque_farmacia.Models;


public class Fornecedor
{
    public string nomeEmpresa;
    public string cnpj;
    public string telefone;

    public void CadastrarFornecedor()
    {   
        Console.Write("Digite o nome da Empresa: ");
        nomeEmpresa = Console.ReadLine();
        Console.Write("Digite o CNPJ: ");
        cnpj = Console.ReadLine();
        Console.Write("Digite o Telefone da empresa: ");
        telefone = Console.ReadLine();
        Console.WriteLine("Cadastro concluído com sucesso.");

    }


}