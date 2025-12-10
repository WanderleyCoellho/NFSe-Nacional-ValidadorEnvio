using System;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;
using System.Security.Cryptography.X509Certificates; // Necessário para manipular certificados
using System.Security; // Necessário para a interface de seleção
using NfseNacional.Dominio;

namespace NfseNacional.Dominio // <--- IMPORTANTE: Verifique se este nome bate com o do seu projeto
{
	public partial class ValidadorForm : Form
	{
		public ValidadorForm()
		{
			InitializeComponent();
		}

		// Botão para selecionar a pasta onde estão os arquivos .xsd
		private void btnSelecionarPasta_Click( object sender, EventArgs e )
		{
			using ( var folderDialog = new FolderBrowserDialog() )
			{
				if ( folderDialog.ShowDialog() == DialogResult.OK )
				{
					txtPastaXsd.Text = folderDialog.SelectedPath;
					Log( "Pasta XSD selecionada: " + folderDialog.SelectedPath );
				}
			}
		}

		// Botão para selecionar o arquivo XML que você quer testar
		private void btnSelecionarArquivo_Click( object sender, EventArgs e )
		{
			using ( var fileDialog = new OpenFileDialog() )
			{
				fileDialog.Filter = "Arquivos XML|*.xml";
				if ( fileDialog.ShowDialog() == DialogResult.OK )
				{
					txtArquivoXml.Text = fileDialog.FileName;
					Log( "Arquivo XML selecionado: " + fileDialog.FileName );
				}
			}
		}

		// AÇÃO PRINCIPAL: Valida e Envia
		private async void btnExecutar_Click( object sender, EventArgs e )
		{
			txtLog.Clear();

			if ( string.IsNullOrWhiteSpace( txtPastaXsd.Text ) || string.IsNullOrWhiteSpace( txtArquivoXml.Text ) )
			{
				MessageBox.Show( "Selecione a pasta XSD e o arquivo XML." );
				return;
			}

			try
			{
				btnExecutar.Enabled = false;

				// 1. Ler XML Bruto
				string xmlParaValidar = File.ReadAllText( txtArquivoXml.Text, Encoding.UTF8 );

				// 2. Assinatura Digital (Se tiver serial)
				if ( !string.IsNullOrWhiteSpace( txtSerialCert.Text ) )
				{
					Log( "Assinando o XML digitalmente...", Color.Blue );
					var assinador = new AssinadorDigital();

					// Substitui o XML bruto pelo XML Assinado
					xmlParaValidar = assinador.AssinarDps( xmlParaValidar, txtSerialCert.Text );

					Log( "XML Assinado com sucesso!", Color.Green );
					// (Opcional) Mostra o XML assinado no console/log se quiser conferir
					// Log(xmlParaValidar); 
				}
				else
				{
					Log( "AVISO: Sem certificado. Validando XML sem assinatura...", Color.Orange );
				}

				// 3. Validação XSD (Agora validamos o XML já assinado ou o original)
				Log( "Validando Schema XSD..." );
				var validador = new NfseValidator( txtPastaXsd.Text );

				if ( validador.ValidarXml( xmlParaValidar ) )
				{
					Log( "ESTRUTURA VÁLIDA!", Color.Green );

					// 4. Envio para API
					if ( !string.IsNullOrWhiteSpace( txtSerialCert.Text ) )
					{
						Log( "Enviando para API Nacional...", Color.Blue );
						var servico = new NfseService();

						// Nota: O método EnviarDpsAsync precisa ser ajustado para não buscar o certificado de novo
						// ou podemos passar apenas o XML já assinado. 
						// Vou assumir que o método EnviarDpsAsync que fizemos antes busca o certificado 
						// apenas para autenticar a conexão TLS (Client Certificate), o que é correto.

						string resposta = await servico.EnviarDpsAsync( xmlParaValidar, txtSerialCert.Text );
						Log( "RETORNO DA API:\n" + resposta, Color.Black );
					}
				}
				else
				{
					Log( "XML INVÁLIDO:", Color.Red );
					foreach ( var erro in validador.Erros )
						Log( erro, Color.Red );
				}
			}
			catch ( Exception ex )
			{
				Log( "ERRO: " + ex.Message, Color.Red );
				if ( ex.InnerException != null ) Log( "Detalhe: " + ex.InnerException.Message, Color.Red );
			}
			finally
			{
				btnExecutar.Enabled = true;
			}
		}

