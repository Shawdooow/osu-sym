#region usings

using System;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class GetMatchListPacket : Packet
    {
        public override uint PacketSize => 256;
    }
}
