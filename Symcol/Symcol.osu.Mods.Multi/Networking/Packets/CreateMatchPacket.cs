using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets
{
    [Serializable]
    public class CreateMatchPacket : Packet
    {
        public override int PacketSize => 1024;

        public MatchListPacket.MatchInfo MatchInfo;
    }
}
