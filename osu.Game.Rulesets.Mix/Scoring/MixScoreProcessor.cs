using System.Collections.Generic;
using osu.Framework.Extensions;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Mix.Judgements;
using osu.Game.Rulesets.Mix.Objects;
using osu.Game.Rulesets.Mix.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;

namespace osu.Game.Rulesets.Mix.Scoring
{
    internal class MixScoreProcessor : ScoreProcessor<MixHitObject>
    {
        public MixScoreProcessor()
        {
        }

        public MixScoreProcessor(RulesetContainer<MixHitObject> rulesetContainer)
            : base(rulesetContainer)
        {
        }

        protected override void Reset(bool storeResults)
        {
            base.Reset(storeResults);

            Health.Value = 1;

            TotalScore.Value = 0;

            scoreResultCounts.Clear();
            comboResultCounts.Clear();
        }

        private readonly Dictionary<HitResult, int> scoreResultCounts = new Dictionary<HitResult, int>();
        private readonly Dictionary<ComboResult, int> comboResultCounts = new Dictionary<ComboResult, int>();

        private float hpDrainRate;

        private int totalAccurateJudgements;

        public override void PopulateScore(Score score)
        {
            base.PopulateScore(score);

            score.Statistics[HitResult.Perfect] = scoreResultCounts.GetOrDefault(HitResult.Perfect);
            score.Statistics[HitResult.Great] = scoreResultCounts.GetOrDefault(HitResult.Great);
            score.Statistics[HitResult.Good] = scoreResultCounts.GetOrDefault(HitResult.Good);
            score.Statistics[HitResult.Meh] = scoreResultCounts.GetOrDefault(HitResult.Meh);
            score.Statistics[HitResult.Miss] = scoreResultCounts.GetOrDefault(HitResult.Miss);
        }

        protected override void SimulateAutoplay(Beatmap<MixHitObject> beatmap)
        {
            hpDrainRate = beatmap.BeatmapInfo.BaseDifficulty.DrainRate;
            totalAccurateJudgements = beatmap.HitObjects.Count;

            foreach (var unused in beatmap.HitObjects)
            {
                AddJudgement(new MixJudgement { Result = HitResult.Perfect });
            }
        }

        protected override void OnNewJudgement(Judgement judgement)
        {
            base.OnNewJudgement(judgement);

            var shapeJudgement = (MixJudgement)judgement;

            if (judgement != null)
            {
                if (judgement.Result != HitResult.None)
                {
                    scoreResultCounts[judgement.Result] = scoreResultCounts.GetOrDefault(judgement.Result) + 1;
                    comboResultCounts[shapeJudgement.Combo] = comboResultCounts.GetOrDefault(shapeJudgement.Combo) + 1;
                }

                switch (judgement.Result)
                {
                    case HitResult.Great:
                        Health.Value = Health.Value + 0.05f;
                        break;
                    case HitResult.Miss:
                        Health.Value = Health.Value - 0.1f;
                        break;
                }
            }
        }
    }
}
