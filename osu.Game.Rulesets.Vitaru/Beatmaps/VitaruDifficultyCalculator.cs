using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Difficulty;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    /// <summary>
    /// Most of this is copied from OsuDifficultyCalculator ATM
    /// </summary>
    public class VitaruDifficultyCalculator : DifficultyCalculator
    {
        public VitaruDifficultyCalculator(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        protected override DifficultyAttributes Calculate(IBeatmap beatmap, Mod[] mods, double timeRate)
        {
            return new DifficultyAttributes(mods, 0);
        }
    }
}
