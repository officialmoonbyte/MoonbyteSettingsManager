namespace MoonbyteSettingsManager
{
    public class MSM    
    {

        #region Vars

        private const string Sep = " : " ;

        private static MSMCore msm = new MSMCore();

        #endregion

        #region Properties

        public static bool ShowLog
        {
            get { return msm.ShowLog; }
            set { msm.ShowLog = value; }
        }

        public static string SettingsFileName
        {
            get { return MSM.SettingsFileName; }
            set { msm.SettingsFileName = value; msm.UpdateDirectory(); }
        }

        public static string SettingsDirectory
        {
            get { return MSM.SettingsDirectory; }
            set
            { msm.SettingsDirectory = value; msm.UpdateDirectory(); }
        }

        #endregion Properties

        #region Public Methods

        #region SaveSettings

        public static void SaveSettings()
        { msm.SaveSettings(); }

        #endregion SaveSettings

        #region EditSetting

        public static void EditSetting(string SettingTitle, string SettingValue)
        { msm.EditSetting(SettingTitle, SettingValue); }

        #endregion EditSetting

        #region ReadSetting

        public static string ReadSetting(string SettingTitle)
        { return msm.ReadSetting(SettingTitle); }

        #endregion ReadSetting

        #region CheckSetting

        public static bool CheckSetting(string SettingTitle)
        { return msm.CheckSetting(SettingTitle); }

        #endregion CheckSetting

        #region DeleteSetting

        public static void DeleteSetting(string SettingTitle)
        { msm.DeleteSetting(SettingTitle); }

        #endregion DeleteSetting

        #endregion Public Methods

    }
}
