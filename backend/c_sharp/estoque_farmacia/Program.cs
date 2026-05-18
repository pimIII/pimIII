using estoque_farmacia.UI;
using estoque_farmacia.Services;

var funcionarioService = new FuncionarioService();
var fornecedorService = new FornecedorService();
var produtoService = new ProdutoService(fornecedorService);
var loteService = new LoteService(produtoService);
var estoque = new Estoque(loteService);
var vendaService = new VendaService(produtoService, estoque);

var menu = new Menu(
    new FuncionarioUI(funcionarioService),
    new ProdutoUI(produtoService),
    new FornecedorUI(fornecedorService),
    new LoteUI(loteService),
    new VendaUI(produtoService, vendaService, estoque));

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
