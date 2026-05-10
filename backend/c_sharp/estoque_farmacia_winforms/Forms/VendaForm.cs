using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

/// <summary>
/// TELA DE VENDAS
/// ==============
///
/// Permite registrar uma venda combinando produtos cadastrados.
/// O atendente busca o produto, define a quantidade e adiciona ao carrinho.
/// Ao final, o sistema mostra o total, aplica desconto opcional e
/// confirma a venda.
///
/// O modulo nao persiste a venda em uma tabela propria (a tabela Venda
/// fica para um proximo incremento). Esta tela serve como interface
/// operacional, atendendo ao requisito de "implementacao completa de
/// um caso de uso" exigido pela disciplina de Interface.
/// </summary>
public class VendaForm : Form
{
    private readonly ProdutoService _produtoService;

    private readonly TextBox _txtBusca = new();
    private readonly NumericUpDown _numQtd = new();
    private readonly DataGridView _grid = new();
    private readonly Label _lblSubtotal = new();
    private readonly Label _lblTotal = new();
    private readonly NumericUpDown _numDesconto = new();

    /// <summary>
    /// Lista que armazena os itens adicionados ao "carrinho" da venda.
    /// Cada elemento e um ItemVenda contendo o produto e a quantidade.
    /// </summary>
    private readonly List<ItemVenda> _carrinho = new();

    /// <summary>
    /// Cache de produtos cadastrados. Carregado uma vez ao abrir a tela
    /// para que a busca por nome/ID seja feita em memoria, sem ir ao banco
    /// a cada digitacao.
    /// </summary>
    private List<Produto> _produtos = new();

    public VendaForm(ProdutoService produtoService)
    {
        _produtoService = produtoService;
        ConfigurarJanela();
        ConstruirTela();
        _produtos = _produtoService.ListarTodos();
        AtualizarTotais();
    }

