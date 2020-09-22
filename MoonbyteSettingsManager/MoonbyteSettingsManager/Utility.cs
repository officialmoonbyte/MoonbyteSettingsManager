using System;
using System.Security.Cryptography;
using System.Text;

namespace MoonbyteSettingsManager
{
    public static class Utility
    {

        #region GenerateKey(int size)

        public static string GeneratePublicKey(RSACryptoServiceProvider rsa) => rsa.ToXmlString(false);

        public static string GeneratePrivateKey(RSACryptoServiceProvider rsa) => rsa.ToXmlString(true);


        #endregion GenerateKey



    }
}
