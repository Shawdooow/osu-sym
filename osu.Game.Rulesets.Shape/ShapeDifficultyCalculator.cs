using osu.Game.Beatmaps;
using osu.Game.Rulesets.Shape.Objects;
using System;
using System.Collections.Generic;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Shape
{
    public class ShapeDifficultyCalculator : DifficultyCalculator
    {
        public ShapeDifficultyCalculator(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes Calculate(IBeatmap beatmap, Mod[] mods, double timeRate)
        {
            return new DifficultyAttributes(mods, 0);
        }
    }
}