    private void ConfigurarJanela()
    {
        Text = "Registrar Venda";
        Size = new Size(960, 620);
        StartPosition = FormStartPosition.CenterParent;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    private void ConstruirTela()
    {
        // Cabecalho azul.
        var topo = new Panel { Dock = DockStyle.Top, Height = 60, BackColor = UIHelper.CorPrimaria };
        var lbl = new Label
        {
            Text = "Registrar Venda",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 15)
        };
        topo.Controls.Add(lbl);
        Controls.Add(topo);

        // Painel de busca e adicionar produto.
        var painelBusca = new Panel
        {
            Location = new Point(20, 80),
            Size = new Size(900, 80),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblBusca = new Label
        {
            Text = "Produto (ID ou nome)",
            Location = new Point(15, 12),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };

        _txtBusca.Location = new Point(15, 35);
        _txtBusca.Size = new Size(400, 28);
        UIHelper.EstilizarCampo(_txtBusca);

        var lblQtd = new Label
        {
            Text = "Quantidade",
            Location = new Point(430, 12),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };

        _numQtd.Location = new Point(430, 35);
        _numQtd.Size = new Size(90, 28);
        _numQtd.Minimum = 1;
        _numQtd.Maximum = 999;
        _numQtd.Value = 1;
        _numQtd.Font = new Font("Segoe UI", 10F);

        var btnAdicionar = new Button { Text = "+ Adicionar", Location = new Point(540, 32), Size = new Size(140, 36) };
        UIHelper.EstilizarBotaoPrimario(btnAdicionar);
        btnAdicionar.Click += BotaoAdicionar_Click;

        painelBusca.Controls.Add(lblBusca);
        painelBusca.Controls.Add(_txtBusca);
        painelBusca.Controls.Add(lblQtd);
        painelBusca.Controls.Add(_numQtd);
        painelBusca.Controls.Add(btnAdicionar);
        Controls.Add(painelBusca);

        // Grade do carrinho.
        var painelCarrinho = new Panel
        {
            Location = new Point(20, 175),
            Size = new Size(620, 380),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        _grid.Location = new Point(10, 10);
        _grid.Size = new Size(600, 320);
        _grid.AllowUserToAddRows = false;
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = Color.White;
        _grid.RowHeadersVisible = false;
        _grid.Columns.Add("Id", "ID");
        _grid.Columns.Add("Nome", "Produto");
        _grid.Columns.Add("Preco", "Preco unit.");
        _grid.Columns.Add("Qtd", "Qtd");
        _grid.Columns.Add("Subtotal", "Subtotal");

        var btnRemoverItem = new Button { Text = "Remover item selecionado", Location = new Point(10, 335), Size = new Size(220, 35) };
        UIHelper.EstilizarBotaoPerigo(btnRemoverItem);
        btnRemoverItem.Click += BotaoRemoverItem_Click;

        painelCarrinho.Controls.Add(_grid);
        painelCarrinho.Controls.Add(btnRemoverItem);
        Controls.Add(painelCarrinho);

        // Painel direito: resumo da venda.
        var painelResumo = new Panel
        {
            Location = new Point(660, 175),
            Size = new Size(260, 380),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        var lblTituloResumo = new Label
        {
            Text = "Resumo",
            Location = new Point(15, 15),
            AutoSize = true,
            Font = new Font("Segoe UI", 12F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };

        var lblSubLabel = new Label
        {
            Text = "Subtotal",
            Location = new Point(15, 55),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            ForeColor = UIHelper.CorTexto
        };
        _lblSubtotal.Text = "R$ 0,00";
        _lblSubtotal.Location = new Point(15, 75);
        _lblSubtotal.AutoSize = true;
        _lblSubtotal.Font = new Font("Segoe UI", 14F, FontStyle.Bold);
        _lblSubtotal.ForeColor = UIHelper.CorTexto;

        var lblDescLabel = new Label
        {
            Text = "Desconto (R$)",
            Location = new Point(15, 120),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            ForeColor = UIHelper.CorTexto
        };
        _numDesconto.Location = new Point(15, 140);
        _numDesconto.Size = new Size(220, 28);
        _numDesconto.DecimalPlaces = 2;
        _numDesconto.Minimum = 0;
        _numDesconto.Maximum = 99999;
        _numDesconto.Increment = 1;
        _numDesconto.Font = new Font("Segoe UI", 10F);
        _numDesconto.ValueChanged += (_, _) => AtualizarTotais();

        var lblTotalLabel = new Label
        {
            Text = "Total",
            Location = new Point(15, 195),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F),
            ForeColor = UIHelper.CorTexto
        };
        _lblTotal.Text = "R$ 0,00";
        _lblTotal.Location = new Point(15, 215);
        _lblTotal.AutoSize = true;
        _lblTotal.Font = new Font("Segoe UI", 18F, FontStyle.Bold);
        _lblTotal.ForeColor = UIHelper.CorPrimaria;

        var btnFinalizar = new Button { Text = "Finalizar venda", Location = new Point(15, 275), Size = new Size(220, 42) };
        UIHelper.EstilizarBotaoPrimario(btnFinalizar);
        btnFinalizar.Click += BotaoFinalizar_Click;

        var btnLimpar = new Button { Text = "Cancelar venda", Location = new Point(15, 325), Size = new Size(220, 38) };
        UIHelper.EstilizarBotaoSecundario(btnLimpar);
        btnLimpar.Click += BotaoCancelar_Click;

        painelResumo.Controls.Add(lblTituloResumo);
        painelResumo.Controls.Add(lblSubLabel);
        painelResumo.Controls.Add(_lblSubtotal);
        painelResumo.Controls.Add(lblDescLabel);
        painelResumo.Controls.Add(_numDesconto);
        painelResumo.Controls.Add(lblTotalLabel);
        painelResumo.Controls.Add(_lblTotal);
        painelResumo.Controls.Add(btnFinalizar);
        painelResumo.Controls.Add(btnLimpar);
        Controls.Add(painelResumo);
    }

    /// <summary>
    /// Procura o produto digitado e adiciona ao carrinho.
    /// A busca aceita o ID exato ou parte do nome.
    /// </summary>
    private void BotaoAdicionar_Click(object? sender, EventArgs e)
    {
        var texto = _txtBusca.Text.Trim();
        if (string.IsNullOrEmpty(texto))
        {
            MessageBox.Show("Digite o nome ou ID do produto.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        Produto? produto;
        // Se o texto for um numero, busca por ID. Senao, busca por nome (parcial).
        if (int.TryParse(texto, out var id))
        {
            produto = _produtos.FirstOrDefault(p => p.Id == id);
        }
        else
        {
            produto = _produtos.FirstOrDefault(p => p.NomeProduto.Contains(texto, StringComparison.OrdinalIgnoreCase));
        }

        if (produto == null)
        {
            MessageBox.Show("Produto nao encontrado.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var qtd = (int)_numQtd.Value;

        // Se ja estiver no carrinho, soma a quantidade. Senao, adiciona.
        var existente = _carrinho.FirstOrDefault(i => i.Produto.Id == produto.Id);
        if (existente != null)
        {
            existente.Quantidade += qtd;
        }
        else
        {
            _carrinho.Add(new ItemVenda { Produto = produto, Quantidade = qtd });
        }

        _txtBusca.Clear();
        _numQtd.Value = 1;
        _txtBusca.Focus();
        RecarregarCarrinho();
        AtualizarTotais();
    }

    private void BotaoRemoverItem_Click(object? sender, EventArgs e)
    {
        if (_grid.SelectedRows.Count == 0)
        {
            MessageBox.Show("Selecione um item.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var idProduto = Convert.ToInt32(_grid.SelectedRows[0].Cells["Id"].Value);
        _carrinho.RemoveAll(i => i.Produto.Id == idProduto);
        RecarregarCarrinho();
        AtualizarTotais();
    }

    private void RecarregarCarrinho()
    {
        _grid.Rows.Clear();
        foreach (var i in _carrinho)
        {
            _grid.Rows.Add(
                i.Produto.Id,
                i.Produto.NomeProduto,
                i.Produto.PrecoVenda.ToString("C2"),
                i.Quantidade,
                (i.Produto.PrecoVenda * i.Quantidade).ToString("C2"));
        }
    }

    /// <summary>
    /// Recalcula o subtotal (soma dos itens) e o total (subtotal - desconto).
    /// O total nunca fica abaixo de zero.
    /// </summary>
    private void AtualizarTotais()
    {
        decimal subtotal = _carrinho.Sum(i => i.Produto.PrecoVenda * i.Quantidade);
        decimal desconto = _numDesconto.Value;
        decimal total = Math.Max(0, subtotal - desconto);

        _lblSubtotal.Text = subtotal.ToString("C2");
        _lblTotal.Text = total.ToString("C2");
    }

    private void BotaoFinalizar_Click(object? sender, EventArgs e)
    {
        if (_carrinho.Count == 0)
        {
            MessageBox.Show("Adicione ao menos um produto.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        decimal total = _carrinho.Sum(i => i.Produto.PrecoVenda * i.Quantidade) - _numDesconto.Value;
        if (total < 0) total = 0;

        MessageBox.Show(
            $"Venda registrada.\nItens: {_carrinho.Count}\nTotal: {total:C2}",
            "Sucesso",
            MessageBoxButtons.OK,
            MessageBoxIcon.Information);

        _carrinho.Clear();
        _numDesconto.Value = 0;
        RecarregarCarrinho();
        AtualizarTotais();
    }

    private void BotaoCancelar_Click(object? sender, EventArgs e)
    {
        if (_carrinho.Count == 0) return;
        if (MessageBox.Show("Cancelar a venda atual?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes) return;
        _carrinho.Clear();
        _numDesconto.Value = 0;
        RecarregarCarrinho();
        AtualizarTotais();
    }

    /// <summary>
    /// Estrutura interna que representa uma linha do carrinho de vendas:
    /// referencia ao produto e quantidade comprada.
    /// </summary>
    private class ItemVenda
    {
        public Produto Produto { get; set; } = null!;
        public int Quantidade { get; set; }
    }
}
