using System;
using System.Windows.Forms;
using Domain.Models;
using Domain.Services;

namespace UI.Forms
{
    public partial class FuncionarioEditForm : Form
    {
        private readonly FuncionarioService _service;
        private Funcionario _model;

        public FuncionarioEditForm(Funcionario model, FuncionarioService service)
        {
            _service = service ?? throw new ArgumentNullException(nameof(service));
            _model = model;
            InitializeComponent();

            if (_model != null)
            {
                txtNome.Text = _model.Nome;
                txtCPF.Text = _model.CPF;
                txtCargo.Text = _model.Cargo;
                numSalario.Value = _model.Salario;
                dtpAdmissao.Value = _model.DataAdmissao;
            }
        }

        private async void btnSalvar_Click(object sender, EventArgs e)
        {
            try
            {
                if (_model == null) _model = new Funcionario();

                _model.Nome = txtNome.Text;
                _model.CPF = txtCPF.Text;
                _model.Cargo = txtCargo.Text;
                _model.Salario = numSalario.Value;
                _model.DataAdmissao = dtpAdmissao.Value.Date;

                if (_model.Id == 0)
                {
                    await _service.CreateAsync(_model);
                }
                else
                {
                    await _service.UpdateAsync(_model);
                }

                this.DialogResult = DialogResult.OK;
                this.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Erro: {ex.Message}", "Erro", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
