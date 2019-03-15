using System;
using osu.Mods.Online.Base;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Lobby
{
    [Serializable]
    public class JoinMatchPacket : Packet
    {
        public override uint PacketSize => 2048;

        public OsuUserInfo User;

        public MatchInfo Match;
    }
}
