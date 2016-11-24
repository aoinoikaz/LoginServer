using DivergentNetwork.Core.Debug;

namespace LoginServer.Config
{
    public sealed class LoginSettings
    {
        public static LoginSettings Instance { get; set; }
        public string IP { get; set; }
        public string TestIP { get; set; }
        public int Port { get; set; }

        public static void Create()
        {
            if((Instance = new LoginSettings()
            {
                IP = "198.27.103.148",
                TestIP = "127.0.0.1",
                Port = 3769
            }) != null) Logger.Log(LogLevel.Info, "Login server settings have been successfully configured.\n");
        }
    }
}
