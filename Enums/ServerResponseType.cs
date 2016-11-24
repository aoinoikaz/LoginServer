namespace LoginServer.Networking.Packets
{
    public enum ServerResponseType : ushort
    {
        AuthenticationSuccess = 0x0,
        AuthenticationFailed = 0x1,
        AuthenticationMultilog = 0x2,
    }
}