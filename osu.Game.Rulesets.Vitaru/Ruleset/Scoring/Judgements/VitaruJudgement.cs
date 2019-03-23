#region usings

using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements
{
    public class VitaruJudgement : Judgement
    {
        public override HitResult MaxResult => HitResult.Great;

        /// <summary>
        /// The positional hit offset.
        /// </summary>
        public Vector2 PositionOffset;

        public override bool IsBonus => BonusScore;

        public bool BonusScore { get; set; }

        public double Weight;

        protected override int NumericResultFor(HitResult result)
        {
            if (!BonusScore) return (int)Weight;

            switch (result)
            {
                default:
                    return 0;
                case HitResult.Meh:
                    Weight = BonusScore ? 50 : 1;
                    return BonusScore ? 50 : 1;
                case HitResult.Good:
                    Weight = 100;
                    return 100;
                case HitResult.Great:
                    Weight = 300;
                    return 300;
            }
        }
     }
}
