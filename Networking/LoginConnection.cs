using System;
using System.Net.Sockets;

using DivergentNetwork.Core.Debug;
using DivergentNetwork.Core.Networking;
using DivergentNetwork.Core.Networking.Events;
using DivergentNetwork.Core.Networking.Packets;
using DivergentNetwork.Core.Networking.Operations;

using LoginServer.Managers;

namespace LoginServer.Networking
{
    public sealed class LoginConnection : Client
    {
        public LoginConnection(Socket socket) : base(socket)
        {
            // Subscribe this client to the the OnReceivePacket and OnDisconnect events
            OnPacket += new EventHandler<PacketReceivedEventArgs>(Connection_OnPacket);
            OnDisconnect += new EventHandler<SessionClosedEventArgs>(Connection_OnDisconnect);
        }


        void Connection_OnDisconnect(object sender, SessionClosedEventArgs e)
        {
            Logger.Log(LogLevel.Info, "{0} has disconnected: {1}", EndPoint, e.Reason);
            ConnectionManager.Instance.RemoveConnection(this);
        }


        // This function will handle incoming login packets from clients
        void Connection_OnPacket(object sender, PacketReceivedEventArgs e)
        {
            try
            {
                // Ensure the client sent a packet that we actually initialized
                if (OperationCodes.ReceivePacket.ContainsKey(e.OpCode))
                {
                    // Reconstruct the packet that the client sent and process it
                    ((ReceivePacket)Activator.CreateInstance(OperationCodes.ReceivePacket[e.OpCode])).Process(this, e.Buffer);
                }
                else
                {
                    // Default divergenet network operation code
                    if (e.OpCode == 0x0001)
                    {
                        ((ReceivePacket)Activator.CreateInstance(OperationCodes.ReceivePacket[e.OpCode])).Process(this, e.Buffer);
                    }
                    else
                    {
                        byte header = (byte)(e.OpCode >> 10);
                        byte type = (byte)(e.OpCode & 1023);

                        Logger.Log(LogLevel.Debug, "Unhandled packet: {0}|{1} Opcode: 0x{2:X4}", header, type, e.OpCode);
                    }  
                }
            }
            catch (Exception ex)
            {
                Logger.Log(LogLevel.Exception, "Exception when handling incoming packet: {0}", ex.ToString());
            }
        }
    }
}
