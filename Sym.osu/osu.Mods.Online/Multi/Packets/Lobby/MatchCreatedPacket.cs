using System;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Lobby
{
    [Serializable]
    public class MatchCreatedPacket : Packet
    {
        public override uint PacketSize => 1024;

        public MatchListPacket.MatchInfo MatchInfo;
    }
}
