using OpenTK;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;

namespace osu.Game.Rulesets.Vitaru.Judgements
{
    public class VitaruJudgement : Judgement
    {
        public override HitResult MaxResult => HitResult.Great;

        /// <summary>
        /// The positional hit offset.
        /// </summary>
        public Vector2 PositionOffset;

        public override bool AffectsCombo => !BonusScore;

        public bool BonusScore = false;

        protected override int NumericResultFor(HitResult result)
        {
            switch (result)
            {
                default:
                    return 0;
                case HitResult.Meh:
                    return 50;
                case HitResult.Good:
                    return 100;
                case HitResult.Great:
                    return 300;
            }
        }

        public ComboResult Combo;
    }
}
