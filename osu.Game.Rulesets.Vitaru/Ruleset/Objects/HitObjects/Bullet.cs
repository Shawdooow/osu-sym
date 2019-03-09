using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects
{
    public class Bullet : Projectile, IHasCurve
    {
        public override double TimePreempt => 100;

        public bool DummyMode { get; set; }

        public double Speed { get; set; } = 1f;
        public double Diameter { get; set; } = 28f;
        public bool DynamicVelocity { get; set; }
        public bool Piercing { get; set; }
        public bool ShootPlayer { get; set; }
        public bool ObeyBoundries { get; } = true;

        public double Curviness
        {
            get { return curviness; }
            set
            {
                curviness = value;
                SliderType = sliderType;
            }
        }

        private double curviness = 1;

        public Shape Shape;

        public SliderType SliderType
        {
            get { return sliderType; }
            set
            {
                sliderType = value;

                const float offset = 0.4f;

                switch (value)
                {
                    case SliderType.Straight:

                        Path = new SliderPath(PathType.Linear, new[]
                        {
                            Position,
                            new Vector2((float)Math.Cos(Angle) * 1000 + Position.X, (float)Math.Sin(Angle) * 1000 + Position.Y)
                        }, 1000);
                        break;
                    case SliderType.CurveLeft:

                        Path = new SliderPath(PathType.PerfectCurve, new[]
                        {
                            Position,
                            new Vector2((float)Math.Cos((Angle + offset) - 0.4f) * 600 + Position.X, (float)Math.Sin((Angle + offset) - 0.4f) * 600 + Position.Y),
                            new Vector2((float)Math.Cos(Angle + offset) * (float)(1200 / Curviness) + Position.X, (float)Math.Sin(Angle + offset) * (float)(1200 / Curviness) + Position.Y),
                        }, 1200);
                        break;
                    case SliderType.CurveRight:
                        Path = new SliderPath(PathType.PerfectCurve, new[]
                        {
                            Position,
                            new Vector2((float)Math.Cos((Angle - offset) + 0.4f) * 600 + Position.X, (float)Math.Sin((Angle - offset) + 0.4f) * 600 + Position.Y),
                            new Vector2((float)Math.Cos(Angle - offset) * (float)(1200 / Curviness) + Position.X, (float)Math.Sin(Angle - offset) * (float)(1200 / Curviness) + Position.Y),
                        }, 1200);
                        break;
                }
                EndTime = StartTime + Path.Distance / Velocity;
            }
        }
        private SliderType sliderType;

        public override double StartTime
        {
            get => startTime;
            set
            {
                if (startTime == value) return;

                startTime = value;
                EndTime = StartTime + Path.Distance / Velocity;
            }
        }
        private double startTime;

        public List<List<SampleInfo>> NodeSamples { get; set; } = new List<List<SampleInfo>>();
        public bool IsSlider { get; set; } = false;
        private const float base_scoring_distance = 100;
        public double Duration => EndTime - StartTime;
        public SliderPath Path { get; private set; } = new SliderPath();
        public double Velocity => Speed;
        public double SpanDuration => Duration / this.SpanCount();
        public int RepeatCount { get; set; }

        public Easing SpeedEasing { get; set; } = Easing.None;

        public ReadOnlySpan<Vector2> ControlPoints
        {
            get { return Path.ControlPoints; }
        }

        public PathType Type
        {
            get { return Path.Type; }
        }

        public double Distance
        {
            get { return Path.Distance; }
        }

        public override Vector2 EndPosition => this.CurvePositionAt(1);
        public Vector2 PositionAt(double t) => this.CurvePositionAt(Interpolation.ApplyEasing(SpeedEasing, t));

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            SliderType = sliderType;
        }
    }

    public enum SliderType
    {
        Straight,

        CurveRight,
        CurveLeft,
    }

    public enum Shape
    {
        Circle,
        Triangle,
        Square
    }
}
