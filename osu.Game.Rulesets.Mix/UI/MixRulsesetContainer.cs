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
using osu.Framework.Graphics;

namespace osu.Game.Rulesets.Mix
{
    internal class MixRulesetContainer : RulesetContainer<MixHitObject>
    {
        public MixRulesetContainer(Ruleset ruleset, WorkingBeatmap beatmap)
            : base(ruleset, beatmap)
        {
        }

        public override ScoreProcessor CreateScoreProcessor() => new MixScoreProcessor(this);

        protected override Playfield CreatePlayfield() => new MixPlayfield
        {
            Anchor = Anchor.CentreLeft,
            Origin = Anchor.CentreLeft
        };

        public override PassThroughInputManager CreateInputManager() => new MixInputManager(Ruleset.RulesetInfo);

        protected override DrawableHitObject<MixHitObject> GetVisualRepresentation(MixHitObject h)
        {
            if (h is MixNote note)
                return new DrawableMixNote(note);
            return null;
        }

        protected override Vector2 GetAspectAdjustedSize()
        {
            const float default_relative_height = MixPlayfield.DEFAULT_HEIGHT / 768;
            const float default_aspect = 16f / 9f;

            float aspectAdjust = MathHelper.Clamp(DrawWidth / DrawHeight, 0.4f, 4) / default_aspect;

            return new Vector2(1, default_relative_height * aspectAdjust);
        }
    }
}
