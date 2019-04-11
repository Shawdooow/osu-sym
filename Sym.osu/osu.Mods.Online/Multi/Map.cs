#region usings

using System;

#endregion

namespace osu.Mods.Online.Multi
{
    [Serializable]
    public class Map
    {
        public int OnlineBeatmapSetID = -1;

        public int OnlineBeatmapID = -1;

        public string BeatmapTitle;

        public string BeatmapArtist;

        public string BeatmapMapper;

        public string BeatmapDifficulty;

        public double BeatmapStars;

        public string RulesetShortname;
    }
}
