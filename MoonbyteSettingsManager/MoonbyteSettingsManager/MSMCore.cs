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
        { File.WriteAllLines(settingsFullDirectory, settings); }

        #endregion SaveSettings

        #region EditSetting

        public void EditSetting(string SettingTitle, string SettingValue)
        {
            if (CheckValues())
            { BaseCommands.BaseEditSetting(SettingTitle, SettingValue, settings); }
        }

        #endregion EditSetting

        #region ReadSetting

        public string ReadSetting(string SettingTitle)
        {
            if (CheckValues())
            { return BaseCommands.BaseReadSetting(SettingTitle, settings); }

            return null;
        }

        #endregion ReadSetting

        #region CheckSetting

        public bool CheckSetting(string SettingTitle)
        {
            if (CheckValues())
            { return BaseCommands.BaseCheckSetting(SettingTitle, settings); }

            return false;
        }

        #endregion CheckSetting

        #region DeleteSetting

        public void DeleteSetting(string SettingTitle)
        {
            if (CheckValues())
            { BaseCommands.BaseDeleteSetting(SettingTitle, settings); }
        }

        #endregion DeleteSetting

        #endregion Public Methods
    }
}
