using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoonbyteSettingsManager
{
    public class MSMCore
    {
        #region Vars

        private bool showLog;
        private List<string> settings = null;
        private string settingsDirectory = null;
        private string settingsFullDirectory = null;
        private string settingsFileName = null;

        #endregion

        #region Events

        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeEditSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeReadSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeCheckSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeDeleteSetting;
        public event EventHandler<OnBeforeMoonbyteCommandsEventArgs> OnBeforeSaveSettings;

        public event EventHandler<EventArgs> OnAfterEditSetting;
        public event EventHandler<EventArgs> OnAfterReadSetting;
        public event EventHandler<EventArgs> OnAfterCheckSetting;
        public event EventHandler<EventArgs> OnAfterDeleteSetting;
        public event EventHandler<EventArgs> OnAfterSaveSettings;

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

                settings = File.ReadAllLines(settingsFullDirectory).ToList();
            }
        }

        #endregion UpdateDirectory

        #region Public Methods

        #region SaveSettings

        public void SaveSettings()
        {
            OnBeforeMoonbyteCommandsEventArgs onBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeSaveSettings?.Invoke(this, onBeforeRequest);

            if (onBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            { 
                File.WriteAllLines(settingsFullDirectory, settings); 
            }

            OnAfterSaveSettings?.Invoke(this, new EventArgs());
        }

        #endregion SaveSettings

        #region EditSetting

        public void EditSetting(string SettingTitle, string SettingValue)
        {
            OnBeforeMoonbyteCommandsEventArgs onBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeEditSetting?.Invoke(this, onBeforeRequest);

            if (onBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues()) BaseCommands.BaseEditSetting(SettingTitle, SettingValue, settings); 
            }

            OnAfterEditSetting?.Invoke(this, new EventArgs());
        }

        #endregion EditSetting

        #region ReadSetting

        public string ReadSetting(string SettingTitle)
        {
            OnBeforeMoonbyteCommandsEventArgs onBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeReadSetting?.Invoke(this, onBeforeRequest);

            if (onBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues()) 
                {
                    OnAfterReadSetting?.Invoke(this, new EventArgs());
                    return BaseCommands.BaseReadSetting(SettingTitle, settings); 
                }
            }

            OnAfterReadSetting?.Invoke(this, new EventArgs());
            return null;
        }

        #endregion ReadSetting

        #region CheckSetting

        public bool CheckSetting(string SettingTitle)
        {
            OnBeforeMoonbyteCommandsEventArgs onBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeCheckSetting?.Invoke(this, onBeforeRequest);

            if (onBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues())
                {
                    OnAfterReadSetting?.Invoke(this, new EventArgs());
                    return BaseCommands.BaseCheckSetting(SettingTitle, settings);
                }
            }

            OnAfterCheckSetting?.Invoke(this, new EventArgs());

            return false;
        }

        #endregion CheckSetting

        #region DeleteSetting

        public void DeleteSetting(string SettingTitle)
        {
            OnBeforeMoonbyteCommandsEventArgs onBeforeRequest = new OnBeforeMoonbyteCommandsEventArgs() { SettingDirectory = this.SettingsDirectory };
            OnBeforeDeleteSetting?.Invoke(this, onBeforeRequest);

            if (onBeforeRequest.CancelRequest == BaseCommands.MoonbyteCancelRequest.Continue)
            {
                if (CheckValues()) BaseCommands.BaseDeleteSetting(SettingTitle, settings); 
            }

            OnAfterDeleteSetting?.Invoke(this, new EventArgs());
        }

        #endregion DeleteSetting

        #endregion Public Methods
    }
}
