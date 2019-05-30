// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Game.Beatmaps;
using OpenTK;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Scoring;
using System.Collections.Generic;
using osu.Game.Audio;
using Symcol.Rulesets.Core.HitObjects;

namespace osu.Game.Rulesets.Classic.Objects
{
    public abstract class ClassicHitObject : SymcolHitObject, IHasComboInformation
    {
        public const double OBJECT_RADIUS = 64;

        public float TimePreempt = 600;

        private double hittable_range = 300;
        public double HitWindow50 = 150;
        public double HitWindow100 = 80;
        public double HitWindow300 = 30;

        public bool First { get; set; }

        public bool SliderStartCircle { get; set; }

        public List<SampleInfo> BetterSamples { get; set; } = new List<SampleInfo>();

        public bool Hidden { get; set; }

        public Vector2 Position { get; set; }
        public float X => Position.X;
        public float Y => Position.Y;

        public Vector2 StackedPosition => Position + StackOffset;

        public virtual Vector2 EndPosition => Position;

        public Vector2 StackedEndPosition => EndPosition + StackOffset;

        public virtual int StackHeight { get; set; }

        public Vector2 StackOffset => new Vector2(StackHeight * Scale * -6.4f);

        public double Radius => OBJECT_RADIUS * Scale;

        public static double MaxSliderVelocity;

        public double SliderVelocity
        {
            get => sv;
            set
            {
                sv = value;

                if (sv > MaxSliderVelocity)
                    MaxSliderVelocity = sv;

                if (sv < MinSliderVelocity)
                    MinSliderVelocity = sv;
            }
        }

        private double sv;

        public static double MinSliderVelocity;

        public float Scale { get; set; } = 1;

        public int ID { get; set; }

        public virtual bool NewCombo { get; set; }

        public int IndexInCurrentCombo { get; set; }

        public int ComboIndex { get; set; }

        public bool LastInCombo { get; set; }

        public double HitWindowFor(HitResult result)
        {
            switch (result)
            {
                default:
                    return hittable_range;
                case HitResult.Miss:
                    return hittable_range;
                case HitResult.Meh:
                    return HitWindow50;
                case HitResult.Good:
                    return HitWindow100;
                case HitResult.Great:
                    return HitWindow300;
            }
        }

        public HitResult ScoreResultForOffset(double offset)
        {
            if (offset < HitWindowFor(HitResult.Great))
                return HitResult.Great;
            if (offset < HitWindowFor(HitResult.Good))
                return HitResult.Good;
            if (offset < HitWindowFor(HitResult.Meh))
                return HitResult.Meh;
            if (offset < HitWindowFor(HitResult.Miss))
                return HitResult.Miss;
            return HitResult.None;
        }

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = 100 * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            SliderVelocity = scoringDistance / timingPoint.BeatLength;

            TimePreempt = (float)BeatmapDifficulty.DifficultyRange(difficulty.ApproachRate, 1800, 1200, 450);

            HitWindow50 = BeatmapDifficulty.DifficultyRange(difficulty.OverallDifficulty, 200, 150, 100);
            HitWindow100 = BeatmapDifficulty.DifficultyRange(difficulty.OverallDifficulty, 140, 100, 60);
            HitWindow300 = BeatmapDifficulty.DifficultyRange(difficulty.OverallDifficulty, 80, 50, 20);

            SampleControlPoint = controlPointInfo.SamplePointAt(StartTime);

            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }
    }
}
