using System;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Player.Packets
{
    [Serializable]
    public class ScorePacket : Packet
    {
        public long ID;

        public int Score;
    }
}
