using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osuTK;
// ReSharper disable SpecifyACultureInStringConversionExplicitly

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables
{
    public class DrawableVitaruJudgement : DrawableJudgement
    {
        private readonly VitaruJudgementResult vitaruJudgementResult;

        public DrawableVitaruJudgement(JudgementResult result, DrawableHitObject judgedObject)
            : base(result, judgedObject)
        {
            vitaruJudgementResult = (VitaruJudgementResult)result;
        }

        protected override void LoadComplete()
        {
            if (Result.Type != HitResult.Miss)
            {
                JudgementText?.TransformSpacingTo(new Vector2(14, 0), 1800, Easing.OutQuint);

                if (JudgementText != null)
                {
                    double value = Math.Round(vitaruJudgementResult.VitaruJudgement.Weight, 1);
                    JudgementText.Text = value == 0 ? "" : value.ToString();
                }
            }

            base.LoadComplete();
        }
    }
}
