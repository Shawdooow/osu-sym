using System;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class ChatPacket : MatchPacket
    {
        public string AuthorColor;

        public string Message;
    }
}
