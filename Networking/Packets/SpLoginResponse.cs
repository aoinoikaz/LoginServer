using System.IO;

using DivergentNetwork.Core.Networking.Packets;

namespace LoginServer.Networking.Packets
{
    public sealed class SpLoginResponse : SendPacket
    {
        private ServerResponseType type;

        public SpLoginResponse(ServerResponseType type)
        {
            this.type = type;
        }

        public override void Write(BinaryWriter writer)
        {
            switch(type)
            {
                case ServerResponseType.AUTHENTICATION_SUCCEEDED:
                    writer.Write(true);
                    break;
                case ServerResponseType.AUTHENTICATION_FAILED:
                    writer.Write(false);
                    break;
                case ServerResponseType.ACCOUNT_MULTI_LOG:
                    writer.Write(0x0C07);
                    break;

            }
        }
    }
}
