using System;

namespace Symcol.Core.Networking.Packets
{
    [Serializable]
    public class DisconnectPacket : Packet
    {
        public override int PacketSize => 128;
    }
}
