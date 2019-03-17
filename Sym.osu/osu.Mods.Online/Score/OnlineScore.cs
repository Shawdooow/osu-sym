using System;
using osu.Mods.Online.Multi;

namespace osu.Mods.Online.Score
{
    [Serializable]
    public class OnlineScore
    {
        public Map Map;

        public double Score;

        public double Combo;

        public double Accuracy;

        public double PP;
    }
}
