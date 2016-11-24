using System;
using System.Collections.Generic;

using DivergentNetwork.Core.Debug;
using LoginServer.Networking;

namespace LoginServer.Managers
{
    public sealed class ConnectionManager
    {
        public static ConnectionManager Instance { get; private set; }

        private List<LoginConnection> connections = new List<LoginConnection>();

        public static void Initialize()
        {
            try
            {
                Instance = new ConnectionManager();
                Logger.Log(LogLevel.Info, "Connection manager initialized.\n");
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Exception, ex.Message);
            }
        }

        public void AddConnection(LoginConnection connection)
        {
            lock (connection)
            {
                connections.Add(connection);
            }
        }

        public bool RemoveConnection(LoginConnection connection)
        {
            lock (connections)
            {
                return connections.Remove(connection);
            }
        }

        public bool IsConnected(string ip)
        {
            lock (connections)
            {
                LoginConnection connection = connections.Find(c => c.EndPoint.Address.ToString() == ip);
                return (connection != null);
            }
        }


        public bool IsLoggedIn(string username)
        {
            lock (connections)
            {
                LoginConnection connection = connections.Find(c => c.Username == username);
                return (connection != null);
            }
        }
        

        public LoginConnection GetMultipleLogin(string username)
        {
            lock (connections)
            {
                return connections.Find(c => c.Username == username);
            }
        }
    }
}
