using System;
using osu.Mods.Online.Base.Packets;

namespace osu.Mods.Online.Multi.Match.Packets
{
    [Serializable]
    public class SetMapPacket : OnlinePacket
    {
        public readonly Map Map;

        public SetMapPacket(Map map) => Map = map;
    }
}
