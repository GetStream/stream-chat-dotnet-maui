namespace StreamChat.Libs.AppInfo
{
    public class ConsoleApplicationInfo : IApplicationInfo
    {
        public string Engine => "Unity";
        
        public string EngineVersion => System.Environment.Version.ToString();
        
        public string Platform => System.Runtime.InteropServices.RuntimeInformation.OSDescription;
        
        public string OperatingSystem => System.Runtime.InteropServices.RuntimeInformation.OSDescription;

        public int MemorySize => -1;
        
        public int GraphicsMemorySize => -1;

        public string ScreenSize => "not available";
    }
}