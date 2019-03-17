using System;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class GetMatchListPacket : Packet
    {
        public override uint PacketSize => 256;
    }
}
