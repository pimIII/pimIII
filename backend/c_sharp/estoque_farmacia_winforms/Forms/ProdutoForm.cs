using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Models;
using estoque_farmacia.Services;

namespace estoque_farmacia_winforms.Forms;

/// <summary>
/// TELA DE CADASTRO DE PRODUTOS
/// =============================
///
/// Permite cadastrar, listar e remover produtos. Os dados ficam
/// no PostgreSQL via Entity Framework Core.
///
/// Layout:
///   - Painel da esquerda: formulario com os campos de cadastro
///   - Painel da direita: tabela com os produtos ja cadastrados
/// </summary>
public class ProdutoForm : Form
{
    private readonly ProdutoService _service;

    // Campos do formulario.
    private readonly TextBox _txtNome = new();
    private readonly TextBox _txtPrecoVenda = new();
    private readonly TextBox _txtPrecoCusto = new();
    private readonly TextBox _txtIdFornecedor = new();
    private readonly CheckBox _chkRequerReceita = new();
    private readonly DataGridView _grid = new();

    /// <summary>
    /// Construtor: recebe o ProdutoService via injecao de dependencia.
    /// O service ja vem com o AppDbContext pronto para uso.
    /// </summary>
    public ProdutoForm(ProdutoService service)
    {
        _service = service;
        ConfigurarJanela();
        ConstruirTela();
        CarregarDados();
    }

    private void ConfigurarJanela()
    {
        Text = "Cadastro de Produtos";
        Size = new Size(950, 580);
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
            Text = "Produtos",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(20, 15)
        };
        topo.Controls.Add(lbl);
        Controls.Add(topo);

        // Painel esquerdo com o formulario de cadastro.
        var painelForm = new Panel
        {
            Location = new Point(20, 80),
            Size = new Size(320, 460),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        int y = 20;

        AdicionarCampo(painelForm, "Nome do produto", _txtNome, ref y);
        AdicionarCampo(painelForm, "Preco de venda (ex: 12,50)", _txtPrecoVenda, ref y);
        AdicionarCampo(painelForm, "Preco de custo (ex: 8,00)", _txtPrecoCusto, ref y);
        AdicionarCampo(painelForm, "ID do fornecedor", _txtIdFornecedor, ref y);

        _chkRequerReceita.Text = "Requer receita medica";
        _chkRequerReceita.Location = new Point(20, y);
        _chkRequerReceita.Font = new Font("Segoe UI", 9F);
        _chkRequerReceita.AutoSize = true;
        painelForm.Controls.Add(_chkRequerReceita);
        y += 35;

        // Botoes do formulario.
        var btnSalvar = new Button { Text = "Salvar", Location = new Point(20, y), Size = new Size(140, 38) };
        UIHelper.EstilizarBotaoPrimario(btnSalvar);
        btnSalvar.Click += BotaoSalvar_Click;

        var btnLimpar = new Button { Text = "Limpar", Location = new Point(170, y), Size = new Size(130, 38) };
        UIHelper.EstilizarBotaoSecundario(btnLimpar);
        btnLimpar.Click += (_, _) => LimparCampos();

        painelForm.Controls.Add(btnSalvar);
        painelForm.Controls.Add(btnLimpar);

        Controls.Add(painelForm);

        // Painel direito com a grade de produtos cadastrados.
        var painelGrid = new Panel
        {
            Location = new Point(360, 80),
            Size = new Size(560, 460),
            BackColor = Color.White,
            BorderStyle = BorderStyle.FixedSingle
        };

        _grid.Location = new Point(10, 10);
        _grid.Size = new Size(540, 400);
        _grid.AllowUserToAddRows = false;        // nao mostra linha em branco para nova entrada
        _grid.ReadOnly = true;
        _grid.SelectionMode = DataGridViewSelectionMode.FullRowSelect;
        _grid.MultiSelect = false;
        _grid.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        _grid.BackgroundColor = Color.White;
        _grid.RowHeadersVisible = false;

        var btnRemover = new Button { Text = "Remover selecionado", Location = new Point(10, 415), Size = new Size(200, 35) };
        UIHelper.EstilizarBotaoPerigo(btnRemover);
        btnRemover.Click += BotaoRemover_Click;

        var btnAtualizar = new Button { Text = "Atualizar lista", Location = new Point(220, 415), Size = new Size(150, 35) };
        UIHelper.EstilizarBotaoSecundario(btnAtualizar);
        btnAtualizar.Click += (_, _) => CarregarDados();

        painelGrid.Controls.Add(_grid);
        painelGrid.Controls.Add(btnRemover);
        painelGrid.Controls.Add(btnAtualizar);
        Controls.Add(painelGrid);
    }

