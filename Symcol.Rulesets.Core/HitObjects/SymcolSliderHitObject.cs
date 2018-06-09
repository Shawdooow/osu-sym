using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using OpenTK;

namespace Symcol.Rulesets.Core.HitObjects
{
    public abstract class SymcolSliderHitObject : SymcolHitObject, IHasCurve
    {
        /// <summary>
        /// Scoring distance with a speed-adjusted beat length of 1 second.
        /// </summary>
        protected const float BASE_SCORING_DISTANCE = 100;

        public double EndTime => StartTime + this.SpanCount() * Curve.Distance / Velocity;
        public double Duration => EndTime - StartTime;

        public double SpanDuration => (Distance / Velocity) / this.SpanCount();

        public override Vector2 EndPosition => Position + PositionAt(1);

        public Bindable<Easing> SpeedEasing { get; set; } = new Bindable<Easing>();

        public Vector2 PositionAt(double t) => Curve.PositionAt(Interpolation.ApplyEasing(SpeedEasing.Value, t));

        public SliderCurve Curve { get; } = new SliderCurve();

        public List<Vector2> ControlPoints
        {
            get { return Curve.ControlPoints; }
            set { Curve.ControlPoints = value; }
        }

        public CurveType CurveType
        {
            get { return Curve.CurveType; }
            set { Curve.CurveType = value; }
        }

        public double Distance
        {
            get { return Curve.Distance; }
            set { Curve.Distance = value; }
        }

        public List<List<SampleInfo>> RepeatSamples { get; set; } = new List<List<SampleInfo>>();
        public int RepeatCount { get; set; }

        public double Velocity;
        public double TickDistance;

        public List<List<SampleInfo>> BetterRepeatSamples { get; set; } = new List<List<SampleInfo>>();

        public List<SampleControlPoint> SampleControlPoints = new List<SampleControlPoint>();

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = BASE_SCORING_DISTANCE * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            Velocity = scoringDistance / timingPoint.BeatLength;
            TickDistance = scoringDistance / difficulty.SliderTickRate;

            for (double i = StartTime + SpanDuration; i <= EndTime; i += SpanDuration)
            {
                SampleControlPoint point = controlPointInfo.SamplePointAt(i);
                SampleControlPoints.Add(new SampleControlPoint()
                {
                    SampleBank = point.SampleBank,
                    SampleBankCount = point.SampleBankCount,
                    SampleVolume = point.SampleVolume,
                });
            }
        }
    }
}
