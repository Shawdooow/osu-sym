#region usings

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

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects
{
    public class Bullet : Projectile, IHasCurve
    {
        public override double TimePreempt => 100;

        public float Speed { get; set; } = 1f;
        public float Diameter { get; set; } = 28f;
        public bool ShootPlayer { get; set; }
        public bool ObeyBoundries { get; set; } = true;

        public override Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                SliderType = sliderType;
            }
        }

        public double Curviness
        {
            get => curviness;
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
            get => sliderType;
            set
            {
                sliderType = value;
                UpdatePath();
            }
        }
        private SliderType sliderType;

        protected virtual void UpdatePath()
        {
            const float offset = 0.4f;

            switch (SliderType)
            {
                case SliderType.Straight:
                    Path = new SliderPath(PathType.Linear, new[]
                    {
                            Position,
                            new Vector2((float)Math.Cos(Angle) * (float)distance + Position.X, (float)Math.Sin(Angle) * (float)distance + Position.Y)
                        }, distance);
                    break;
                case SliderType.Target:
                    Path = new SliderPath(PathType.Linear, new[]
                    {
                            new Vector2((float)Math.Cos(Angle) * -(float)distance / 2 + Position.X, (float)Math.Sin(Angle) * -(float)distance / 2 + Position.Y),
                            new Vector2((float)Math.Cos(Angle) * (float)distance / 2 + Position.X, (float)Math.Sin(Angle) * (float)distance / 2 + Position.Y),
                        }, distance);
                    break;
                case SliderType.CurveLeft:
                    Path = new SliderPath(PathType.PerfectCurve, new[]
                    {
                            Position,
                            new Vector2((float)Math.Cos(Angle + offset - 0.4f) * (float)distance + Position.X, (float)Math.Sin(Angle + offset - 0.4f) * (float)distance + Position.Y),
                            new Vector2((float)Math.Cos(Angle + offset) * (float)((float)distance * 2 / Curviness) + Position.X, (float)Math.Sin(Angle + offset) * (float)((float)distance * 2 / Curviness) + Position.Y),
                        }, distance * 2);
                    break;
                case SliderType.CurveRight:
                    Path = new SliderPath(PathType.PerfectCurve, new[]
                    {
                            Position,
                            new Vector2((float)Math.Cos(Angle - offset + 0.4f) * (float)distance + Position.X, (float)Math.Sin(Angle - offset + 0.4f) * (float)distance + Position.Y),
                            new Vector2((float)Math.Cos(Angle - offset) * (float)((float)distance * 2 / Curviness) + Position.X, (float)Math.Sin(Angle - offset) * (float)((float)distance * 2 / Curviness) + Position.Y),
                        }, distance * 2);
                    break;
            }
            switch (SliderType)
            {
                default:
                    EndTime = StartTime + Path.Distance / Velocity;
                    break;
                case SliderType.Target:
                    StartTime = PatternStartTime - Path.Distance / Velocity / 2f;
                    break;
            }
        }

        public double PatternStartTime
        {
            get => patternStartTime;
            set
            {
                patternStartTime = value;
                StartTime = value;
            }
        }

        private double patternStartTime;

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
        public double Duration => EndTime - StartTime;
        public SliderPath Path { get; private set; } = new SliderPath();
        public double Velocity => Speed;
        public int RepeatCount { get; set; }

        public Easing SpeedEasing { get; set; } = Easing.None;

        public ReadOnlySpan<Vector2> ControlPoints => Path.ControlPoints;

        public PathType Type => Path.Type;

        public double Distance
        {
            get => Path.Distance;
            set
            {
                distance = value;
                UpdatePath();
            }
        }
        private double distance = 800;

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
        Target,

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
