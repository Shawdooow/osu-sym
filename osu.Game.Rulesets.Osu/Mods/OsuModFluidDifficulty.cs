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
        public override double ScoreMultiplier => 0;
        public override ModType Type => ModType.Special;
        public override FontAwesome Icon => FontAwesome.fa_upload;

        private double speedMulitplier = 1;
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
            speedMulitplier = 0.7 + scoreProcessor.Combo.Value * 0.007;
            clock.Rate = speedMulitplier;
        }

        public bool AllowFail => false;
    }
}
