using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using OpenTK;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableVitaruJudgement : DrawableJudgement
    {
        public DrawableVitaruJudgement(JudgementResult result, DrawableHitObject judgedObject)
            : base(result, judgedObject)
        {
        }

        protected override void LoadComplete()
        {
            if (Result.Type != HitResult.Miss)
                JudgementText?.TransformSpacingTo(new Vector2(14, 0), 1800, Easing.OutQuint);

            base.LoadComplete();
        }
    }
}
