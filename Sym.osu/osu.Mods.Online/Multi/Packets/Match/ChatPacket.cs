using System;
using osu.Mods.Online.Base.Packets;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class ChatPacket : OnlinePacket
    {
        public string AuthorColor;

        public string Message;
    }
}
