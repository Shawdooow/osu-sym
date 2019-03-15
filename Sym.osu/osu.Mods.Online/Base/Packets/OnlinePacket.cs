using System;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Base.Packets
{
    [Serializable]
    public class OnlinePacket : Packet
    {
        public override uint PacketSize => 2048;

        public OsuUserInfo User;
    }
}
