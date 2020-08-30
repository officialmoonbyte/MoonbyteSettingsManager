using System;

namespace MoonbyteSettingsManager
{
    public class MSM    
    {

        #region Vars

        private const string Sep = " : " ;

        private static MSMCore msm = new MSMCore();

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

        #region Initialization

        public MSM()
        {
            msm.OnBeforeEditSetting += this.OnBeforeEditSetting;
            msm.OnBeforeReadSetting += this.OnBeforeReadSetting;
            msm.OnBeforeCheckSetting += this.OnBeforeCheckSetting;
            msm.OnBeforeDeleteSetting += this.OnBeforeDeleteSetting;
            msm.OnBeforeSaveSettings += this.OnBeforeSaveSettings;
            msm.OnAfterEditSetting += this.OnAfterEditSetting;
            msm.OnAfterReadSetting += this.OnAfterReadSetting;
            msm.OnAfterCheckSetting += this.OnAfterCheckSetting;
            msm.OnAfterDeleteSetting += this.OnAfterDeleteSetting;
            msm.OnAfterSaveSettings += this.OnAfterSaveSettings;
        }

        #endregion Initialization

        #region Public Methods

        public static void SaveSettings() => msm.SaveSettings();
        public static void EditSetting(string SettingTitle, string SettingValue) => msm.EditSetting(SettingTitle, SettingValue);
        public static string ReadSetting(string SettingTitle) => msm.ReadSetting(SettingTitle); 
        public static bool CheckSetting(string SettingTitle) => msm.CheckSetting(SettingTitle);
        public static void DeleteSetting(string SettingTitle) => msm.DeleteSetting(SettingTitle);


        #endregion Public Methods

    }
}
