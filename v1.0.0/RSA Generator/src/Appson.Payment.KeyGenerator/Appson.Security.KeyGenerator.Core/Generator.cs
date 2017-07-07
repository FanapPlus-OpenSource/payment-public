using System.IO;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Appson.Security.KeyGenerator.Core
{
	public static class Generator
	{
		public static SignatureInformation Generate()
		{
			var keyPairGenerator = new RsaKeyPairGenerator();
			var parameters = new KeyGenerationParameters(new SecureRandom(), 2048);
			keyPairGenerator.Init(parameters);

			var keyPair = keyPairGenerator.GenerateKeyPair();

			var privatePem = GetPrivateKey(keyPair);//GetText(keyPair.Private);
			var publicPem = GetText(keyPair.Public);

			var rsaParameters = DotNetUtilities.ToRSAParameters((RsaPrivateCrtKeyParameters)keyPair.Private);

			var cryptoServiceProvider = new RSACryptoServiceProvider();
			cryptoServiceProvider.ImportParameters(rsaParameters);

			var publicXml = cryptoServiceProvider.ToXmlString(false);
			var privateXml = cryptoServiceProvider.ToXmlString(true);
			return new SignatureInformation
			{
				PublicXml = publicXml,
				PrivateXml = privateXml,
				PublicPem = publicPem,
				PrivatePem = privatePem
			};
		}

		private static string GetText(AsymmetricKeyParameter key)
		{
			var stringWriter = new StringWriter();
			var pemWriter = new PemWriter(stringWriter);

			pemWriter.WriteObject(key);

			pemWriter.Writer.Flush();
			return stringWriter.ToString();
		}
		private static string GetPrivateKey(AsymmetricCipherKeyPair key)
		{
			var stringWriter = new StringWriter();
			var pemWriter = new PemWriter(stringWriter);

			pemWriter.WriteObject(new Pkcs8Generator(key.Private));

			pemWriter.Writer.Flush();
			return stringWriter.ToString();
		}
	}
}