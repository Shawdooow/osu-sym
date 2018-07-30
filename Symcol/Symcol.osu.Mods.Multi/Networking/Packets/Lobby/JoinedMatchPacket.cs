using System;
using System.Collections.Generic;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Lobby
{
    [Serializable]
    public class JoinedMatchPacket : Packet
    {
        public override int PacketSize => Players.Count > 0 ? Players.Count * 1024 : 512;

        public List<OsuClientInfo> Players = new List<OsuClientInfo>();
    }
}
