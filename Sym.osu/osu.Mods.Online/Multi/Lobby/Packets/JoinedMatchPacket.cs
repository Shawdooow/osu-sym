#region usings

using System;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class JoinedMatchPacket : Packet
    {
        public override int PacketSize => MatchInfo.Users.Count > 0 ? MatchInfo.Users.Count * 1024 + 1024 : 2048;

        public MatchInfo MatchInfo;
    }
}
