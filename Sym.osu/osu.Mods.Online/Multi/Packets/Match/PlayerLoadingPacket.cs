using System;
using System.Collections.Generic;
using osu.Mods.Online.Base;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class PlayerLoadingPacket : Packet
    {
        public override uint PacketSize => Convert.ToUInt32(Users.Count > 0 ? Users.Count * 1024 : 1024);

        public List<OsuUserInfo> Users = new List<OsuUserInfo>();
    }
}
