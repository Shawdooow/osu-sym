using System;
using osu.Mods.Online.Base.Packets;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class SetMapPacket : OnlinePacket
    {
        public readonly Map Map;

        public SetMapPacket(Map map) => Map = map;
    }
}
