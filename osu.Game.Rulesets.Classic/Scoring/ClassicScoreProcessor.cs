// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Collections.Generic;
using osu.Framework.Extensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Classic.Judgements;
using osu.Game.Rulesets.Classic.Objects;
using osu.Game.Rulesets.Classic.Objects.Drawables;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Rulesets.Classic.UI;
using System.Linq;
using System;

namespace osu.Game.Rulesets.Classic.Scoring
{
    internal class ClassicScoreProcessor : ScoreProcessor<ClassicHitObject>
    {
        public new static int Combo;

        public ClassicScoreProcessor(RulesetContainer<ClassicHitObject> rulesetContainer)
            : base(rulesetContainer)
        {
            if (ClassicPlayfield.OnJudgement == null)
                ClassicPlayfield.OnJudgement += j => CreateResult(j);
        }

        //public override bool PassiveHealthDrain => true;

        private float hpDrainRate;

        private readonly Dictionary<HitResult, int> scoreResultCounts = new Dictionary<HitResult, int>();
        private readonly Dictionary<ComboResult, int> comboResultCounts = new Dictionary<ComboResult, int>();

        protected override void SimulateAutoplay(Beatmap<ClassicHitObject> beatmap)
        {
            hpDrainRate = beatmap.BeatmapInfo.BaseDifficulty.DrainRate;

            foreach (var obj in beatmap.HitObjects)
            {
                if (obj is Slider slider)
                {
                    // Ticks
                    foreach (var unused in slider.NestedHitObjects.OfType<SliderTick>())
                        CreateResult(new ClassicJudgement { Result = HitResult.Great });

                    // RepeatPoints
                    foreach (var unused in slider.NestedHitObjects.OfType<RepeatPoint>())
                        CreateResult(new ClassicJudgement { Result = HitResult.Great });
                }

                if (obj is Hold hold)
                {
                    // Ticks
                    foreach (var unused in hold.NestedHitObjects.OfType<SliderTick>())
                        CreateResult(new ClassicJudgement { Result = HitResult.Great });

                    // RepeatPoints
                    foreach (var unused in hold.NestedHitObjects.OfType<RepeatPoint>())
                        CreateResult(new ClassicJudgement { Result = HitResult.Great });
                }

                CreateResult(new ClassicJudgement { Result = HitResult.Great });
            }
        }

        protected override void Reset(bool storeResults)
        {
            base.Reset(storeResults);

            Combo = 0;

            scoreResultCounts.Clear();
            comboResultCounts.Clear();
        }

        public override void PopulateScore(Score score)
        {
            base.PopulateScore(score);

            score.Statistics[HitResult.Great] = scoreResultCounts.GetOrDefault(HitResult.Great);
            score.Statistics[HitResult.Good] = scoreResultCounts.GetOrDefault(HitResult.Good);
            score.Statistics[HitResult.Meh] = scoreResultCounts.GetOrDefault(HitResult.Meh);
            score.Statistics[HitResult.Miss] = scoreResultCounts.GetOrDefault(HitResult.Miss);
        }

        protected override void ApplyResult(JudgementResult result)
        {
            base.ApplyResult(result);

            var osuJudgement = (ClassicJudgement)result.Judgement;

            if (osuJudgement.Result != HitResult.None)
            {
                scoreResultCounts[osuJudgement.Result] = scoreResultCounts.GetOrDefault(osuJudgement.Result) + 1;
                comboResultCounts[osuJudgement.Combo] = comboResultCounts.GetOrDefault(osuJudgement.Combo) + 1;
            }

            Combo++;                

            switch (osuJudgement.Result)
            {
                case HitResult.Great:
                    ClassicUi.CurrentHealth = Math.Min(ClassicUi.CurrentHealth + (10.2 - hpDrainRate) * 0.02, 1);
                    Health.Value = ClassicUi.CurrentHealth;
                    break;

                case HitResult.Good:
                    ClassicUi.CurrentHealth = Math.Min(ClassicUi.CurrentHealth + (8 - hpDrainRate) * 0.01, 1);
                    Health.Value = ClassicUi.CurrentHealth;
                    break;

                case HitResult.Meh:
                    ClassicUi.CurrentHealth = Math.Min(ClassicUi.CurrentHealth + (4 - hpDrainRate) * 0.005, 1);
                    Health.Value = ClassicUi.CurrentHealth;
                    break;

                /*case HitResult.SliderTick:
                    Health.Value += Math.Max(7 - hpDrainRate, 0) * 0.01;
                    break;*/

                case HitResult.Miss:
                    if (Combo >= 10)
                        ClassicRuleset.ClassicAudio.Sample.Get("Gameplay/combobreak").Play();
                    Combo = 0;
                    ClassicUi.CurrentHealth = Math.Max(ClassicUi.CurrentHealth - hpDrainRate * 0.03, 0);
                    Health.Value = ClassicUi.CurrentHealth;
                    break;
            }
        }
    }
}
