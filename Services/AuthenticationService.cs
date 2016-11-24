using System;
using System.Data.SqlClient;

using DivergentNetwork.Core.Networking;
using DivergentNetwork.Core.Database;
using DivergentNetwork.Core.Debug;

using LoginServer.Managers;
using LoginServer.Enums;
using LoginServer.Networking;
using LoginServer.Networking.Packets;

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
                        Logger.Log(LogLevel.Warn, "Multiple login detected at account : {0}\n", key);
                        LoginConnection loggedInClient = ConnectionManager.Instance.GetMultipleLogin(key);

                        new SpLoginResponse(ServerResponseType.ACCOUNT_MULTI_LOG).Send(connection);

                        loggedInClient.Disconnect();
                    }
 
                    Logger.Log(LogLevel.Debug, "{0} from {1} has been authenticated: {2}", key, connection.EndPoint, type.ToString());

                    connection.IsAuthenticated = true;
                    connection.Username = key;
                    connection.ServerIdentifier = sID;

                    Logger.Log("Right here is where we'd send the authentication succeeded packet");
                    new SpLoginResponse(ServerResponseType.AUTHENTICATION_SUCCEEDED).Send(connection);
                    
                }
                else
                {
                    Logger.Log(LogLevel.Debug, "{0} from {1} has not been authenticated: {2}", key, connection.EndPoint, type.ToString());
                    connection.IsAuthenticated = false;

                    new SpLoginResponse(ServerResponseType.AUTHENTICATION_FAILED).Send(connection);
                }
            }
        }
    }
}
