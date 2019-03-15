using System;
using System.Collections.Generic;
using osu.Mods.Online.Base;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class PlayerListPacket : Packet
    {
        public override uint PacketSize => Convert.ToUInt32(Players.Count > 0 ? Players.Count * 1024 + 1024 : 1024);

        public List<OsuUserInfo> Players = new List<OsuUserInfo>();
    }
}
