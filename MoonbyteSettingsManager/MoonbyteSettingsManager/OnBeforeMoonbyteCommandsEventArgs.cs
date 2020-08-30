using static MoonbyteSettingsManager.BaseCommands;

namespace MoonbyteSettingsManager
{
    public class OnBeforeMoonbyteCommandsEventArgs
    {
        public MoonbyteCancelRequest CancelRequest = MoonbyteCancelRequest.Continue;
        public string SettingDirectory;
        public string ErrorMessage = "USER_PLUGINAUTH";
        public string RawData;
    }
}
