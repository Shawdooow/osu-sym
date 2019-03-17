using System;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Player.Packets
{
    [Serializable]
    public class CursorPositionPacket : Packet
    {
        public long ID;
        public float X;
        public float Y;
    }
}

