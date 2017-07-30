using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.OpenSsl;
using Org.BouncyCastle.Security;

namespace Appson.Security.KeyGenerator.Core.Tests
{
	[TestClass]
	public class GeneratorTests
	{
		private const string SampleText = "this is a text";
		private SignatureInformation _signatureInfo;

		[TestInitialize]
		public void GeneratesKey()
		{
			_signatureInfo = Generator.Generate();
		}

		[TestMethod]
		public void SignedTextWithPrivateXmlKeyIsVerifiableUsingPublicXmlKey()
		{
			var signature = SignUsingPrivateXmlKey(SampleText);
			var checkResult = VerifySignatureUsingPublicXmlKey(signature, SampleText);
			Assert.IsTrue(checkResult);
		}

		[TestMethod]
		public void SignedTextWithPrivatePemKeyIsVerifiableUsingPublicPemKey()
		{
			var signature = SignUsingPrivatePemKey(SampleText);
			var checkResult = VerifySignatureUsingPublicPemKey(signature, SampleText);
			Assert.IsTrue(checkResult);
		}

		[TestMethod]
		public void SignedTextWithPrivatePemKeyIsVerifiableUsingPublicXmlKey()
		{
			var signature = SignUsingPrivatePemKey(SampleText);
			var checkResult = VerifySignatureUsingPublicXmlKey(signature, SampleText);
			Assert.IsTrue(checkResult);
		}

		[TestMethod]
		public void SignedTextWithPrivateXmlKeyIsVerifiableUsingPublicPemKey()
		{
			var signature = SignUsingPrivateXmlKey(SampleText);
			var checkResult = VerifySignatureUsingPublicPemKey(signature, SampleText);
			Assert.IsTrue(checkResult);
		}

		#region Private Methods

		private string SignUsingPrivatePemKey(string message)
		{
			RsaPrivateCrtKeyParameters privateKey;

			using (var reader = new StringReader(_signatureInfo.PrivatePem))
			{
				privateKey = (RsaPrivateCrtKeyParameters) new PemReader(reader).ReadObject();
			}

			var signer = SignerUtilities.GetSigner("SHA1withRSA");

			signer.Init(true, privateKey);

			var sampleTextBytes = Encoding.UTF8.GetBytes(message);
			signer.BlockUpdate(sampleTextBytes, 0, sampleTextBytes.Length);

			var signBytes = signer.GenerateSignature();
			var signature = Convert.ToBase64String(signBytes);

			return signature;
		}

		private bool VerifySignatureUsingPublicPemKey(string signature, string message)
		{
			var msgBytes = Encoding.UTF8.GetBytes(message);
			var sigBytes = Convert.FromBase64String(signature);


			RsaKeyParameters keyParameters;

			using (var reader = new StringReader(_signatureInfo.PublicPem))
			{
				keyParameters = (RsaKeyParameters) new PemReader(reader).ReadObject();
			}
			var signer = SignerUtilities.GetSigner("SHA1withRSA");
			signer.Init(false, keyParameters);
			signer.BlockUpdate(msgBytes, 0, msgBytes.Length);
			var check = signer.VerifySignature(sigBytes);
			return check;
		}

		private string SignUsingPrivateXmlKey(string message)
		{
			try
			{
				var cspParams = new CspParameters {ProviderType = 1};
				var rsaProvider = new RSACryptoServiceProvider(cspParams);
				rsaProvider.FromXmlString(_signatureInfo.PrivateXml);
				var plainBytes = Encoding.UTF8.GetBytes(message);

				var encryptedBytes = rsaProvider.SignData(plainBytes, new SHA1CryptoServiceProvider());

				return Convert.ToBase64String(encryptedBytes);
			}
			catch
			{
				return string.Empty;
			}
		}

		private bool VerifySignatureUsingPublicXmlKey(string signature, string message)
		{
			var cspParams = new CspParameters {ProviderType = 1};
			var rsaProvider = new RSACryptoServiceProvider(cspParams);
			rsaProvider.FromXmlString(_signatureInfo.PublicXml);
			var encryptedBytes = Convert.FromBase64String(signature);
			var plainInput = Encoding.UTF8.GetBytes(message);
			var check = rsaProvider.VerifyData(plainInput, new SHA1CryptoServiceProvider(), encryptedBytes);

			return check;
		}

		#endregion
	}
}