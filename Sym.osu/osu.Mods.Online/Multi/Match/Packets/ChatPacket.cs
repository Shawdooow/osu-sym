using System;
using osu.Mods.Online.Base.Packets;

namespace osu.Mods.Online.Multi.Match.Packets
{
    [Serializable]
    public class ChatPacket : OnlinePacket
    {
        public string AuthorColor;

        public string Message;
    }
}
