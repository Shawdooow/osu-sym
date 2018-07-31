using System;
using Symcol.Core.Networking.Packets;

namespace Symcol.osu.Mods.Multi.Networking.Packets.Match
{
    [Serializable]
    public class SetMapPacket : Packet
    {
        public OsuClientInfo Player;

        public int OnlineBeatmapSetID = -1;

        public int OnlineBeatmapID = -1;

        public string BeatmapTitle;

        public string BeatmapArtist;

        public string BeatmapMapper;

        public string BeatmapDifficulty;
    }
}
