using System;
using System.Collections.Generic;
using Symcol.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class MatchLoadingPacket : Packet
    {
        public override int PacketSize => Players.Count > 0 ? Players.Count * 1024 : 1024;

        public List<OsuClientInfo> Players = new List<OsuClientInfo>();
    }
}
