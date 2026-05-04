using System;
using System.Threading.Tasks;
using System.Windows.Forms;
using Domain.Models;
using Domain.Services;

namespace UI.Forms
{
    public partial class FuncionarioForm : Form
    {
        private readonly FuncionarioService _service;

        public FuncionarioForm(FuncionarioService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            InitializeComponent();
            _ = LoadGridAsync();
        }

        private async Task LoadGridAsync()
        {
            try
            {
                var list = await _service.GetAllAsync();
                dgvFuncionarios.DataSource = list;
                dgvFuncionarios.AutoResizeColumns();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro ao carregar funcionários: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private async void btnNovo_Click(object sender, EventArgs e)
        {
            var edit = new FuncionarioEditForm(null, _service);
            if (edit.ShowDialog() == DialogResult.OK)
                await LoadGridAsync();
        }

        private async void btnEditar_Click(object sender, EventArgs e)
        {
            if (dgvFuncionarios.CurrentRow?.DataBoundItem is Funcionario f)
            {
                var edit = new FuncionarioEditForm(f, _service);
                if (edit.ShowDialog() == DialogResult.OK)
                    await LoadGridAsync();
            }
        }

        private async void btnExcluir_Click(object sender, EventArgs e)
        {
            if (dgvFuncionarios.CurrentRow?.DataBoundItem is Funcionario f)
            {
                var confirm = MessageBox.Show($"Confirma exclusão de {f.Nome}?", "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (confirm == DialogResult.Yes)
                {
                    await _service.DeleteAsync(f.Id);
                    await LoadGridAsync();
                }
            }
        }

        private async void txtPesquisar_TextChanged(object sender, EventArgs e)
        {
            var termo = txtPesquisar.Text;
            var result = await _service.SearchByNameAsync(termo);
            dgvFuncionarios.DataSource = result;
        }
    }
}
