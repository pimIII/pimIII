using estoque_farmacia.UI;
using estoque_farmacia.Services;

var funcionarioService = new FuncionarioService();
var produtoService = new ProdutoService();
var fornecedorService = new FornecedorService();
var loteService = new LoteService(produtoService);

var menu = new Menu(
    new FuncionarioUI(funcionarioService),
    new ProdutoUI(produtoService),
    new FornecedorUI(fornecedorService),
    new LoteUI(loteService));

try
{
    if (menu.ValidarLogin())
        menu.ProcessarMenu();
    else
        Console.WriteLine("Sistema encerrado por não autorização.");
}
catch (Exception ex)
{
    Console.WriteLine($"ERRO ao inicializar aplicação: {ex.Message}");
}
