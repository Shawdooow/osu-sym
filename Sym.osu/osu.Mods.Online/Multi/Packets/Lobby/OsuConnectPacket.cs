using System;
using osu.Mods.Online.Base;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Packets.Lobby
{
    [Serializable]
    public class OsuConnectPacket : ConnectPacket
    {
        public override uint PacketSize => 1024;

        public OsuUserInfo User;
    }
}
