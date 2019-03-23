#region usings

using System;
using osu.Framework.Configuration;
using osu.Framework.Extensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osu.Game.Scoring;
using osu.Mods.Online.Score.Rulesets;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Scoring
{
    internal class VitaruScoreProcessor : OnlineScoreProcessor<VitaruHitObject>
    {
        private readonly VitaruRulesetContainer vitaruRulesetContainer;

        public static int ComboCount;

        private DebugStat<int> expectedJudgementCount;
        private readonly Bindable<double> pp = new Bindable<double>();
        private readonly Bindable<double> scoreMiss = new Bindable<double>();
        private readonly Bindable<double> scoreCombo = new Bindable<double>();

        protected override bool Ranked => !vitaruRulesetContainer.VitaruPlayfield.Cheated;

        public VitaruScoreProcessor(VitaruRulesetContainer vitaruRulesetContainer, VitaruPlayfield playfield) : base(vitaruRulesetContainer)
        {
            this.vitaruRulesetContainer = vitaruRulesetContainer;

            Mode.Disabled = false;
            Mode.Value = ScoringMode.Classic;
            Mode.Disabled = true;

            playfield.OnResult += AddResult;
            playfield.OnDispose += () => playfield.OnResult -= AddResult;
        }

        protected override void Reset(bool storeResults)
        {
            base.Reset(storeResults);

            ComboCount = 0;

            ScoreResultCounts.Clear();
        }

        protected override JudgementResult CreateResult(Judgement judgement) => new VitaruJudgementResult(judgement);

        protected override void SimulateAutoplay(Beatmap<VitaruHitObject> beatmap)
        {
            DebugToolkit.ScoreDebugItems.Add(new DebugStat<double>(pp) { Text = "PP" });
            DebugToolkit.ScoreDebugItems.Add(new DebugStat<double>(scoreCombo) { Text = "%Combo%" });
            DebugToolkit.ScoreDebugItems.Add(new DebugStat<double>(scoreMiss) { Text = "%Miss%" });
            DebugToolkit.GeneralDebugItems.Add(new DebugStat<double>(pp) { Text = "PP" });
            DebugToolkit.GeneralDebugItems.Add(expectedJudgementCount = new DebugStat<int>(new Bindable<int>()) { Text = "Expected Judgement Count" });

            foreach (VitaruHitObject obj in beatmap.HitObjects)
                if (obj is Cluster cluster)
                    foreach (Projectile unused in cluster.GetProjectiles())
                        addJudge(unused);

            void addJudge(VitaruHitObject hit)
            {
                Judgement judgement = hit.CreateJudgement();
                JudgementResult result = CreateResult(judgement);
                result.Type = judgement.MaxResult;

                AddResult(result);

                expectedJudgementCount.Bindable.Value++;
            }
        }

        public override void PopulateScore(ScoreInfo score)
        {
            base.PopulateScore(score);

            score.Statistics[HitResult.Great] = ScoreResultCounts.GetOrDefault(HitResult.Great);
            score.Statistics[HitResult.Good] = ScoreResultCounts.GetOrDefault(HitResult.Good);
            score.Statistics[HitResult.Meh] = ScoreResultCounts.GetOrDefault(HitResult.Meh);
            score.Statistics[HitResult.Miss] = ScoreResultCounts.GetOrDefault(HitResult.Miss);
        }

        public override double GetStandardisedScore()
        {
            double missWeight = 1d + misses / miss_weight;
            scoreMiss.Value = Math.Round(missWeight, 2);

            double comboWeight = 1d + Math.Max(0d, HighestCombo / 2 - 1) / 25d;
            scoreCombo.Value = Math.Round(comboWeight, 2);

            return bonusScore + baseScore / missWeight * comboWeight;
        }

        protected override void ApplyResult(JudgementResult result)
        {
            VitaruJudgementResult vitaruResult = (VitaruJudgementResult)result;
            VitaruJudgement vitaruJudgement = vitaruResult.VitaruJudgement;

            vitaruResult.ComboAtJudgement = Combo;
            vitaruResult.HighestComboAtJudgement = HighestCombo;

            if (!vitaruJudgement.BonusScore)
                JudgedHits++;

            if (vitaruJudgement.AffectsCombo)
            {
                switch (vitaruResult.Type)
                {
                    case HitResult.None:
                        break;
                    case HitResult.Miss:
                        Combo.Value = 0;
                        misses++;
                        break;
                    default:
                        if (vitaruJudgement.Weight >= 1)
                            Combo.Value++;
                        break;
                }
            }

            if (vitaruJudgement.IsBonus && vitaruResult.IsHit)
                bonusScore += vitaruJudgement.NumericResultFor(vitaruResult);
            else
            {
                baseScore += vitaruJudgement.NumericResultFor(vitaruResult);
                rollingMaxBaseScore += vitaruJudgement.MaxNumericResult;
            }

            if (vitaruResult.Type != HitResult.None)
                ScoreResultCounts[vitaruResult.Type] = ScoreResultCounts.GetOrDefault(vitaruResult.Type) + 1;

            if (vitaruRulesetContainer?.VitaruPlayfield?.Player != null)
                Health.Value = vitaruRulesetContainer.VitaruPlayfield.Player.Health / vitaruRulesetContainer.VitaruPlayfield.Player.MaxHealth;

            ComboCount = Combo.Value;
        }

        protected override void RevertResult(JudgementResult result)
        {

        }

        private double misses;
        private double rollingMaxBaseScore;
        private double baseScore;
        private double bonusScore;

        //Amount of misses to half baseScore
        private const double miss_weight = 5;

        protected override void UpdateScore()
        {
            if (rollingMaxBaseScore != 0)
                Accuracy.Value = baseScore / rollingMaxBaseScore;

            double missWeight = 1d + misses / miss_weight;
            scoreMiss.Value = Math.Round(missWeight, 2);

            double comboWeight = 1d + Math.Max(0d, HighestCombo / 2 - 1) / 25d;
            scoreCombo.Value = Math.Round(comboWeight, 2);

            TotalScore.Value = bonusScore + baseScore / missWeight * comboWeight;

            PP = Math.Round(TotalScore.Value * VitaruPPCalculator.PP_MULTIPLIER, 2);
            pp.Value = PP;
        }
    }
}
