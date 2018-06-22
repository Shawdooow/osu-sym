using osu.Framework.Input;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Shape.Objects;
using osu.Game.Rulesets.Shape.Objects.Drawables;
using osu.Game.Rulesets.Shape.Scoring;
using osu.Game.Rulesets.UI;
using OpenTK;

namespace osu.Game.Rulesets.Shape.UI
{
    internal class ShapeRulesetContainer : RulesetContainer<ShapeHitObject>
    {
        public ShapeRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new ShapeScoreProcessor(this);

        protected override Playfield CreatePlayfield() => new ShapePlayfield();

        public override PassThroughInputManager CreateInputManager() => new ShapeInputManager(Ruleset.RulesetInfo);

        protected override DrawableHitObject<ShapeHitObject> GetVisualRepresentation(ShapeHitObject h)
        {
            var shape = h as BaseShape;
            if (shape != null)
                return new DrawableBaseShape(shape);
            return null;
        }

        protected override Vector2 GetAspectAdjustedSize() => new Vector2(0.75f);
    }
}
