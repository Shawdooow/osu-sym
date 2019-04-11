#region usings

using System;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class MatchCreatedPacket : Packet
    {
        public override uint PacketSize => 1024;

        public MatchInfo MatchInfo;

        public bool Join;
    }
}
