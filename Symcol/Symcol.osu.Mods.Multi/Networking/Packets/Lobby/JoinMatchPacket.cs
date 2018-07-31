using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Lobby
{
    [Serializable]
    public class JoinMatchPacket : Packet
    {
        public override int PacketSize => 2048;

        public OsuClientInfo OsuClientInfo;

        public MatchListPacket.MatchInfo Match;
    }
}
