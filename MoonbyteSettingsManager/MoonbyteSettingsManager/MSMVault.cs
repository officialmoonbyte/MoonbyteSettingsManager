using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;

namespace MoonbyteSettingsManager
{
    public class MSMVault
    {

        #region Vars

        private const string Sep = " : ";

        private bool showLog;
        private string settingsDirectory = null;
        private string settingsFullDirectory = null;
        private string settingsFileName = null;

        #endregion Vars

        #region Properties

        public bool ShowLog
        {
            get { return showLog; }
            set { showLog = value; }
        }

        public string SettingsFileName
        {
            get { return settingsFileName; }
            set { settingsFileName = value; UpdateDirectory(); }
        }

        public string SettingsDirectory
        {
            get { return settingsDirectory; }
            set
            { settingsDirectory = value; UpdateDirectory(); }
        }

        #endregion Properties

        #region Public Methods

        #region EditSetting

        public void EditSetting(string SettingTitle, string SettingValue)
        {
            if (CheckValues())
            {
                List<string> Settings = LoadSettings();

                BaseCommands.BaseEditSetting(SettingTitle, SettingValue, Settings);

                SaveSettings(Settings);
            }
        }

        #endregion EditSetting

        #region ReadSetting

        public string ReadSetting(string SettingTitle)
        {
            if (CheckValues())
            {
                List<string> Settings = LoadSettings();

                return BaseCommands.BaseReadSetting(SettingTitle, Settings, true);
            }
            return null;
        }

        #endregion ReadSetting

        #region CheckSetting

        public bool CheckSetting(string SettingTitle)
        {
            if (CheckValues())
            {
                List<string> Settings = LoadSettings();

                return BaseCommands.BaseCheckSetting(SettingTitle, Settings, true);
            }
            return false;
        }

        #endregion CheckSetting

        #region DeleteSetting

        public void DeleteSetting(string SettingTitle)
        {
            if (CheckValues())
            {
                List<string> Settings = LoadSettings();

                BaseCommands.BaseDeleteSetting(SettingTitle, Settings);

                SaveSettings(Settings);
            }
        }

        #endregion DeleteSetting

        #endregion Public Methods

        #region Private Methods

        #region CheckValues

        private bool CheckValues()
        {
            if (settingsDirectory == null) return false;

            return true;
        }

        #endregion CheckValues

        #region UpdateDirectory

        public void UpdateDirectory()
        {
            if (CheckValues())
            {
                //Initializes the full directory and makes sure the settings file exist's
                if (settingsFileName == null) settingsFileName = "MSMSettings.ini";
                settingsFullDirectory = settingsDirectory + @"\" + settingsFileName;
                if (!Directory.Exists(settingsDirectory)) Directory.CreateDirectory(settingsDirectory);
                if (!File.Exists(settingsFullDirectory)) File.Create(settingsFullDirectory).Close();
            }
        }

        #endregion UpdateDirectory

        #region Get Mac Address

        private string GetClientMacAddress()
        { return NetworkInterface.GetAllNetworkInterfaces().Where(nic => nic.OperationalStatus == OperationalStatus.Up && nic.NetworkInterfaceType != NetworkInterfaceType.Loopback).Select(nic => nic.GetPhysicalAddress().ToString()).FirstOrDefault(); }

        #endregion Get Mac Address

        #region SaveSettings

        private void SaveSettings(List<string> Settings)
        {
            string FullSettings = string.Join(Environment.NewLine, Settings);
            string saveValue = Encrypt(FullSettings, GetClientMacAddress());
            File.WriteAllText(settingsFullDirectory, saveValue);

            Settings = null;
            FullSettings = null;
            saveValue = null;

            GC.Collect();
            GC.WaitForPendingFinalizers();
        }

        #endregion SaveSettings

        #region LoadSettings

        private List<string> LoadSettings()
        {
            List<string> LoadedSettings = new List<string>();

            string loadedFileValue = File.ReadAllText(settingsFullDirectory);
            string decryptedLoadedValue = Decrypt(loadedFileValue, GetClientMacAddress());

            LoadedSettings = decryptedLoadedValue.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

            return LoadedSettings;
        }

        #endregion LoadSettings

        #region Encrypt

        private string Encrypt(string encryptString, string EncryptionKey)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    { cs.Write(clearBytes, 0, clearBytes.Length); cs.Close(); }

                    encryptString = Convert.ToBase64String(ms.ToArray());
                }
            }

            return encryptString;
        }

        #endregion Encrypt

        #region Decrypt

        private string Decrypt(string input, string EncryptionKey)
        {
            input = input.Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(input);
            using (Aes encryptor = Aes.Create())
            {
                Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(EncryptionKey, new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream ms = new MemoryStream())
                {
                    using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    { cs.Write(cipherBytes, 0, cipherBytes.Length); cs.Close(); }

                    input = Encoding.Unicode.GetString(ms.ToArray());
                }
            }

            return input;
        }

        #endregion Decrypt

        #endregion Private Methods
    }
}


