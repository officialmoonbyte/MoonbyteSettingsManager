using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace MoonbyteSettingsManager
{
    public class MSM    
    {

        #region Vars

        private const string Sep = " : " ;

        private static bool showLog;
        private static List<string> settings = null;
        private static string settingsDirectory = null;
        private static string settingsFullDirectory = null;
        private static string settingsFileName = null;

        #endregion

        #region Properties

        public static bool ShowLog
        {
            get { return showLog; }
            set { showLog = value; }
        }

        public static string SettingsFileName
        {
            get { return settingsFileName; }
            set { settingsFileName = value; UpdateDirectory(); }
        }

        public static string SettingsDirectory
        {
            get { return settingsDirectory; }
            set
            { settingsDirectory = value; UpdateDirectory(); }
        }

        #endregion Properties

        #region CheckValues

        private static bool CheckValues()
        {
            if (settingsDirectory == null) return false;

            return true;
        }

        #endregion CheckValues

        #region UpdateDirectory

        private static void UpdateDirectory()
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

        public static void SaveSettings()
        { File.WriteAllLines(settingsFullDirectory, settings); }

        #endregion SaveSettings

        #region EditSetting

        public static void EditSetting(string SettingTitle, string SettingValue)
        {
            if (CheckValues())
            {
                string newString = SettingTitle + Sep + SettingValue;
                int i = 0; bool found = false; foreach(string s in settings)
                {
                    string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                    if (rawSetting[0] == SettingTitle)
                    {
                        settings[i] = newString;
                        found = true;
                        break;
                    }
                    i++;
                }

                if (found == false) { settings.Add(newString); }
            }
        }

        #endregion EditSetting

        #region ReadSetting

        public static string ReadSetting(string SettingTitle)
        {
            if (CheckValues())
            {
                foreach (string s in settings)
                {
                    string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                    if (rawSetting[0] == SettingTitle)
                    {
                        return rawSetting[1];
                    }
                }
            }

            return null;
        }

        #endregion ReadSetting

        #region CheckSetting

        public static bool CheckSetting(string SettingTitle)
        {
            if (CheckValues())
            {
                foreach (string s in settings)
                {
                    string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                    if (rawSetting[0] == SettingTitle)
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        #endregion CheckSetting

        #region DeleteSetting

        public static void DeleteSetting(string SettingTitle)
        {
            if (CheckValues())
            {
                string oldString = SettingTitle + Sep;
                int i = 0; bool found = false; foreach (string s in settings)
                {
                    string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                    if (rawSetting[0] == SettingTitle)
                    {
                        oldString += rawSetting[1];
                        settings.Remove(oldString);
                        break;
                    }
                    i++;
                }
            }
        }

        #endregion DeleteSetting

        #endregion Public Methods

    }
}
