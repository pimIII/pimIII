using System.Drawing;
using System.Windows.Forms;
using estoque_farmacia.Services;
using Microsoft.Extensions.DependencyInjection;

namespace estoque_farmacia_winforms.Forms;

/// <summary>
/// TELA DE LOGIN
/// =============
///
/// Primeira tela exibida ao abrir o sistema. O usuario digita login
/// e senha; se forem validos, abre o menu principal e fecha esta tela.
///
/// As credenciais aqui estao fixas (admin/123) para simplificar o teste
/// academico. Quando os funcionarios forem cadastrados no banco, a
/// validacao pode ser trocada por uma consulta ao FuncionarioService
/// comparando o hash da senha.
/// </summary>
public class LoginForm : Form
{
    // Campos do formulario. Sao declarados como atributos da classe
    // para que possam ser acessados em qualquer metodo.
    private readonly TextBox _txtLogin = new();
    private readonly TextBox _txtSenha = new();
    private readonly Label _lblErro = new();
    private readonly Button _btnEntrar = new();

    public LoginForm()
    {
        ConfigurarJanela();
        ConstruirTela();
    }

    /// <summary>
    /// Define propriedades gerais da janela (tamanho, titulo, posicao).
    /// </summary>
    private void ConfigurarJanela()
    {
        Text = "Login - Sistema de Gestao de Farmacia";
        Size = new Size(420, 460);
        StartPosition = FormStartPosition.CenterScreen;       // abre centralizado na tela
        FormBorderStyle = FormBorderStyle.FixedSingle;        // impede o usuario de redimensionar
        MaximizeBox = false;
        BackColor = UIHelper.CorFundo;
        Font = new Font("Segoe UI", 9F);
    }

    /// <summary>
    /// Monta todos os controles visuais da tela (titulo, campos, botao).
    /// Em vez de usar o Designer do Visual Studio, criamos os controles
    /// por codigo. Isso facilita a leitura e o aprendizado.
    /// </summary>
    private void ConstruirTela()
    {
        // Faixa azul no topo (cabecalho).
        var topo = new Panel
        {
            Dock = DockStyle.Top,
            Height = 90,
            BackColor = UIHelper.CorPrimaria
        };

        var lblTitulo = new Label
        {
            Text = "FarmaSystem",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 18F, FontStyle.Bold),
            AutoSize = true,
            Location = new Point(30, 20)
        };
        var lblSubtitulo = new Label
        {
            Text = "Sistema de Gestao de Farmacia",
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 10F),
            AutoSize = true,
            Location = new Point(32, 55)
        };
        topo.Controls.Add(lblTitulo);
        topo.Controls.Add(lblSubtitulo);

        // Label e campo de login.
        var lblLogin = new Label { Text = "Usuario", Location = new Point(40, 130) };
        UIHelper.EstilizarLabel(lblLogin);

        _txtLogin.Location = new Point(40, 155);
        _txtLogin.Size = new Size(330, 28);
        UIHelper.EstilizarCampo(_txtLogin);

        // Label e campo de senha.
        var lblSenha = new Label { Text = "Senha", Location = new Point(40, 200) };
        UIHelper.EstilizarLabel(lblSenha);

        _txtSenha.Location = new Point(40, 225);
        _txtSenha.Size = new Size(330, 28);
        _txtSenha.UseSystemPasswordChar = true; // mostra caracteres mascarados
        UIHelper.EstilizarCampo(_txtSenha);

        // Mensagem de erro (comeca invisivel).
        _lblErro.Text = "Usuario ou senha invalidos.";
        _lblErro.ForeColor = UIHelper.CorErro;
        _lblErro.Font = new Font("Segoe UI", 9F);
        _lblErro.Location = new Point(40, 265);
        _lblErro.AutoSize = true;
        _lblErro.Visible = false;

        // Botao "Entrar".
        _btnEntrar.Text = "Entrar";
        _btnEntrar.Location = new Point(40, 300);
        _btnEntrar.Size = new Size(330, 42);
        UIHelper.EstilizarBotaoPrimario(_btnEntrar);
        _btnEntrar.Click += BotaoEntrar_Click;

        // Permite o usuario pressionar Enter em qualquer campo para fazer login.
        AcceptButton = _btnEntrar;

        // Adiciona tudo na janela.
        Controls.Add(topo);
        Controls.Add(lblLogin);
        Controls.Add(_txtLogin);
        Controls.Add(lblSenha);
        Controls.Add(_txtSenha);
        Controls.Add(_lblErro);
        Controls.Add(_btnEntrar);
    }

    /// <summary>
    /// Acao executada quando o usuario clica em "Entrar".
    /// </summary>
    private void BotaoEntrar_Click(object? sender, EventArgs e)
    {
        var login = _txtLogin.Text.Trim();
        var senha = _txtSenha.Text;

        // Validacao simples local. Em uma versao futura, isso poderia
        // consultar a tabela Funcionarios e comparar o SenhaHash.
        if (login == "admin" && senha == "123")
        {
            // Pega o MenuForm via injecao de dependencia e abre.
            var menu = Program.Services.GetRequiredService<MenuForm>();
            menu.Show();

            // Esconde a tela de login (em vez de fechar) para que o
            // Application.Run nao encerre a aplicacao.
            Hide();

            // Quando o menu for fechado, encerramos a aplicacao.
            menu.FormClosed += (_, _) => Application.Exit();
        }
        else
        {
            _lblErro.Visible = true;
            _txtSenha.Clear();
            _txtLogin.Focus();
        }
    }
}
