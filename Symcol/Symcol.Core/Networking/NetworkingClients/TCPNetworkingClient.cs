﻿using Symcol.Core.Networking.Packets;

namespace Symcol.Core.Networking.NetworkingClients
{
    public class TCPNetworkingClient : NetworkingClient
    {
        public override int Avalable { get; }

        public override void SendPacket(Packet packet)
        {
            throw new System.NotImplementedException();
        }

        public override Packet GetPacket()
        {
            throw new System.NotImplementedException();
        }

        public override void SendBytes(byte[] bytes)
        {
            throw new System.NotImplementedException();
        }

        public override byte[] GetBytes()
        {
            throw new System.NotImplementedException();
        }

        public override void Dispose()
        {
            throw new System.NotImplementedException();
        }
    }
}