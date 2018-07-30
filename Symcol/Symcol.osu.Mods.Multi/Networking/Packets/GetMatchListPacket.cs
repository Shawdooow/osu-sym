using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets
{
    [Serializable]
    public class GetMatchListPacket : Packet
    {
        public override int PacketSize => 256;
    }
}
