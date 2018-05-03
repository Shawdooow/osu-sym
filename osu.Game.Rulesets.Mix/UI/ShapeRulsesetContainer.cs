using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Mix.Judgements;
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
using System;

namespace osu.Game.Rulesets.Mix
{
    internal class ShapeRulesetContainer : RulesetContainer<MixHitObject>
    {
        public ShapeRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap, bool isForCurrentRuleset)
            : base(ruleset, beatmap, isForCurrentRuleset)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new MixScoreProcessor(this);

        protected override BeatmapConverter<MixHitObject> CreateBeatmapConverter() => new MixBeatmapConverter();

        protected override BeatmapProcessor<MixHitObject> CreateBeatmapProcessor() => new MixBeatmapProcessor();

        protected override Playfield CreatePlayfield() => new ShapePlayfield();

        public override PassThroughInputManager CreateInputManager() => new MixInputManager(Ruleset.RulesetInfo);

        protected override DrawableHitObject<MixHitObject> GetVisualRepresentation(MixHitObject h)
        {
            var shape = h as BaseShape;
            if (shape != null)
                return new DrawableBaseShape(shape);
            return null;
        }

        protected override Vector2 GetAspectAdjustedSize() => new Vector2(0.75f);
    }
}
