using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Mix.Objects;
using osu.Game.Rulesets.Mix.Objects.Drawables;
using osu.Game.Rulesets.Mix.Beatmaps;
using osu.Game.Rulesets.Mix.UI;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using OpenTK;
using osu.Game.Rulesets.Mix.Scoring;
using osu.Framework.Input;

namespace osu.Game.Rulesets.Mix
{
    internal class MixRulesetContainer : RulesetContainer<MixHitObject>
    {
        public MixRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(ruleset, beatmap, isForCurrentRuleset)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new MixScoreProcessor(this);

        protected override BeatmapConverter<MixHitObject> CreateBeatmapConverter() => new MixBeatmapConverter();

        protected override BeatmapProcessor<MixHitObject> CreateBeatmapProcessor() => new MixBeatmapProcessor();

        protected override Playfield CreatePlayfield() => new MixPlayfield();

        public override PassThroughInputManager CreateInputManager() => new MixInputManager(Ruleset.RulesetInfo);

        protected override DrawableHitObject<MixHitObject> GetVisualRepresentation(MixHitObject h)
        {
            if (h is BaseShape shape)
                return new DrawableBaseShape(shape);
            return null;
        }

        protected override Vector2 GetAspectAdjustedSize() => new Vector2(0.75f);
    }
}
