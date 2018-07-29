using System;

namespace Symcol.Core.Networking.Packets
{
    [Serializable]
    public class ConnectPacket : Packet
    {
        public override int PacketSize => 256;

        public string Gamekey;
    }
}
