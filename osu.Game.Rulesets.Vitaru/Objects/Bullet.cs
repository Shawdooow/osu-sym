using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Game.Audio;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class Bullet : VitaruHitObject, IHasCurve
    {
        public override double TimePreempt => 100;

        public override HitObjectType Type => HitObjectType.Bullet;

        public bool DummyMode { get; set; }

        public double Damage { get; set; } = 10;
        public double Speed { get; set; } = 1f;
        public double Diameter { get; set; } = 16f;
        public double Angle { get; set; }
        public bool DynamicVelocity { get; set; }
        public bool Piercing { get; set; }
        public int Team { get; set; } = -1;
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

                float offset = 0.4f;

                switch (value)
                {
                    case SliderType.Straight:
                        Curve = new SliderCurve()
                        {
                            CurveType = CurveType.Linear,
                            Distance = 1000,
                            ControlPoints = new List<Vector2>
                            {
                                Position,
                                new Vector2((float)Math.Cos(Angle) * 1000 + Position.X, (float)Math.Sin(Angle) * 1000 + Position.Y)
                            },
                        };
                        break;
                    case SliderType.CurveLeft:
                        Curve = new SliderCurve()
                        {
                            CurveType = CurveType.PerfectCurve,
                            Distance = 1200,
                            ControlPoints = new List<Vector2>
                            {
                                Position,
                                new Vector2((float)Math.Cos((Angle + offset) - 0.4f) * 600 + Position.X, (float)Math.Sin((Angle + offset) - 0.4f) * 600 + Position.Y),
                                new Vector2((float)Math.Cos(Angle + offset) * (float)(1200 / Curviness) + Position.X, (float)Math.Sin(Angle + offset) * (float)(1200 / Curviness) + Position.Y),
                            },
                        };
                        break;
                    case SliderType.CurveRight:
                        Curve = new SliderCurve()
                        {
                            CurveType = CurveType.PerfectCurve,
                            Distance = 1200,
                            ControlPoints = new List<Vector2>
                            {
                                Position,
                                new Vector2((float)Math.Cos((Angle - offset) + 0.4f) * 600 + Position.X, (float)Math.Sin((Angle - offset) + 0.4f) * 600 + Position.Y),
                                new Vector2((float)Math.Cos(Angle - offset) * (float)(1200 / Curviness) + Position.X, (float)Math.Sin(Angle - offset) * (float)(1200 / Curviness) + Position.Y),
                            },
                        };
                        break;
                }
                EndTime = StartTime + Curve.Distance / Velocity;

            }
        }

        private SliderType sliderType;

        public List<List<SampleInfo>> RepeatSamples { get; set; } = new List<List<SampleInfo>>();
        public bool IsSlider { get; set; } = false;
        private const float base_scoring_distance = 100;
        public double Duration => EndTime - StartTime;
        public SliderCurve Curve { get; private set; } = new SliderCurve();
        public double Velocity => Speed;
        public double SpanDuration => Duration / this.SpanCount();
        public int RepeatCount { get; set; }

        public Easing SpeedEasing { get; set; } = Easing.None;

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
