using System;
using System.Data.SqlClient;

using DivergentNetwork.Core.Networking;
using DivergentNetwork.Core.Database;
using DivergentNetwork.Core.Debug;

using LoginServer.Managers;
using LoginServer.Enums;
using LoginServer.Networking;
using LoginServer.Networking.Packets;
using DivergentNetwork.Core.Networking.Events;

namespace LoginServer.Services
{
    public class AuthenticationService
    {
        public static void Validate(AuthenticationType type, Client connection, string key, string id)
        {
            using (var dbConnection = DatabaseManager.GetConnection())
            {
                SqlCommand command = new SqlCommand();

                command.CommandText = 
                    type == AuthenticationType.Custom ? 
                    "SELECT sID FROM tAccounts WHERE sUsername = @key AND sPassword = @id" :  // CUSTOM QUERY
                    "SELECT sID FROM tAccounts WHERE sUsername = @key AND sUserId = @id"; // FB QUERY

                command.Parameters.AddWithValue("@key", key);
                command.Parameters.AddWithValue("@id", id);
                command.Connection = dbConnection;

                int sID = Convert.ToInt32(command.ExecuteScalar());

                // If the account exists
                if (sID > 0)
                {
                    // Check if more than 1 user is logged into the account    
                    if (ConnectionManager.Instance.IsLoggedIn(key))
                    {
                        Logger.Log(LogLevel.Info, "Multiple login detected at account : {0}\n", key);
                        LoginConnection loggedInClient = ConnectionManager.Instance.GetMultipleLogin(key);

                        new SpLoginResponse(ServerResponseType.AuthenticationMultilog).Send(connection);

                        loggedInClient.Disconnect(DisconnectCause.LoggedOff);
                    }

                    Logger.Log(LogLevel.Info, "{0} authentication succeeded: {1} | {2}", type.ToString(), key, connection.EndPoint.ToString());

                    connection.IsAuthenticated = true;
                    connection.Username = key;
                    connection.ServerIdentifier = sID;

                    new SpLoginResponse(ServerResponseType.AuthenticationSuccess).Send(connection);
                }
                else
                {
                    Logger.Log(LogLevel.Info, "{0} authentication failed: {1} | {2}", type.ToString(), key, connection.EndPoint.ToString());
                    connection.IsAuthenticated = false;

                    new SpLoginResponse(ServerResponseType.AuthenticationFailed).Send(connection);
                }
            }
        }
    }
}
