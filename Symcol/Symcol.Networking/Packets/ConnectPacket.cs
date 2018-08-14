using System;

namespace Symcol.Networking.Packets
{
    [Serializable]
    public class ConnectPacket : Packet
    {
        public override int PacketSize => 256;

        public string Gamekey;
    }
}
