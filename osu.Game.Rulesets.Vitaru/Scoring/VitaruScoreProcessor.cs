using System.Collections.Generic;
using osu.Framework.Extensions;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Judgements;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Vitaru.UI;
using System.Linq;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Debug;

namespace osu.Game.Rulesets.Vitaru.Scoring
{
    internal class VitaruScoreProcessor : ScoreProcessor<VitaruHitObject>
    {
        private readonly VitaruRulesetContainer vitaruRulesetContainer;

        public new static int Combo;

        private DebugStat<int> expectedJudgementCount;

        public VitaruScoreProcessor(VitaruRulesetContainer vitaruRulesetContainer)
            : base(vitaruRulesetContainer)
        {
            this.vitaruRulesetContainer = vitaruRulesetContainer;
        }

        protected override void Reset(bool storeResults)
        {
            base.Reset(storeResults);

            Combo = 0;

            scoreResultCounts.Clear();
            comboResultCounts.Clear();
        }

        private readonly Dictionary<HitResult, int> scoreResultCounts = new Dictionary<HitResult, int>();
        private readonly Dictionary<ComboResult, int> comboResultCounts = new Dictionary<ComboResult, int>();

        protected override void SimulateAutoplay(Beatmap<VitaruHitObject> beatmap)
        {
            DebugToolkit.GeneralDebugItems.Add(expectedJudgementCount = new DebugStat<int>(new Bindable<int>()) { Text = "Expected Judgement Count" });

            foreach (var obj in beatmap.HitObjects)
            {
                if (obj is Pattern pattern)
                {
                    foreach (var unused in pattern.GetBullets().OfType<Bullet>())
                    {
                        AddJudgement(new VitaruJudgement { Result = HitResult.Great });
                        expectedJudgementCount.Bindable.Value++;
                    }
                    foreach (var unused in pattern.GetBullets().OfType<Laser>())
                    {
                        AddJudgement(new VitaruJudgement { Result = HitResult.Great });
                        expectedJudgementCount.Bindable.Value++;
                    }
                }
            }
        }

        public override void PopulateScore(Score score)
        {
            base.PopulateScore(score);

            score.Statistics[HitResult.Great] = scoreResultCounts.GetOrDefault(HitResult.Great);
            score.Statistics[HitResult.Good] = scoreResultCounts.GetOrDefault(HitResult.Good);
            score.Statistics[HitResult.Meh] = scoreResultCounts.GetOrDefault(HitResult.Meh);
            score.Statistics[HitResult.Miss] = scoreResultCounts.GetOrDefault(HitResult.Miss);
        }

        protected override void OnNewJudgement(Judgement judgement)
        {
            base.OnNewJudgement(judgement);

            var vitaruJudgement = (VitaruJudgement)judgement;

            if (judgement.Result != HitResult.None)
            {
                scoreResultCounts[judgement.Result] = scoreResultCounts.GetOrDefault(judgement.Result) + 1;
                comboResultCounts[vitaruJudgement.Combo] = comboResultCounts.GetOrDefault(vitaruJudgement.Combo) + 1;

                Combo++;

                if (judgement.Result == HitResult.Miss)
                    Combo = 0;
            }

            if (vitaruRulesetContainer.VitaruPlayfield.Player != null)
                Health.Value = vitaruRulesetContainer.VitaruPlayfield.Player.Health / vitaruRulesetContainer.VitaruPlayfield.Player.MaxHealth;

        }
    }
}
