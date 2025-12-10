using System;
using System.Windows.Forms;
using System.Security.Cryptography; // Necessário
using NfseNacional;
using NfseNacional.Dominio;

namespace NfseNacional.App
{
	static class Program
	{
		[STAThread]
		static void Main()
		{
			// --- REGISTRO DO ALGORITMO SHA-256 MANUAL ---
			// Aqui usamos a classe que acabamos de criar (RSAPKCS1SHA256SignatureDescription)
			// e associamos ela à URL oficial do SHA-256.
			CryptoConfig.AddAlgorithm(
				typeof( RSAPKCS1SHA256SignatureDescription ),
				"http://www.w3.org/2001/04/xmldsig-more#rsa-sha256" );

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault( false );
			Application.Run( new ValidadorForm() );
		}
	}
}