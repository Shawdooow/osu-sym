// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Classic.ClassicDifficulty.Preprocessing;
using osu.Game.Rulesets.Classic.ClassicDifficulty.Skills;
using osu.Game.Rulesets.Classic.Objects;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Classic.ClassicDifficulty
{
    public class ClassicDifficultyCalculator : DifficultyCalculator
    {
        public ClassicDifficultyCalculator(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes Calculate(IBeatmap beatmap, Mod[] mods, double timeRate)
        {
            return new DifficultyAttributes(mods, 0);
        }
    }
}
