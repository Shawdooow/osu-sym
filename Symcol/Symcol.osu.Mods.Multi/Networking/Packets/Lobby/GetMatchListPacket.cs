using System;
using Symcol.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Lobby
{
    [Serializable]
    public class GetMatchListPacket : Packet
    {
        public override int PacketSize => 256;
    }
}
