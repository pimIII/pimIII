using System.Drawing;
using System.Windows.Forms;
using Microsoft.Extensions.DependencyInjection;

namespace estoque_farmacia_winforms.Forms;

/// <summary>
/// MENU PRINCIPAL
/// ==============
///
/// Tela inicial apos o login. Apresenta botoes que abrem cada modulo
/// do sistema (cadastros e vendas). Cada botao instancia o Form
/// correspondente via injecao de dependencia.
/// </summary>
public class MenuForm : Form
{
    public MenuForm()
    {
        ConfigurarJanela();
        ConstruirTela();
    }

    private void ConfigurarJanela()
    {
        Text = "Menu Principal - FarmaSystem";
        Size = new Size(780, 520);
        StartPosition = FormStartPosition.CenterScreen;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        // Cabecalho azul.
        var topo = new Panel
        {
            Dock = DockStyle.Top,
            Height = 70,
            BackColor = UIHelper.CorPrimaria
        };
        var lblTitulo = new Label
        {
            Text = "Menu Principal",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 16F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 20)
        };
        topo.Controls.Add(lblTitulo);

        // Painel central com os botoes em grade 2x2.
        var painelBotoes = new TableLayoutPanel
        {
            Location = new Point(40, 100),
            Size = new Size(700, 350),
            ColumnCount = 2,
            RowCount = 2,
            CellBorderStyle = TableLayoutPanelCellBorderStyle.None
        };
        painelBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        painelBotoes.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50));
        painelBotoes.RowStyles.Add(new RowStyle(SizeType.Percent, 50));
        painelBotoes.RowStyles.Add(new RowStyle(SizeType.Percent, 50));

        // Cria os botoes principais. Cada um abre a respectiva tela.
        painelBotoes.Controls.Add(CriarBotaoMenu("Produtos", AbrirProdutos), 0, 0);
        painelBotoes.Controls.Add(CriarBotaoMenu("Funcionarios", AbrirFuncionarios), 1, 0);
        painelBotoes.Controls.Add(CriarBotaoMenu("Fornecedores", AbrirFornecedores), 0, 1);
        painelBotoes.Controls.Add(CriarBotaoMenu("Vendas", AbrirVendas), 1, 1);

        // Botao "Sair" no rodape.
        var btnSair = new Button
        {
            Text = "Sair",
            Location = new Point(640, 460),
            Size = new Size(100, 32)
        };
        UIHelper.EstilizarBotaoSecundario(btnSair);
        btnSair.Click += (_, _) => Close();

        Controls.Add(topo);
        Controls.Add(painelBotoes);
        Controls.Add(btnSair);
    }

    /// <summary>
    /// Helper que cria um botao grande para o menu principal.
    /// Recebe o texto e a acao a ser executada ao clicar.
    /// </summary>
    private Button CriarBotaoMenu(string texto, EventHandler aoClicar)
    {
        var btn = new Button
        {
            Text = texto,
            Dock = DockStyle.Fill,
            Margin = new Padding(15),
            BackColor = Color.White,
            ForeColor = UIHelper.CorPrimaria,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            FlatStyle = FlatStyle.Flat,
            Cursor = Cursors.Hand
        };
        btn.FlatAppearance.BorderColor = UIHelper.CorPrimaria;
        btn.FlatAppearance.BorderSize = 2;
        btn.Click += aoClicar;
        return btn;
    }

    // Cada handler abre o respectivo Form como dialogo (modal).
    // ShowDialog() bloqueia o menu ate o usuario fechar a tela aberta,
    // o que evita que multiplas janelas iguais sejam abertas ao mesmo tempo.

    private void AbrirProdutos(object? s, EventArgs e)
    {
        using var f = Program.Services.GetRequiredService<ProdutoForm>();
        f.ShowDialog(this);
    }

    private void AbrirFuncionarios(object? s, EventArgs e)
    {
        using var f = Program.Services.GetRequiredService<FuncionarioForm>();
        f.ShowDialog(this);
    }

    private void AbrirFornecedores(object? s, EventArgs e)
    {
        using var f = Program.Services.GetRequiredService<FornecedorForm>();
        f.ShowDialog(this);
    }

    private void AbrirVendas(object? s, EventArgs e)
    {
        using var f = Program.Services.GetRequiredService<VendaForm>();
        f.ShowDialog(this);
    }
}
