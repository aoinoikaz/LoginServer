using System;
using System.Net.Sockets;

using DivergentNetwork.Core.Debug;
using DivergentNetwork.Core.Networking;

using LoginServer.Config;
using LoginServer.Managers;

namespace LoginServer.Networking
{
    public class LoginListener : Listener
    {
        public static LoginListener Instance;

        // We need to pass the port to our base listener class
        public LoginListener(int port) : base (port)
        {
            // Simply start listening for clients
            StartListening();

            Logger.Log(LogLevel.Info, "Login listener now accepting players on port: {0}.\n", port);
        }


        // Initializes the login server
        public static void Initialize()
        {
            try
            {
                Instance = new LoginListener(LoginSettings.Instance.Port);  
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Exception, ex.Message);
            }
        }


        public override void OnClientConnected(Socket socket)
        {
            Logger.Log(LogLevel.Debug, "Incoming player connection: {0}", socket.RemoteEndPoint.ToString());

            LoginConnection connection = new LoginConnection(socket);

            ConnectionManager.Instance.AddConnection(connection);
        }
    }
}
