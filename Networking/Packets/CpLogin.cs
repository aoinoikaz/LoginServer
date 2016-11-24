using System;
using System.IO;
using System.Data.SqlClient;

using DivergentNetwork.Core.Debug;
using DivergentNetwork.Core.Networking.Packets;

using LoginServer.Services;

public sealed class CpLogin : ReceivePacket, IDisposable
{
    private string usr;
    private string pw;

    public override void Read()
    {
        usr = reader.ReadString();
        pw = reader.ReadString();
    }

    public override void Process()
    {
        AuthenticationService.Validate(LoginServer.Enums.AuthenticationType.Custom, connection, usr, pw);
    }

    public void Dispose()
    {
        Dispose();
    }


}

