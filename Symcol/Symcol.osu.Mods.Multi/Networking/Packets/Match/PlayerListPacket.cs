using System;
using System.Collections.Generic;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class PlayerListPacket : Packet
    {
        public override int PacketSize => Players.Count > 0 ? Players.Count * 1024 : 512;

        public List<OsuClientInfo> Players = new List<OsuClientInfo>();
    }
}
