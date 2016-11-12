using System;
using System.Security.Cryptography;
using System.Text;

namespace PaymentSample.Common
{
    public class SignatureHelper
    {
        public static SignatureInformation CreateNewSignatureInformation()
        {
            try
            {
                var signatureInformation = new SignatureInformation();
                // Create a new key pair on target CSP
                var cspParams = new CspParameters
                {
                    ProviderType = 1,
                    Flags = CspProviderFlags.UseArchivableKey,
                    KeyNumber = (int) KeyNumber.Exchange
                };
                var rsaProvider = new RSACryptoServiceProvider(cspParams);
                var publicKey = rsaProvider.ToXmlString(false);

                signatureInformation.Public = publicKey;
                var privateKey = rsaProvider.ToXmlString(true);
                signatureInformation.Private = privateKey;

                return signatureInformation;
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static string Sign(string key, string text)
        {
            try
            {
                var cspParams = new CspParameters {ProviderType = 1};
                var rsaProvider = new RSACryptoServiceProvider(cspParams);
                rsaProvider.FromXmlString(key);
                var plainBytes = Encoding.UTF8.GetBytes(text);

                var encryptedBytes = rsaProvider.SignData(plainBytes, new SHA1CryptoServiceProvider());

                return Convert.ToBase64String(encryptedBytes);
            }
            catch (Exception)
            {
                return null;
            }
        }

        public static bool Check(string key, string signedText, string text)
        {
            try
            {
                var cspParams = new CspParameters {ProviderType = 1};
                var rsaProvider = new RSACryptoServiceProvider(cspParams);
                rsaProvider.FromXmlString(key);
                var encryptedBytes = Convert.FromBase64String(signedText);
                var plainInput = Encoding.UTF8.GetBytes(text);
                var check = rsaProvider.VerifyData(plainInput, new SHA1CryptoServiceProvider(), encryptedBytes);
                return check;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}