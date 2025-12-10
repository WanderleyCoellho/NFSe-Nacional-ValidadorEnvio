using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

public class NfseService
{
	public static void ConfigurarTls12()
	{
		// TRUQUE PARA VS2012 / .NET 4.5:
		// O Enum Tls12 não existe nativamente no 4.5 sem patch, então usamos o cast numérico (3072).
		try
		{
			ServicePointManager.SecurityProtocol = (SecurityProtocolType)3072;
		}
		catch
		{
			// Fallback se o cast falhar (mas geralmente é necessário para gov.br)
			ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls | SecurityProtocolType.Ssl3;
		}

		// Ignorar erros de validação de certificado do servidor (útil em homologação, perigoso em produção)
		ServicePointManager.ServerCertificateValidationCallback += ( sender, cert, chain, sslPolicyErrors ) => true;
	}

	public async Task<string> EnviarDpsAsync( string xmlDpsAssinado, string serialCertificado )
	{
		ConfigurarTls12();

		// 1. Buscar o Certificado Digital no Repositório do Windows
		var certificado = BuscarCertificado( serialCertificado );

		if ( certificado == null )
			throw new Exception( "Certificado digital não encontrado." );


		using ( var handler = new WebRequestHandler() )
		{
			handler.ClientCertificates.Add( certificado );

			using ( var client = new HttpClient( handler ) )
			{
				// Configura Headers para JSON
				client.DefaultRequestHeaders.Accept.Clear();

				client.DefaultRequestHeaders.Accept.Add( new MediaTypeWithQualityHeaderValue( "application/json" ) );


				// 1. Compacta e Codifica (GZip + Base64)
				string dpsBase64 = CompactarEnviar( xmlDpsAssinado );

				// 2. Monta o JSON
				string jsonBody = string.Format( "{{ \"dpsXmlGZipB64\": \"{0}\" }}", dpsBase64 );

				var content = new StringContent( jsonBody, Encoding.UTF8, "application/json" );

				string urlAbsoluta = "https://sefin.producaorestrita.nfse.gov.br/SefinNacional/nfse"; //versão 2

				HttpResponseMessage response = await client.PostAsync( urlAbsoluta, content );

				string responseString = await response.Content.ReadAsStringAsync();

				if ( !response.IsSuccessStatusCode )
				{
					throw new Exception( "Erro API (" + response.StatusCode + "): " + responseString );
				}

				return responseString;
			}
		}
	}

	public async Task<string> ConsultarDpsAsync( string idDps, string serialCertificado )
	{
		ConfigurarTls12(); // Garante TLS 1.2

		var certificado = BuscarCertificado( serialCertificado );
		if ( certificado == null ) throw new Exception( "Certificado não encontrado." );

		WebRequestHandler handler = new WebRequestHandler();
		handler.ClientCertificates.Add( certificado );

		using ( var client = new HttpClient( handler ) )
		{
			client.BaseAddress = new Uri( "https://adn.producaorestrita.nfse.gov.br/municipios/api/v1/" ); // Base correta
			client.DefaultRequestHeaders.Accept.Clear();

			// Rota padrão para consultar uma DPS específica
			// O idDps é aquela string longa: "DPS3550308..."
			string endpoint = string.Format( "dps/{0}", idDps );

			HttpResponseMessage response = await client.GetAsync( endpoint );

			string retorno = await response.Content.ReadAsStringAsync();

			if ( !response.IsSuccessStatusCode )
			{
				return "Erro na Consulta: " + response.StatusCode + " - " + retorno;
			}

			return retorno; // Aqui deve vir o XML da NFS-e se ela existir
		}
	}

	public X509Certificate2 BuscarCertificado( string serial )
	{
		X509Store store = new X509Store( StoreName.My, StoreLocation.CurrentUser );

		store.Open( OpenFlags.ReadOnly );

		foreach ( var cert in store.Certificates )
		{
			// Remove espaços e deixa maiúsculo para comparar
			if ( cert.SerialNumber.Replace( " ", "" ).ToUpper() == serial.Replace( " ", "" ).ToUpper() )
			{
				// VERIFICAÇÃO CRÍTICA - Verificar se o Certificado Digital está com CHAVE PRIVADA:
				if ( cert != null && cert.HasPrivateKey )
				{
					return cert; // Certificado encontrado e com chave privada!
				}
			}
		}

		return null;
	}

	public static string CompactarEnviar( string xmlTexto )
	{
		// 1. Converte a string XML para bytes (UTF-8)
		byte[] buffer = Encoding.UTF8.GetBytes( xmlTexto );

		using ( MemoryStream memoryStream = new MemoryStream() )
		{
			using ( GZipStream gZipStream = new GZipStream( memoryStream, CompressionMode.Compress, true ) )
			{
				gZipStream.Write( buffer, 0, buffer.Length );
			}
			// Importante: O stream precisa ser fechado/flushado antes de pegar o array

			// 2. Pega os bytes compactados e converte para Base64
			return Convert.ToBase64String( memoryStream.ToArray() );
		}
	}
}