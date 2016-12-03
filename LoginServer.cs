using System;

using DivergentNetwork.Core.Debug;
using DivergentNetwork.Core.Database;
using DivergentNetwork.Core.Networking.Operations;

using LoginServer.Config;
using LoginServer.Managers;
using LoginServer.Networking;
using LoginServer.Networking.Packets;

namespace LoginServer
{
    public class LoginServer
    {
        static void Main(string[] args)
        {
            Logger.WriteConsoleTitle("Login Server Initialization");

            Logger.Log(LogLevel.Info, "Configuring login server settings...");
            LoginSettings.Create();

            Logger.Log(LogLevel.Info, "Configuring login listener to accept connections...");
            LoginListener.Initialize();

            Logger.Log(LogLevel.Info, "Configuring connection manager...");
            ConnectionManager.Initialize();

            Logger.Log(LogLevel.Info, "Configuring operation codes:");
            OperationCodes.AddOperationCode(OperationType.Receive, unchecked(0x0CA1), typeof(CpLogin));
            OperationCodes.AddOperationCode(OperationType.Send, unchecked(0x0CB2), typeof(SpLoginResponse));

            Logger.Log("");
            Logger.Log(LogLevel.Info, "Configuring database manager...");
            DatabaseManager.Configure();
            Logger.Log("");

            Logger.WriteConsoleTitle("Event Flow");

            Console.ReadLine();
            
        }
    }
}
