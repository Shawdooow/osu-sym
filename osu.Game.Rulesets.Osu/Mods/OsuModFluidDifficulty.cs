using System;
using osu.Framework.Timing;
using osu.Game.Graphics;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Osu.Mods
{
    public class OsuModFluidDifficulty : Mod, IApplicableToClock, IApplicableToScoreProcessor, IApplicableFailOverride
    {
        public override string Name => "Fluid Difficutly";
        public override string ShortenedName => "FluidDiff";
        public override double ScoreMultiplier => 1.2;
        public override ModType Type => ModType.Special;
        public override FontAwesome Icon => FontAwesome.fa_upload;

        private IAdjustableClock clock;

        public void ApplyToClock(IAdjustableClock clock)
        {
            this.clock = clock;
        }

        public void ApplyToScoreProcessor(ScoreProcessor scoreProcessor)
        {
            scoreProcessor.Health.ValueChanged += v => updateClock(scoreProcessor);
            scoreProcessor.Combo.ValueChanged += v => updateClock(scoreProcessor);
        }

        private void updateClock(ScoreProcessor scoreProcessor)
        {
            double scoreMultplier = Math.Min(scoreProcessor.Combo.Value, 100) / 100d;
            double accuracyMultiplier = Math.Min(scoreProcessor.Accuracy.Value, 0.95d) / 0.95d;

            double difficultyMultiplier = (scoreMultplier + accuracyMultiplier) / 2;

            clock.Rate = 0.5 + difficultyMultiplier * 1.5;
        }

        public bool AllowFail => false;
    }
}
