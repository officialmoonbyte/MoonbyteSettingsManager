using System;
using System.Collections.Generic;

namespace MoonbyteSettingsManager
{
    public class BaseCommands
    {
        private const string Sep = " : ";
        public enum MoonbyteCancelRequest { Cancel, Continue }

        public static void BaseDeleteSetting(string SettingTitle, List<string> Settings)
        {
            string oldString = SettingTitle + Sep;
            int i = 0; bool found = false; foreach (string s in Settings)
            {
                string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                if (rawSetting[0] == SettingTitle)
                {
                    oldString += rawSetting[1];
                    Settings.Remove(oldString);
                    break;
                }
                i++;
            }
        }

        public static void BaseEditSetting(string SettingTitle, string SettingValue, List<string> Settings)
        {
            string newString = SettingTitle + Sep + SettingValue;

            int i = 0; bool found = false; foreach (string s in Settings)
            {
                string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                try
                {
                    if (rawSetting[0] == SettingTitle)
                    {
                        Settings[i] = newString;
                        found = true;
                        break;
                    }
                    i++;
                }
                catch { }
            }

            if (found == false) { Settings.Add(newString); }
        }

        public static bool BaseCheckSetting(string SettingTitle, List<string> Settings, bool DeleteList = false)
        {
            bool returnBool = false;
            foreach (string s in Settings)
            {
                string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                if (rawSetting[0] == SettingTitle)
                {
                    returnBool = true;
                }
            }

            if (DeleteList) Settings = null;
            return returnBool;
        }

        public static string BaseReadSetting(string SettingTitle, List<string> Settings, bool DeleteList = false)
        {
            string returnString = null;
            foreach (string s in Settings)
            {
                string[] rawSetting = s.Split(new string[] { Sep }, StringSplitOptions.RemoveEmptyEntries);
                if (rawSetting[0] == SettingTitle)
                {
                    returnString = rawSetting[1];
                }
            }

            if (DeleteList) Settings = null;
            return returnString;
        }
    }
}
