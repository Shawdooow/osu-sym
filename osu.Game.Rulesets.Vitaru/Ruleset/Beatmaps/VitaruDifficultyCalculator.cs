#region usings

using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Difficulty;
using osu.Game.Rulesets.Difficulty.Preprocessing;
using osu.Game.Rulesets.Difficulty.Skills;
using osu.Game.Rulesets.Mods;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Beatmaps
{
    /// <summary>
    /// Most of this is copied from OsuDifficultyCalculator ATM
    /// </summary>
    public class VitaruDifficultyCalculator : DifficultyCalculator
    {
        public VitaruDifficultyCalculator(Rulesets.Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected DifficultyAttributes Calculate(IBeatmap beatmap, Mod[] mods, double timeRate)
        {
            return new DifficultyAttributes(mods, 0);
        }

        protected override DifficultyAttributes CreateDifficultyAttributes(IBeatmap beatmap, Mod[] mods, Skill[] skills, double clockRate) => throw new System.NotImplementedException();

        protected override IEnumerable<DifficultyHitObject> CreateDifficultyHitObjects(IBeatmap beatmap, double clockRate) => throw new System.NotImplementedException();

        protected override Skill[] CreateSkills(IBeatmap beatmap) => throw new System.NotImplementedException();
    }
}
