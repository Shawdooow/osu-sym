using System;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class PlayerLoadingPacket : Packet
    {
        public override uint PacketSize => Convert.ToUInt32(Match.Users.Count > 0 ? Match.Users.Count * 1024 : 1024);

        public MatchInfo Match;
    }
}
