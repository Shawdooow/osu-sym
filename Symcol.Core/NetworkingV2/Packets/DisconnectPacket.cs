using System;

namespace Symcol.Core.NetworkingV2.Packets
{
    [Serializable]
    public class DisconnectPacket : Packet
    {
        public override int PacketSize => 128;
    }
}
