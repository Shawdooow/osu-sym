using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class ChatPacket : Packet
    {
        public string Author;

        public string AuthorColor;

        public string Message;
    }
}
