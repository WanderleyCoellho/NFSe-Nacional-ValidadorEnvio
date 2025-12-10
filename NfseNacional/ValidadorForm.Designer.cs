namespace NfseNacional.Dominio
{
	partial class ValidadorForm
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose( bool disposing )
		{
			if ( disposing && ( components != null ) )
			{
				components.Dispose();
			}
			base.Dispose( disposing );
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.panel1 = new System.Windows.Forms.Panel();
			this.txtLog = new System.Windows.Forms.RichTextBox();
			this.btnExecutar = new System.Windows.Forms.Button();
			this.txtSerialCert = new System.Windows.Forms.TextBox();
			this.label3 = new System.Windows.Forms.Label();
			this.btnSelecionarArquivo = new System.Windows.Forms.Button();
			this.txtArquivoXml = new System.Windows.Forms.TextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.btnSelecionarPasta = new System.Windows.Forms.Button();
			this.txtPastaXsd = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnProcurarCertificado = new System.Windows.Forms.Button();
			this.btnConsultar = new System.Windows.Forms.Button();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// panel1
			// 
			this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
			this.panel1.Controls.Add(this.btnConsultar);
			this.panel1.Controls.Add(this.btnProcurarCertificado);
			this.panel1.Controls.Add(this.txtLog);
			this.panel1.Controls.Add(this.btnExecutar);
			this.panel1.Controls.Add(this.txtSerialCert);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.btnSelecionarArquivo);
			this.panel1.Controls.Add(this.txtArquivoXml);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.btnSelecionarPasta);
			this.panel1.Controls.Add(this.txtPastaXsd);
			this.panel1.Controls.Add(this.label1);
			this.panel1.Dock = System.Windows.Forms.DockStyle.Fill;
			this.panel1.Location = new System.Drawing.Point(0, 0);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(426, 407);
			this.panel1.TabIndex = 0;
			// 
			// txtLog
			// 
			this.txtLog.Location = new System.Drawing.Point(10, 247);
			this.txtLog.Name = "txtLog";
			this.txtLog.Size = new System.Drawing.Size(404, 146);
			this.txtLog.TabIndex = 12;
			this.txtLog.Text = "";
			// 
			// btnExecutar
			// 
			this.btnExecutar.Location = new System.Drawing.Point(31, 191);
			this.btnExecutar.Name = "btnExecutar";
			this.btnExecutar.Size = new System.Drawing.Size(141, 38);
			this.btnExecutar.TabIndex = 11;
			this.btnExecutar.Text = "Validar e Enviar";
			this.btnExecutar.UseVisualStyleBackColor = true;
			this.btnExecutar.Click += new System.EventHandler(this.btnExecutar_Click);
			// 
			// txtSerialCert
			// 
			this.txtSerialCert.Location = new System.Drawing.Point(129, 152);
			this.txtSerialCert.Name = "txtSerialCert";
			this.txtSerialCert.Size = new System.Drawing.Size(283, 20);
			this.txtSerialCert.TabIndex = 10;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 136);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(136, 13);
			this.label3.TabIndex = 9;
			this.label3.Text = "Serial do Certificado Digital:";
			// 
			// btnSelecionarArquivo
			// 
			this.btnSelecionarArquivo.Location = new System.Drawing.Point(10, 94);
			this.btnSelecionarArquivo.Name = "btnSelecionarArquivo";
			this.btnSelecionarArquivo.Size = new System.Drawing.Size(120, 23);
			this.btnSelecionarArquivo.TabIndex = 8;
			this.btnSelecionarArquivo.Text = "Slecionar XML";
			this.btnSelecionarArquivo.UseVisualStyleBackColor = true;
			this.btnSelecionarArquivo.Click += new System.EventHandler(this.btnSelecionarArquivo_Click);
			// 
			// txtArquivoXml
			// 
			this.txtArquivoXml.Location = new System.Drawing.Point(129, 95);
			this.txtArquivoXml.Name = "txtArquivoXml";
			this.txtArquivoXml.ReadOnly = true;
			this.txtArquivoXml.Size = new System.Drawing.Size(283, 20);
			this.txtArquivoXml.TabIndex = 7;
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(15, 79);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(102, 13);
			this.label2.TabIndex = 6;
			this.label2.Text = "Arquivo XML (DPS):";
			// 
			// btnSelecionarPasta
			// 
			this.btnSelecionarPasta.Location = new System.Drawing.Point(10, 36);
			this.btnSelecionarPasta.Name = "btnSelecionarPasta";
			this.btnSelecionarPasta.Size = new System.Drawing.Size(120, 23);
			this.btnSelecionarPasta.TabIndex = 5;
			this.btnSelecionarPasta.Text = "Slecionar Pasta XDS";
			this.btnSelecionarPasta.UseVisualStyleBackColor = true;
			this.btnSelecionarPasta.Click += new System.EventHandler(this.btnSelecionarPasta_Click);
			// 
			// txtPastaXsd
			// 
			this.txtPastaXsd.Location = new System.Drawing.Point(129, 37);
			this.txtPastaXsd.Name = "txtPastaXsd";
			this.txtPastaXsd.ReadOnly = true;
			this.txtPastaXsd.Size = new System.Drawing.Size(283, 20);
			this.txtPastaXsd.TabIndex = 4;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(15, 21);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(109, 13);
			this.label1.TabIndex = 3;
			this.label1.Text = "Pasta dos Esquemas:";
			// 
			// btnProcurarCertificado
			// 
			this.btnProcurarCertificado.Location = new System.Drawing.Point(10, 150);
			this.btnProcurarCertificado.Name = "btnProcurarCertificado";
			this.btnProcurarCertificado.Size = new System.Drawing.Size(120, 23);
			this.btnProcurarCertificado.TabIndex = 13;
			this.btnProcurarCertificado.Text = "Procurar";
			this.btnProcurarCertificado.UseVisualStyleBackColor = true;
			this.btnProcurarCertificado.Click += new System.EventHandler(this.btnProcurarCertificado_Click);
			// 
			// btnConsultar
			// 
			this.btnConsultar.Location = new System.Drawing.Point(237, 191);
			this.btnConsultar.Name = "btnConsultar";
			this.btnConsultar.Size = new System.Drawing.Size(141, 38);
			this.btnConsultar.TabIndex = 14;
			this.btnConsultar.Text = "Consultar Status DPS";
			this.btnConsultar.UseVisualStyleBackColor = true;
			this.btnConsultar.Click += new System.EventHandler(this.btnConsultar_Click);
			// 
			// ValidadorForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(426, 407);
			this.Controls.Add(this.panel1);
			this.Name = "ValidadorForm";
			this.Text = "ValidadorForm";
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.RichTextBox txtLog;
		private System.Windows.Forms.Button btnExecutar;
		private System.Windows.Forms.TextBox txtSerialCert;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Button btnSelecionarArquivo;
		private System.Windows.Forms.TextBox txtArquivoXml;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSelecionarPasta;
		private System.Windows.Forms.TextBox txtPastaXsd;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnProcurarCertificado;
		private System.Windows.Forms.Button btnConsultar;
	}
}