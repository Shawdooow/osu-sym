using System;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Lobby
{
    [Serializable]
    public class CreateMatchPacket : Packet
    {
        public override uint PacketSize => 2048;

        public MatchInfo MatchInfo;
    }
}
