using System;
using System.IO;
using System.Security.Cryptography;

namespace MoonbyteSettingsManager
{
    public class EncryptionKey
    {
        #region Vars

        private string _encryptionKeyFileDirectory { get; set; }

        #endregion Vars

        #region Properties

        public string EncryptionKeyFileDirectory
        {
            get { return _encryptionKeyFileDirectory; }
            set
            {
                _encryptionKeyFileDirectory = value;
                SaveToFile();
            }
        }

        #endregion Properties

        #region Initialization

        public EncryptionKey(string encryptionKeyFileDirectory)
        {
            _encryptionKeyFileDirectory = encryptionKeyFileDirectory;
            SaveToFile();
        }

        #endregion Initialization

        #region Public Methods

        public void SaveToFile() 
        { if (!File.Exists(_encryptionKeyFileDirectory)) File.WriteAllText(_encryptionKeyFileDirectory, Utility.GeneratePublicKey(Aes.Create())); }

        public string GetEncryptionKey() => File.ReadAllText(_encryptionKeyFileDirectory);

        #endregion Public Methods

    }
}
