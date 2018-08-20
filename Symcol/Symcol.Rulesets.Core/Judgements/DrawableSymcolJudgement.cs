using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;

namespace Symcol.Rulesets.Core.Judgements
{
    public class DrawableSymcolJudgement : DrawableJudgement
    {
        public readonly SymcolJudgement Judgement;

        public DrawableSymcolJudgement(JudgementResult result, DrawableHitObject judgedObject)
            : base(result, judgedObject)
        {
            Judgement = (SymcolJudgement)result.Judgement;
        }
    }
}
