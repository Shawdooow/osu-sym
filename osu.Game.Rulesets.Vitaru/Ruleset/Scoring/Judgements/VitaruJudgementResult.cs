using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements
{
    public class VitaruJudgementResult : JudgementResult
    {
        public VitaruJudgement VitaruJudgement => (VitaruJudgement)Judgement;

        public new int ComboAtJudgement { get; internal set; }

        public new int HighestComboAtJudgement { get; internal set; }

        public VitaruJudgementResult(Judgement judgement)
            : base(judgement)
        {
        }
    }
}
