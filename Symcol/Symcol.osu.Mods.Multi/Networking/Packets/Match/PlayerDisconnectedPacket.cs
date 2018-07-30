using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class PlayerDisconnectedPacket : Packet
    {
        public OsuClientInfo Player;
    }
}
