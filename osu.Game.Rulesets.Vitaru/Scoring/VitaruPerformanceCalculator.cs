using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Vitaru.Scoring
{
    public class VitaruPerformanceCalculator : PerformanceCalculator
    {
        private const float pp_multiplier = 1f;

        public static float CurrentPPValue = 0;
        public static float MaxPPValue = 0;

        public VitaruPerformanceCalculator(Ruleset ruleset, WorkingBeatmap beatmap, Score score) : base(ruleset, beatmap, score)
        {
            CurrentPPValue = 0;
            MaxPPValue = 0;
        }

        public override double Calculate(Dictionary<string, double> categoryRatings = null)
        {
            return 0 * Score.TotalScore * pp_multiplier;
        }
    }
}
