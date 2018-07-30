using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets
{
    public class MatchCreatedPacket : Packet
    {
        public override int PacketSize => 512;

        public MatchListPacket.MatchInfo MatchInfo;
    }
}
