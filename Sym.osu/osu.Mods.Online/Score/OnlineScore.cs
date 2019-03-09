using System;

namespace osu.Mods.Online.Score
{
    [Serializable]
    public class OnlineScore
    {
        public int OnlineBeatmapSetID;

        public int OnlineBeatmapID;

        public string BeatmapTitle;

        public string BeatmapArtist;

        public string BeatmapMapper;

        public string BeatmapDifficulty;

        public string RulesetShortname;

        public double Score;

        public double Combo;

        public double Accuracy;

        public double PP;
    }
}
