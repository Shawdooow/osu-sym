using System;
using Symcol.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Lobby
{
    [Serializable]
    public class MatchCreatedPacket : Packet
    {
        public override int PacketSize => 1024;

        public MatchListPacket.MatchInfo MatchInfo;
    }
}
