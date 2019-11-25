using MoonbyteSettingsManager;
using System;

namespace MSM_Testing_Application
{
    class Program
    {
        static void Main(string[] args)
        {
            //Initializes MSM
            MSM.SettingsDirectory = Environment.CurrentDirectory;

            while (true)
            {
                string consoleRead = Console.ReadLine();

                string[] consoleArgs = consoleRead.Split(' ');
                consoleArgs[0] = consoleArgs[0].ToUpper();

                if (consoleArgs[0] == "SAVESETTINGS")
                { MSM.SaveSettings(); }
                if (consoleArgs[0] == "EDITSETTING")
                { MSM.EditSetting(consoleArgs[1], consoleArgs[2]); }
                if (consoleArgs[0] == "DELETESETTING")
                { MSM.DeleteSetting(consoleArgs[1]); }
                if (consoleArgs[0] == "READSETTING")
                { Console.WriteLine("Returned value : " + MSM.ReadSetting(consoleArgs[1])); }
                if (consoleArgs[0] == "CHECKSETTING")
                { Console.WriteLine("Returned Value : " + MSM.CheckSetting(consoleArgs[1])); }
                if (consoleArgs[0] == "HELP")
                {
                    Console.WriteLine("Showing help - Help displays all of the public methods in MSM");
                    Console.WriteLine("There are currently 5 commands. All of these commands you can use in your project code");
                    Console.WriteLine(" ");
                    Console.WriteLine("EditSetting [SettingTitle] [SettingValue] - Edits a setting in the settings array.");
                    Console.WriteLine("ReadSetting [SettingTitle] - Returns the value in that setting.");
                    Console.WriteLine("CheckSetting [SettingTitle] - Returns a bool value if the setting exists or not.");
                    Console.WriteLine("DeleteSetting [SettingTitle] - Deletes that setting from the settings array.");
                    Console.WriteLine("SaveSettings [SettingTitle] - Saves the settings array to a file.");
                }
            }
        }
    }
}
