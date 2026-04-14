using estoque_farmacia.UI;
using estoque_farmacia.Models;
using estoque_farmacia.Services;


Menu menuVariavel = new Menu ();

if (menuVariavel.ValidarLogin())
{
    menuVariavel.ProcessarMenu();
    
}

else
{
    Console.WriteLine ("Sistema encerrado por não autorização.");
}