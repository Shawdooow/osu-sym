using osu.Game.Beatmaps;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Mix.Objects;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Mix.Mods
{
    public class MixModNoFail : ModNoFail
    {

    }

    public class MixModEasy : ModEasy
    {

    }

    public class MixModHidden : ModHidden
    {
        public override string Description => @"Notes fade out";
        public override double ScoreMultiplier => 1.06;
    }

    public class MixModHardRock : ModHardRock
    {
        public override double ScoreMultiplier => 1.08;
        public override bool Ranked => true;
    }

    public class MixModSuddenDeath : ModSuddenDeath
    {
        public override string Description => "Don't Miss";
        public override bool Ranked => true;
    }

    public class MixModDaycore : ModDaycore
    {
        public override double ScoreMultiplier => 0.3;
    }

    public class MixModDoubleTime : ModDoubleTime
    {
        public override double ScoreMultiplier => 1.16;
    }

    public class MixModHalfTime : ModHalfTime
    {
        public override double ScoreMultiplier => 0.3;
    }

    public class MixModNightcore : ModNightcore
    {
        public override double ScoreMultiplier => 1.16;
    }

    public class MixModFlashlight : ModFlashlight
    {
        public override string Description => @"I don't even know how to play with this";
        public override double ScoreMultiplier => 1.18;
    }

    public class MixRelax : ModRelax
    {
        public override bool Ranked => false;
    }

    public class MixModAutoplay : ModAutoplay<MixHitObject>
    {
        protected override Score CreateReplayScore(Beatmap<MixHitObject> beatmap) => new Score
        {

        };
    }
}
