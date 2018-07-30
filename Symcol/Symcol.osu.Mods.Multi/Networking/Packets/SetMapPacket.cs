using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets
{
    [Serializable]
    public class SetMapPacket : Packet
    {
        public int OnlineBeatmapSetID = -1;

        public int OnlineBeatmapID = -1;

        public string BeatmapName;

        public string Mapper;

        public string BeatmapDifficulty;
    }
}
