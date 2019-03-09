using System;
using osu.Mods.Online.Base.Packets;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class SetMapPacket : OnlinePacket
    {
        public int OnlineBeatmapSetID = -1;

        public int OnlineBeatmapID = -1;

        public string BeatmapTitle;

        public string BeatmapArtist;

        public string BeatmapMapper;

        public string BeatmapDifficulty;

        public int? RulesetID;
    }
}
