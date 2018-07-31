using System;
using Symcol.Core.Networking.Packets;
using Symcol.osu.Mods.Multi.Networking.Packets.Lobby;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class LeavePacket : Packet
    {
        public override int PacketSize => 2048;

        public MatchListPacket.MatchInfo Match;

        public OsuClientInfo Player;
    }
}
