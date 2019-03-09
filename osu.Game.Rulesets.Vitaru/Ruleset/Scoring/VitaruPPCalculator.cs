using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Scoring;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Scoring
{
    public class VitaruPPCalculator : PerformanceCalculator
    {
        public const double PP_MULTIPLIER = 0.00002d;

        public VitaruPPCalculator(Rulesets.Ruleset ruleset, WorkingBeatmap beatmap, ScoreInfo score) : base(ruleset, beatmap, score)
        {
        }

        public override double Calculate(Dictionary<string, double> categoryRatings = null)
        {
            double pp = Score.TotalScore * PP_MULTIPLIER;
            return pp;
        }
    }
}
