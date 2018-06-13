using System;

namespace Symcol.Core.NetworkingV2.Packets
{
    [Serializable]
    public sealed class TestPacket : Packet
    {
        public override int PacketSize => 128;
    }
}
