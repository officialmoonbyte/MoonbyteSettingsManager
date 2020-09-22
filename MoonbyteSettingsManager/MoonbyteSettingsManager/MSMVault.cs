using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;

namespace MoonbyteSettingsManager
{
    public class MSMVault
    {

        #region Vars

        private const string Sep = " : ";

        EncryptionKey encryptionKey;

        private bool showLog;
        private string settingsDirectory = null;
        private string settingsFullDirectory = null;
        private string settingsFileName = null;

        #endregion Vars

        #region Events

        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeDecryptMessage;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeEncryptMessage;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeLoadSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeSaveSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeDeleteSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeCheckSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeReadSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeEditSetting;

        public event EventHandler<EventArgs> OnAfterDecryptMessage;
        public event EventHandler<EventArgs> OnAfterEncryptMessage;
        public event EventHandler<EventArgs> OnAfterLoadSetting;
        public event EventHandler<EventArgs> OnAfterSaveSetting;
        public event EventHandler<EventArgs> OnAfterDeleteSetting;
        public event EventHandler<EventArgs> OnAfterCheckSetting;
        public event EventHandler<EventArgs> OnAfterReadSetting;
        public event EventHandler<EventArgs> OnAfterEditSetting;

        #endregion Events

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

        #region Initialization

        public MSMVault(string encryptionKeyFileDirectory) => encryptionKey = new EncryptionKey(encryptionKeyFileDirectory);

        #endregion Initialization

        #region Public Methods

        #region EditSetting

        public void EditSetting(string SettingTitle, string SettingValue)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeEditSetting?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues())
                {
                    List<string> Settings = LoadSettings();

                    BaseCommands.BaseEditSetting(SettingTitle, SettingValue, Settings);

                    SaveSettings(Settings);
                }
            }

            OnAfterEditSetting?.Invoke(this, new EventArgs());
        }

        #endregion EditSetting

        #region ReadSetting

        public string ReadSetting(string SettingTitle)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeReadSetting?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues())
                {
                    List<string> Settings = LoadSettings();

                    OnAfterReadSetting?.Invoke(this, new EventArgs());
                    return BaseCommands.BaseReadSetting(SettingTitle, Settings, true);
                }
            }

            OnAfterReadSetting?.Invoke(this, new EventArgs());
            return null;
        }

        #endregion ReadSetting

        #region CheckSetting

        public bool CheckSetting(string SettingTitle)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeCheckSetting?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues())
                {
                    List<string> Settings = LoadSettings();

                    OnAfterCheckSetting?.Invoke(this, new EventArgs());
                    return BaseCommands.BaseCheckSetting(SettingTitle, Settings, true);
                }
            }

            OnAfterCheckSetting?.Invoke(this, new EventArgs());
            return false;
        }

        #endregion CheckSetting

        #region DeleteSetting

        public void DeleteSetting(string SettingTitle)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeDeleteSetting?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues())
                {
                    List<string> Settings = LoadSettings();

                    BaseCommands.BaseDeleteSetting(SettingTitle, Settings);

                    SaveSettings(Settings);
                }
            }

            OnAfterDeleteSetting?.Invoke(this, new EventArgs());
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

        #region SaveSettings

        private void SaveSettings(List<string> Settings)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeSaveSetting?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                string FullSettings = string.Join(Environment.NewLine, Settings);
                string saveValue = Encrypt(FullSettings);
                File.WriteAllText(settingsFullDirectory, saveValue);

                Settings = null;
                FullSettings = null;
                saveValue = null;

                GC.Collect();
                GC.WaitForPendingFinalizers();
            }

            OnAfterSaveSetting?.Invoke(this, new EventArgs());
        }

        #endregion SaveSettings

        #region LoadSettings

        private List<string> LoadSettings()
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeLoadSetting?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                List<string> LoadedSettings = new List<string>();

                string loadedFileValue = File.ReadAllText(settingsFullDirectory);
                string decryptedLoadedValue = Decrypt(loadedFileValue);

                LoadedSettings = decryptedLoadedValue.Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.RemoveEmptyEntries).ToList();

                OnAfterLoadSetting?.Invoke(this, new EventArgs());
                return LoadedSettings;
            }

            OnAfterLoadSetting?.Invoke(this, new EventArgs());
            return null;
        }

        #endregion LoadSettings

        #region Encrypt

        private string Encrypt(string encryptString)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeEncryptMessage?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                byte[] clearBytes = Encoding.Unicode.GetBytes(encryptString);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey.GetEncryptionKey(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                        { cs.Write(clearBytes, 0, clearBytes.Length); cs.Close(); }

                        encryptString = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            OnAfterEncryptMessage?.Invoke(this, new EventArgs());
            return encryptString;
        }

        #endregion Encrypt

        #region Decrypt

        private string Decrypt(string input)
        {
            OnBeforeMoonbyteCommandsEventArgs OnBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeDecryptMessage?.Invoke(this, OnBeforeRequest);

            if (OnBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                input = input.Replace(" ", "+");
                byte[] cipherBytes = Convert.FromBase64String(input);
                using (Aes encryptor = Aes.Create())
                {
                    Rfc2898DeriveBytes pdb = new Rfc2898DeriveBytes(encryptionKey.GetEncryptionKey(), new byte[] { 0x49, 0x76, 0x61, 0x6e, 0x20, 0x4d, 0x65, 0x64, 0x76, 0x65, 0x64, 0x65, 0x76 });

                    encryptor.Key = pdb.GetBytes(32);
                    encryptor.IV = pdb.GetBytes(16);

                    using (MemoryStream ms = new MemoryStream())
                    {
                        using (CryptoStream cs = new CryptoStream(ms, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                        { cs.Write(cipherBytes, 0, cipherBytes.Length); cs.Close(); }

                        input = Encoding.Unicode.GetString(ms.ToArray());
                    }
                }
            }

            OnAfterDecryptMessage?.Invoke(this, new EventArgs());
            return input;
        }

        #endregion Decrypt

        #endregion Private Methods
    }
}


