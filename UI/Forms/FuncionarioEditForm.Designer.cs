namespace UI.Forms
{
    partial class FuncionarioEditForm
    {
        private System.ComponentModel.IContainer components = null;
        private System.Windows.Forms.Label lblNome;
        private System.Windows.Forms.TextBox txtNome;
        private System.Windows.Forms.Label lblCPF;
        private System.Windows.Forms.TextBox txtCPF;
        private System.Windows.Forms.Label lblCargo;
        private System.Windows.Forms.TextBox txtCargo;
        private System.Windows.Forms.Label lblSalario;
        private System.Windows.Forms.NumericUpDown numSalario;
        private System.Windows.Forms.Label lblAdmissao;
        private System.Windows.Forms.DateTimePicker dtpAdmissao;
        private System.Windows.Forms.Button btnSalvar;
        private System.Windows.Forms.Button btnCancelar;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        private void InitializeComponent()
        {
            this.lblNome = new System.Windows.Forms.Label();
            this.txtNome = new System.Windows.Forms.TextBox();
            this.lblCPF = new System.Windows.Forms.Label();
            this.txtCPF = new System.Windows.Forms.TextBox();
            this.lblCargo = new System.Windows.Forms.Label();
            this.txtCargo = new System.Windows.Forms.TextBox();
            this.lblSalario = new System.Windows.Forms.Label();
            this.numSalario = new System.Windows.Forms.NumericUpDown();
            this.lblAdmissao = new System.Windows.Forms.Label();
            this.dtpAdmissao = new System.Windows.Forms.DateTimePicker();
            this.btnSalvar = new System.Windows.Forms.Button();
            this.btnCancelar = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.numSalario)).BeginInit();
            this.SuspendLayout();
            // 
            // Controls layout (simplified)
            // 
            this.lblNome.Text = "Nome:";
            this.lblNome.Location = new System.Drawing.Point(12, 9);
            this.txtNome.Location = new System.Drawing.Point(80, 6);
            this.txtNome.Size = new System.Drawing.Size(300, 20);
            this.lblCPF.Text = "CPF:";
            this.lblCPF.Location = new System.Drawing.Point(12, 39);
            this.txtCPF.Location = new System.Drawing.Point(80, 36);
            this.txtCPF.Size = new System.Drawing.Size(150, 20);
            this.lblCargo.Text = "Cargo:";
            this.lblCargo.Location = new System.Drawing.Point(12, 69);
            this.txtCargo.Location = new System.Drawing.Point(80, 66);
            this.txtCargo.Size = new System.Drawing.Size(200, 20);
            this.lblSalario.Text = "Salário:";
            this.lblSalario.Location = new System.Drawing.Point(12, 99);
            this.numSalario.Location = new System.Drawing.Point(80, 96);
            this.numSalario.DecimalPlaces = 2;
            this.numSalario.Maximum = 1000000;
            this.lblAdmissao.Text = "Admissão:";
            this.lblAdmissao.Location = new System.Drawing.Point(12, 129);
            this.dtpAdmissao.Location = new System.Drawing.Point(80, 126);
            this.btnSalvar.Text = "Salvar";
            this.btnSalvar.Location = new System.Drawing.Point(80, 160);
            this.btnSalvar.Click += new System.EventHandler(this.btnSalvar_Click);
            this.btnCancelar.Text = "Cancelar";
            this.btnCancelar.Location = new System.Drawing.Point(170, 160);
            this.btnCancelar.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            // 
            // Form
            // 
            this.ClientSize = new System.Drawing.Size(400, 200);
            this.Controls.Add(this.lblNome);
            this.Controls.Add(this.txtNome);
            this.Controls.Add(this.lblCPF);
            this.Controls.Add(this.txtCPF);
            this.Controls.Add(this.lblCargo);
            this.Controls.Add(this.txtCargo);
            this.Controls.Add(this.lblSalario);
            this.Controls.Add(this.numSalario);
            this.Controls.Add(this.lblAdmissao);
            this.Controls.Add(this.dtpAdmissao);
            this.Controls.Add(this.btnSalvar);
            this.Controls.Add(this.btnCancelar);
            this.Name = "FuncionarioEditForm";
            this.Text = "Editar Funcionário";
            ((System.ComponentModel.ISupportInitialize)(this.numSalario)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();
        }
    }
}
