#region usings

using System;
using osu.Mods.Online.Base;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Lobby.Packets
{
    [Serializable]
    public class OsuConnectPacket : ConnectPacket
    {
        public override int PacketSize => 1024;

        public OsuUser User;
    }
}
