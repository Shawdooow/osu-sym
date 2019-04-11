#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Extensions;
using osu.Framework.Extensions.TypeExtensions;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;

#endregion

namespace osu.Mods.Rulesets.Core.Rulesets
{
    public abstract class SymcolScoreProcessor<TObject> : ScoreProcessor
        where TObject : HitObject
    {
        protected virtual double BasePortion => 0.3;
        protected virtual double ComboPortion => 0.7;
        protected virtual double MaxScore => 1000000;

        protected override bool HasCompleted => JudgedHits == MaxHits;

        protected int MaxHits { get; set; }
        protected int JudgedHits { get; set; }

        protected double PP { get; set; }

        protected double MaxHighestCombo;

        protected double MaxBaseScore;
        protected double RollingMaxBaseScore;
        protected double BaseScore;
        protected double BonusScore;

        protected SymcolScoreProcessor()
        {
        }

        protected SymcolScoreProcessor(DrawableRuleset<TObject> rulesetContainer)
        {
            rulesetContainer.OnNewResult += AddResult;
            rulesetContainer.OnRevertResult += RemoveResult;

            ApplyBeatmap(rulesetContainer.Beatmap);
            SimulateAutoplay(rulesetContainer.Beatmap);
            Reset(true);

            if (MaxBaseScore == 0 || MaxHighestCombo == 0)
            {
                Mode.Value = ScoringMode.Classic;
                Mode.Disabled = true;
            }

            Mode.ValueChanged += _ => UpdateScore();
        }

        /// <summary>
        /// Applies any properties of the <see cref="Beatmap{T}"/> which affect scoring to this <see cref="ScoreProcessor{TObject}"/>.
        /// </summary>
        /// <param name="beatmap">The <see cref="Beatmap{TObject}"/> to read properties from.</param>
        protected virtual void ApplyBeatmap(Beatmap<TObject> beatmap)
        {
        }

        /// <summary>
        /// Simulates an autoplay of the <see cref="Beatmap{TObject}"/> to determine scoring values.
        /// </summary>
        /// <remarks>This provided temporarily. DO NOT USE.</remarks>
        /// <param name="beatmap">The <see cref="Beatmap{TObject}"/> to simulate.</param>
        protected virtual void SimulateAutoplay(Beatmap<TObject> beatmap)
        {
            foreach (TObject obj in beatmap.HitObjects)
                simulate(obj);

            void simulate(HitObject obj)
            {
                foreach (HitObject nested in obj.NestedHitObjects)
                    simulate(nested);

                Judgement judgement = obj.CreateJudgement();
                if (judgement == null)
                    return;

                JudgementResult result = CreateResult(judgement);
                if (result == null)
                    throw new InvalidOperationException($"{GetType().ReadableName()} must provide a {nameof(JudgementResult)} through {nameof(CreateResult)}.");

                result.Type = judgement.MaxResult;

                AddResult(result);
            }
        }

        /// <summary>
        /// Adds the score change of a <see cref="JudgementResult"/> to this <see cref="ScoreProcessor"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to apply.</param>
        protected virtual void AddResult(JudgementResult result)
        {
            ApplyResult(result);
            UpdateScore();

            UpdateFailed();
            NotifyNewJudgement(result);
        }

        /// <summary>
        /// Removes the score change of a <see cref="JudgementResult"/> that was applied to this <see cref="ScoreProcessor"/>.
        /// </summary>
        /// <param name="judgement">The judgement to remove.</param>
        /// <param name="result">The judgement scoring result.</param>
        protected virtual void RemoveResult(JudgementResult result)
        {
            RevertResult(result);
            UpdateScore();
        }

        /// <summary>
        /// Applies the score change of a <see cref="JudgementResult"/> to this <see cref="ScoreProcessor"/>.
        /// </summary>
        /// <param name="result">The <see cref="JudgementResult"/> to apply.</param>
        protected virtual void ApplyResult(JudgementResult result)
        {
            //TODO: fix this
            //result.ComboAtJudgement = Combo;
            //result.HighestComboAtJudgement = HighestCombo;

            JudgedHits++;

            if (result.Judgement.AffectsCombo)
            {
                switch (result.Type)
                {
                    case HitResult.None:
                        break;
                    case HitResult.Miss:
                        Combo.Value = 0;
                        break;
                    default:
                        Combo.Value++;
                        break;
                }
            }

            if (result.Judgement.IsBonus)
            {
                if (result.IsHit)
                    BonusScore += result.Judgement.NumericResultFor(result);
            }
            else
            {
                BaseScore += result.Judgement.NumericResultFor(result);
                RollingMaxBaseScore += result.Judgement.MaxNumericResult;
            }
        }

        /// <summary>
        /// Reverts the score change of a <see cref="JudgementResult"/> that was applied to this <see cref="ScoreProcessor"/>.
        /// </summary>
        /// <param name="result">The judgement scoring result.</param>
        protected virtual void RevertResult(JudgementResult result)
        {
            Combo.Value = result.ComboAtJudgement;
            HighestCombo.Value = result.HighestComboAtJudgement;

            JudgedHits--;

            if (result.Judgement.IsBonus)
            {
                if (result.IsHit)
                    BonusScore -= result.Judgement.NumericResultFor(result);
            }
            else
            {
                BaseScore -= result.Judgement.NumericResultFor(result);
                RollingMaxBaseScore -= result.Judgement.MaxNumericResult;
            }
        }

        protected virtual void UpdateScore()
        {
            if (RollingMaxBaseScore != 0)
                Accuracy.Value = BaseScore / RollingMaxBaseScore;

            switch (Mode.Value)
            {
                case ScoringMode.Standardised:
                    TotalScore.Value = MaxScore * (BasePortion * BaseScore / MaxBaseScore + ComboPortion * HighestCombo.Value / MaxHighestCombo) + BonusScore;
                    break;
                case ScoringMode.Classic:
                    // should emulate osu-stable's scoring as closely as we can (https://osu.ppy.sh/help/wiki/Score/ScoreV1)
                    TotalScore.Value = BonusScore + BaseScore * (1 + Math.Max(0, HighestCombo.Value - 1) / 25);
                    break;
            }
        }

        protected readonly Dictionary<HitResult, int> ScoreResultCounts = new Dictionary<HitResult, int>();

        protected override void Reset(bool storeResults)
        {
            if (storeResults)
            {
                MaxHits = JudgedHits;
                MaxHighestCombo = HighestCombo.Value;
                MaxBaseScore = BaseScore;
            }

            base.Reset(storeResults);

            JudgedHits = 0;
            BaseScore = 0;
            RollingMaxBaseScore = 0;
            BonusScore = 0;
        }

        protected override int GetStatistic(HitResult result) => ScoreResultCounts.GetOrDefault(result);

        /// <summary>
        /// Creates the <see cref="JudgementResult"/> that represents the scoring result for a <see cref="HitObject"/>.
        /// </summary>
        /// <param name="judgement">The <see cref="Judgement"/> that provides the scoring information.</param>
        protected virtual JudgementResult CreateResult(Judgement judgement) => new JudgementResult(judgement);
    }
}
