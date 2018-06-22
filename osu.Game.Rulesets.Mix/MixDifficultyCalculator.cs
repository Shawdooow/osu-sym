using osu.Game.Beatmaps;
using System;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Mix
{
    public class MixDifficultyCalculator : DifficultyCalculator
    {
        public MixDifficultyCalculator(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes Calculate(IBeatmap beatmap, Mod[] mods, double timeRate)
        {
            return new DifficultyAttributes(mods, 0);
        }
    }
}
