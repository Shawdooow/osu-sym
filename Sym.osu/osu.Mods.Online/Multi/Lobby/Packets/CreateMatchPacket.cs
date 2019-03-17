using System;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class CreateMatchPacket : Packet
    {
        public override uint PacketSize => 2048;

        public MatchInfo MatchInfo;
    }
}