    /// <summary>
    /// Helper que adiciona um par "label + textbox" no painel de cadastro.
    /// Atualiza o "y" para o proximo campo aparecer logo abaixo.
    /// </summary>
    private static void AdicionarCampo(Control painel, string textoLabel, TextBox campo, ref int y)
    {
        var label = new Label
        {
            Text = textoLabel,
            Location = new Point(20, y),
            AutoSize = true,
            Font = new Font("Segoe UI", 9F, FontStyle.Bold),
            ForeColor = UIHelper.CorTexto
        };
        painel.Controls.Add(label);
        y += 22;

        campo.Location = new Point(20, y);
        campo.Size = new Size(280, 26);
        UIHelper.EstilizarCampo(campo);
        painel.Controls.Add(campo);
        y += 40;
    }

    /// <summary>
    /// Carrega a lista de produtos do banco e exibe na grade.
    /// </summary>
    private void CarregarDados()
    {
        var lista = _service.ListarTodos();

        // Limpa colunas e linhas antes de recarregar.
        _grid.Columns.Clear();
        _grid.Rows.Clear();

        // Define as colunas que serao mostradas. Usamos colunas manuais
        // em vez de DataSource para poder formatar valores (preco em reais).
        _grid.Columns.Add("Id", "ID");
        _grid.Columns.Add("Nome", "Nome");
        _grid.Columns.Add("PrecoVenda", "Venda");
        _grid.Columns.Add("PrecoCusto", "Custo");
        _grid.Columns.Add("Fornecedor", "Fornec.");
        _grid.Columns.Add("Receita", "Receita");

        foreach (var p in lista)
        {
            _grid.Rows.Add(
                p.Id,
                p.NomeProduto,
                p.PrecoVenda.ToString("C2"),                     // C2 = formato monetario com 2 casas
                p.PrecoCusto.ToString("C2"),
                p.IdFornecedor,
                p.RequerReceita ? "Sim" : "Nao");
        }
    }

    /// <summary>
    /// Acao do botao "Salvar". Valida os campos e chama o service.
    /// </summary>
    private void BotaoSalvar_Click(object? sender, EventArgs e)
    {
        // Validacoes basicas antes de ir ao service.
        if (string.IsNullOrWhiteSpace(_txtNome.Text))
        {
            MessageBox.Show("Informe o nome do produto.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // TryParse tenta converter o texto para decimal. Se nao for um
        // numero valido, retorna false e a variavel fica em zero.
        if (!decimal.TryParse(_txtPrecoVenda.Text, out var precoVenda) || precoVenda < 0)
        {
            MessageBox.Show("Preco de venda invalido.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!decimal.TryParse(_txtPrecoCusto.Text, out var precoCusto) || precoCusto < 0)
        {
            MessageBox.Show("Preco de custo invalido.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }
        if (!int.TryParse(_txtIdFornecedor.Text, out var idFornecedor) || idFornecedor <= 0)
        {
            MessageBox.Show("ID do fornecedor invalido.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        // Monta o objeto e envia para o service.
        var novo = new Produto
        {
            NomeProduto = _txtNome.Text.Trim(),
            PrecoVenda = decimal.Round(precoVenda, 2),
            PrecoCusto = decimal.Round(precoCusto, 2),
            IdFornecedor = idFornecedor,
            RequerReceita = _chkRequerReceita.Checked
        };

        if (_service.Salvar(novo))
        {
            MessageBox.Show("Produto salvo.", "Sucesso", MessageBoxButtons.OK, MessageBoxIcon.Information);
            LimparCampos();
            CarregarDados();
        }
        else
        {
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel salvar o produto.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    /// <summary>
    /// Acao do botao "Remover". Confirma e chama o service.
    /// </summary>
    private void BotaoRemover_Click(object? sender, EventArgs e)
    {
        if (_grid.SelectedRows.Count == 0)
        {
            MessageBox.Show("Selecione um produto para remover.", "Atencao", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            return;
        }

        var id = Convert.ToInt32(_grid.SelectedRows[0].Cells["Id"].Value);

        var resp = MessageBox.Show($"Remover produto ID {id}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
        if (resp != DialogResult.Yes) return;

        if (_service.Remover(id))
        {
            CarregarDados();
        }
        else
        {
            var motivo = string.IsNullOrEmpty(_service.UltimoErro) ? "motivo desconhecido" : _service.UltimoErro;
            MessageBox.Show("Nao foi possivel remover.\n\n" + motivo, "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    private void LimparCampos()
    {
        _txtNome.Clear();
        _txtPrecoVenda.Clear();
        _txtPrecoCusto.Clear();
        _txtIdFornecedor.Clear();
        _chkRequerReceita.Checked = false;
        _txtNome.Focus();
    }
}