		// Método auxiliar para escrever no Log colorido
		private void Log( string texto, Color? cor = null )
		{
			txtLog.SelectionStart = txtLog.TextLength;
			txtLog.SelectionLength = 0;

			txtLog.SelectionColor = cor ?? Color.Black;
			txtLog.AppendText( DateTime.Now.ToString( "HH:mm:ss" ) + ": " + texto + Environment.NewLine );
			txtLog.ScrollToCaret();
		}

		private void btnProcurarCertificado_Click( object sender, EventArgs e )
		{
			// 1. Abre o repositório de certificados do Usuário Atual (onde geralmente ficam os A1/A3)
			X509Store store = new X509Store( StoreName.My, StoreLocation.CurrentUser );
			try
			{
				store.Open( OpenFlags.ReadOnly );

				// 2. Filtra apenas certificados válidos (que não expiraram)
				X509Certificate2Collection collection = (X509Certificate2Collection)store.Certificates;
				X509Certificate2Collection fcollection = (X509Certificate2Collection)collection.Find( X509FindType.FindByTimeValid, DateTime.Now, false );

				// 3. Abre a janela nativa do Windows para seleção
				X509Certificate2Collection scollection = X509Certificate2UI.SelectFromCollection(
					fcollection,
					"Selecionar Certificado",
					"Escolha o certificado digital para assinar o documento:",
					X509SelectionFlag.SingleSelection
				);

				// 4. Se o usuário selecionou algum, pega o Serial e joga na tela
				if ( scollection != null && scollection.Count > 0 )
				{
					X509Certificate2 certSelecionado = scollection[0];

					// Pega o Serial Number
					txtSerialCert.Text = certSelecionado.SerialNumber;

					Log( "Certificado selecionado: " + certSelecionado.Subject );
					Log( "Serial: " + certSelecionado.SerialNumber );
					Log( "Validade: " + certSelecionado.NotAfter.ToString( "dd/MM/yyyy" ) );
				}
			}
			catch ( Exception ex )
			{
				Log( "Erro ao abrir repositório de certificados: " + ex.Message, Color.Red );
			}
			finally
			{
				store.Close();
			}
		}	

		private async void btnConsultar_Click( object sender, EventArgs e )
		{
			try
			{
				// 1. Precisamos descobrir qual ID foi gerado. 
				// Se você estiver usando o XML de teste fixo, o ID está dentro do arquivo XML ou podemos ler do textbox.
				// Para facilitar, vamos ler do arquivo XML que está na tela:

				string xmlContent = File.ReadAllText( txtArquivoXml.Text );

				// Extração simples do ID usando manipulação de string (ou use XmlDocument se preferir)
				// Procura por Id="DPS...
				int indexId = xmlContent.IndexOf( "Id=\"DPS" );
				if ( indexId == -1 )
				{
					MessageBox.Show( "Não foi possível achar o ID da DPS neste XML." );
					return;
				}

				// Pega os 45 caracteres do ID (DPS + 42 números)
				string idDps = xmlContent.Substring( indexId + 4, 45 );

				Log( "Consultando DPS ID: " + idDps + "...", Color.Blue );

				if ( string.IsNullOrWhiteSpace( txtSerialCert.Text ) )
				{
					MessageBox.Show( "Selecione o certificado para consultar." );
					return;
				}

				var servico = new NfseService();

				string resposta = await servico.ConsultarDpsAsync( idDps, txtSerialCert.Text );

				Log( "RETORNO DA CONSULTA:", Color.Black );
				Log( resposta );

				// Verifica se retornou sucesso
				if ( resposta.Contains( "<cStat>100</cStat>" ) )
				{
					Log( "CONFIRMADO: A nota existe e foi autorizada!", Color.Green );
				}
				else if ( resposta.Contains( "não encontrada" ) )
				{
					Log( "A nota ainda não consta na base de dados.", Color.Orange );
				}
			}
			catch ( Exception ex )
			{
				Log( "Erro ao consultar: " + ex.Message, Color.Red );
			}
		}
	}
}