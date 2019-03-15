using System;
using System.Collections.Generic;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Lobby
{
    [Serializable]
    public class MatchListPacket : Packet
    {
        public override uint PacketSize => Convert.ToUInt32(MatchInfoList.Count > 0 ? MatchInfoList.Count * 1024 + 1024 : 2048);

        public List<MatchInfo> MatchInfoList = new List<MatchInfo>();
    }
}
