#region usings

using System;
using osu.Mods.Online.Base;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class JoinMatchPacket : Packet
    {
        public override uint PacketSize => 2048;

        public OsuUserInfo User;

        public MatchInfo Match;
    }
}
