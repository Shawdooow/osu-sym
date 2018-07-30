using System;
using System.Collections.Generic;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Lobby
{
    [Serializable]
    public class JoinedMatchPacket : Packet
    {
        public override int PacketSize => Clients.Count > 0 ? Clients.Count * 1024 : 512;

        public List<OsuClientInfo> Clients = new List<OsuClientInfo>();
    }
}
