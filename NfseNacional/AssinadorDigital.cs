using System;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;
using System.Xml;

namespace NfseNacional.Dominio
{
	public class AssinadorDigital
	{
		public string AssinarDps( string xmlTexto, string serialCertificado )
		{
			XmlDocument doc = new XmlDocument();
			doc.PreserveWhitespace = true;
			doc.LoadXml( xmlTexto );

			var certificado = BuscarCertificadoPorSerial( serialCertificado );
			if ( certificado == null ) throw new Exception( "Certificado não encontrado." );

			var listaInfDps = doc.GetElementsByTagName( "infDPS" );
			if ( listaInfDps.Count == 0 ) throw new Exception( "infDPS não encontrado" );

			XmlElement infDpsElement = (XmlElement)listaInfDps[0];
			string idTarget = infDpsElement.GetAttribute( "Id" );

			SignedXml signedXml = new SignedXml( doc );

			// --- SOLUÇÃO PARA CHAVES NÃO EXPORTÁVEIS E SHA-256 ---
			RSACryptoServiceProvider keyOriginal = (RSACryptoServiceProvider)certificado.PrivateKey;

			// Se o provedor original não for Type 24 (que suporta SHA-256), tentamos abrir o container com Type 24
			if ( keyOriginal.CspKeyContainerInfo.ProviderType != 24 )
			{
				try
				{
					// Montamos os parâmetros para acessar A MESMA chave, mas com o provedor PROV_RSA_AES (24)
					CspParameters cspParams = new CspParameters( 24 );
					cspParams.KeyContainerName = keyOriginal.CspKeyContainerInfo.KeyContainerName;
					cspParams.KeyNumber = (int)keyOriginal.CspKeyContainerInfo.KeyNumber;

					// Se a chave estiver na máquina (e não no usuário), precisamos avisar
					if ( keyOriginal.CspKeyContainerInfo.MachineKeyStore )
					{
						cspParams.Flags = CspProviderFlags.UseMachineKeyStore;
					}

					// IMPORTANTE: Avisa que queremos usar a chave que JÁ EXISTE, não criar uma nova
					cspParams.Flags |= CspProviderFlags.UseExistingKey;

					// Cria o novo provedor apontando para a chave existente
					RSACryptoServiceProvider sha256Key = new RSACryptoServiceProvider( cspParams );

					signedXml.SigningKey = sha256Key;
				}
				catch ( Exception )
				{
					// Se falhar (ex: Token A3 com driver restrito), tenta usar a chave original mesmo
					// (Pode falhar depois no ComputeSignature se o driver não suportar SHA256)
					signedXml.SigningKey = keyOriginal;
				}
			}
			else
			{
				signedXml.SigningKey = keyOriginal;
			}
			// --------------------------------------------------------

			// Configurações SHA-256 (Requer que a classe RSAPKCS1SHA256... esteja registrada no Program.cs)
			signedXml.SignedInfo.SignatureMethod = "http://www.w3.org/2001/04/xmldsig-more#rsa-sha256";
			signedXml.SignedInfo.CanonicalizationMethod = SignedXml.XmlDsigC14NTransformUrl;

			Reference reference = new Reference( "#" + idTarget );
			reference.AddTransform( new XmlDsigEnvelopedSignatureTransform() );
			reference.AddTransform( new XmlDsigC14NTransform() );
			reference.DigestMethod = "http://www.w3.org/2001/04/xmlenc#sha256";

			signedXml.AddReference( reference );

			KeyInfo keyInfo = new KeyInfo();
			keyInfo.AddClause( new KeyInfoX509Data( certificado ) ); // Adiciona o certificado ao KeyInfo
			signedXml.KeyInfo = keyInfo;

			// Calcula a assinatura
			signedXml.ComputeSignature();

			XmlElement xmlDigitalSignature = signedXml.GetXml();
			doc.DocumentElement.AppendChild( doc.ImportNode( xmlDigitalSignature, true ) );

			return doc.OuterXml;
		}

		private X509Certificate2 BuscarCertificadoPorSerial( string serial )
		{
			if ( string.IsNullOrEmpty( serial ) ) return null;
			X509Store store = new X509Store( StoreName.My, StoreLocation.CurrentUser );
			store.Open( OpenFlags.ReadOnly );
			try
			{
				string serialLimpo = serial.Replace( " ", "" ).ToUpper();
				foreach ( var cert in store.Certificates )
				{
					if ( cert.SerialNumber.ToUpper() == serialLimpo ) return cert;
				}
			}
			finally { store.Close(); }
			return null;
		}
	}
}