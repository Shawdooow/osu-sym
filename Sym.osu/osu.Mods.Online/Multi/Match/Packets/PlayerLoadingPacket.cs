#region usings

using System;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Match.Packets
{
    [Serializable]
    public class PlayerLoadingPacket : Packet
    {
        public override int PacketSize => Match.Users.Count > 0 ? Match.Users.Count * 1024 : 1024;

        public MatchInfo Match;
    }
}
