namespace LoginServer.Networking.Packets
{
    public enum ServerResponseType : ushort
    {
        AUTHENTICATION_SUCCEEDED = 0x0,
        AUTHENTICATION_FAILED = 0x1,
        ACCOUNT_MULTI_LOG = 0x2
    }
}