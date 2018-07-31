using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class MatchPacket : Packet
    {
        public override int PacketSize => 1024;

        public OsuClientInfo Player;
    }
}
