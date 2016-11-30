using System;

using DivergentNetwork.Core.Debug;
using DivergentNetwork.Core.Networking.Packets;

using LoginServer.Services;
using LoginServer.Enums;

public sealed class CpLogin : ReceivePacket, IDisposable
{
    private string usr;
    private string pw;

    public override void Read()
    {
        usr = Reader.ReadString();
        pw = Reader.ReadString();
    }

    public override void Process()
    {
        try
        {
            AuthenticationService.Validate(AuthenticationType.Custom, Client, usr, pw);
        }
        catch (Exception e)
        {
            Logger.Log(LogLevel.Info, "Caught exception inside derived class when tryna process: {0}", e.Message);
        }
    }

    public void Dispose()
    {
        Dispose();
    }


}

