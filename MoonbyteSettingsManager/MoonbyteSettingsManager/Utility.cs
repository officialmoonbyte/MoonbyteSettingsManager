using System;
using System.Security.Cryptography;
using System.Text;

namespace MoonbyteSettingsManager
{
    public static class Utility
    {

        #region GenerateKey(int size)

        public static string GeneratePublicKey(Aes aesEncryption)
        {
            aesEncryption.GenerateIV();
            aesEncryption.GenerateKey();

            return Convert.ToBase64String(aesEncryption.Key);
        }

        public static string GeneratePrivateKey(RSACryptoServiceProvider rsa) => rsa.ToXmlString(true);


        #endregion GenerateKey



    }
}
