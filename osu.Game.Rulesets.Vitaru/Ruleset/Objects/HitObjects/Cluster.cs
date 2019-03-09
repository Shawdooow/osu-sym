using System;
using System.Collections.Generic;
using osu.Framework.Configuration;
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
    public class Cluster : VitaruHitObject, IHasCurve, IHasTeam
    {
        /// <summary>
        /// All Cluster specific stuff
        /// </summary>
        #region Cluster
        public int Team { get; set; }

        public override double TimeUnPreempt => TimePreempt * 2;

        public override bool Hidden
        {
            get => base.Hidden;
            set
            {
                base.Hidden = value;
                foreach (Projectile p in NestedHitObjects)
                    p.Hidden = value;
            }

        }

        public override bool TrueHidden
        {
            get => base.TrueHidden;
            set
            {
                base.TrueHidden = value;
                foreach (Projectile p in NestedHitObjects)
                    p.TrueHidden = value;
            }

        }

        public override bool Flashlight
        {
            get => base.Flashlight;
            set
            {
                base.Flashlight = value;
                foreach (Projectile p in NestedHitObjects)
                    p.Flashlight = value;
            }

        }

        public bool Convert { get; set; }
        public double ClusterSpeed { get; set; } = 0.25d;
        public double ClusterDensity { get; set; } = 1;

        //Radians
        public double ClusterAngle { get; set; } = Math.PI / 2;
        public double ClusterDiameter { get; set; } = 20;
        public double ClusterDamage { get; set; } = 20;
        private double beatLength;

        public List<SampleInfo> BetterSamples = new List<SampleInfo>();

        public List<SampleControlPoint> SampleControlPoints = new List<SampleControlPoint>();
        #endregion

        /// <summary>
        /// All Slider specific stuff
        /// </summary>
        #region Slider
        public bool IsSlider { get; set; }
        public List<List<SampleInfo>> NodeSamples { get; set; } = new List<List<SampleInfo>>();
        private const float base_scoring_distance = 100;
        public double Duration => EndTime - StartTime;
        public readonly Bindable<SliderPath> PathBindable = new Bindable<SliderPath>();

        public SliderPath Path
        {
            get => PathBindable.Value;
            set => PathBindable.Value = value;
        }
        public int RepeatCount { get; set; }
        public double Velocity;
        public double SpanDuration => Duration / this.SpanCount();

        public List<SampleInfo> GetRepeatSamples(int repeat)
        {
            if (NodeSamples.Count > repeat)
                return NodeSamples[repeat];
            return BetterSamples;
        }

        public SampleControlPoint GetSampleControlPoint(int repeat)
        {
            if (SampleControlPoints.Count > repeat)
                return SampleControlPoints[repeat];
            return SampleControlPoint;
        }

        public List<SampleInfo> GetAdjustedSamples(int repeat = -1)
        {
            List<SampleInfo> list = new List<SampleInfo>();
            if (repeat >= 0)
                foreach (SampleInfo info in GetRepeatSamples(repeat))
                    list.Add(GetAdjustedSample(info, GetSampleControlPoint(repeat)));
            else
                foreach (SampleInfo info in BetterSamples)
                    list.Add(GetAdjustedSample(info));

            return list;
        }

        public Easing SpeedEasing { get; set; } = Easing.None;

        public override Vector2 EndPosition => PositionAt(1);
        public Vector2 PositionAt(double t) => Position + this.CurvePositionAt(Interpolation.ApplyEasing(SpeedEasing, t));

        public int RepeatAt(double progress) => (int)(progress * RepeatCount);

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

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = base_scoring_distance * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            Velocity = scoringDistance / timingPoint.BeatLength;

            beatLength = timingPoint.BeatLength;

            if (IsSlider)
            {
                EndTime = StartTime + this.SpanCount() * Path.Distance / Velocity;
                SampleControlPoints.Add(controlPointInfo.SamplePointAt(StartTime));

                for (double i = StartTime + SpanDuration; i <= EndTime; i += SpanDuration)
                {
                    if (i > 9000) break;
                    SampleControlPoint point = controlPointInfo.SamplePointAt(i);
                    SampleControlPoints.Add(point);
                }
            }
            else if (!IsSpinner)
                EndTime = StartTime;
        }
        #endregion

        /// <summary>
        /// All Spinner specific stuff
        /// </summary>
        #region Spinner
        public bool IsSpinner { get; set; }
        #endregion

        /// <summary>
        /// All Bullet loading stuff
        /// </summary>
        #region Bullet Loading
        public float EnemyHealth { get; set; } = 40;

        public void AddNested(Projectile projectile) => base.AddNested(projectile);

        public List<Projectile> GetProjectiles()
        {
            if (Convert)
            {
                List<Projectile> projectiles = new List<Projectile>();

                if (!IsSlider)
                    foreach (SampleInfo info in GetAdjustedSamples())
                        foreach (Projectile p in getCluster(Position, getPatternID(info)))
                        {
                            p.Ar = Ar;

                            p.Hidden = Hidden;
                            p.TrueHidden = TrueHidden;
                            p.Flashlight = Flashlight;

                            p.NewCombo = NewCombo;
                            p.ComboOffset = ComboOffset;
                            p.IndexInCurrentCombo = IndexInCurrentCombo;
                            p.ComboIndex = ComboIndex;
                            p.LastInCombo = LastInCombo;

                            projectiles.Add(p);
                        }

                if (IsSlider)
                {
                    foreach (SampleInfo info in GetAdjustedSamples(0))
                        foreach (Projectile p in getCluster(Position, getPatternID(info)))
                        {
                            p.Ar = Ar;

                            p.Hidden = Hidden;
                            p.TrueHidden = TrueHidden;
                            p.Flashlight = Flashlight;

                            p.NewCombo = NewCombo;
                            p.ComboOffset = ComboOffset;
                            p.IndexInCurrentCombo = IndexInCurrentCombo;
                            p.ComboIndex = ComboIndex;
                            p.LastInCombo = LastInCombo;

                            projectiles.Add(p);
                        }

                    for (int repeatIndex = 0, repeat = 1; repeatIndex < RepeatCount + 1; repeatIndex++, repeat++)
                        foreach (SampleInfo info in GetAdjustedSamples(repeat))
                            foreach (Projectile p in getCluster(Position + Path.PositionAt(repeat % 2), getPatternID(info)))
                            {
                                p.Ar = Ar;

                                p.Hidden = Hidden;
                                p.TrueHidden = TrueHidden;
                                p.Flashlight = Flashlight;

                                p.NewCombo = NewCombo;
                                p.ComboOffset = ComboOffset;
                                p.IndexInCurrentCombo = IndexInCurrentCombo;
                                p.ComboIndex = ComboIndex;
                                p.LastInCombo = LastInCombo;

                                p.StartTime = StartTime + repeat * SpanDuration;
                                p.Position = Position + Path.PositionAt(repeat % 2);
                                projectiles.Add(p);
                            }
                }

                return projectiles;
            }

            throw new NotImplementedException("Native vitaru! mapping doesn't exist yet!");
        }

        private List<Projectile> getCluster(Vector2 pos, int id)
        {
            switch (id)
            {
                default:
                    return Patterns.Wave(ClusterSpeed * (float)Velocity * 2, ClusterDiameter, ClusterDamage, pos, StartTime, Team, ClusterDensity, ClusterAngle);
                case 1:
                    return Patterns.Wave(ClusterSpeed * (float)Velocity * 2, ClusterDiameter, ClusterDamage, pos, StartTime, Team, ClusterDensity, ClusterAngle);
                case 2:
                    return Patterns.Line(ClusterSpeed * (float)Velocity * 2 * 0.75f, ClusterSpeed * (float)Velocity * 2 * 1.5f, ClusterDiameter, ClusterDamage, pos, StartTime, Team, ClusterDensity, ClusterAngle);
                case 3:
                    return Patterns.Triangle(ClusterSpeed * (float)Velocity * 2, ClusterDiameter, ClusterDamage, pos, StartTime, Team, ClusterDensity, ClusterAngle);
                case 4:
                    return Patterns.Wedge(ClusterSpeed * (float)Velocity * 2, ClusterDiameter, ClusterDamage, pos, StartTime, Team, ClusterDensity, ClusterAngle);
                case 5:
                    return Patterns.Circle(ClusterSpeed * (float)Velocity * 2, ClusterDiameter, ClusterDamage, pos, StartTime, Team, ClusterDensity);
                case 6:
                    return Patterns.Flower(ClusterSpeed * (float)Velocity * 2, ClusterDiameter, ClusterDamage, pos, StartTime, Duration, Team, beatLength, ClusterDensity);
            }
        }

        private int getPatternID(SampleInfo info)
        {
            if (IsSpinner) return 6;

            switch (info.Bank)
            {
                default:
                    throw new NotImplementedException("getPatternID(); => bad SampleInfo: " + info.Bank + " - " + info.Name);
                case "normal" when info.Name == "hitnormal":
                    return 1;
                case "normal" when info.Name == "hitwhistle":
                    return 2;
                case "normal" when info.Name == "hitfinish":
                    return 3;
                case "normal" when info.Name == "hitclap":
                    return 5;
                case "drum" when info.Name == "hitnormal":
                    return 1;
                case "drum" when info.Name == "hitwhistle":
                    return 2;
                case "drum" when info.Name == "hitfinish":
                    return 3;
                case "drum" when info.Name == "hitclap":
                    return 4;
                case "soft" when info.Name == "hitnormal":
                    return 1;
                case "soft" when info.Name == "hitwhistle":
                    return 2;
                case "soft" when info.Name == "hitfinish":
                    return 3;
                case "soft" when info.Name == "hitclap":
                    return 5;
            }
        }

        //Finds what direction the player is
        public float PlayerRelativePositionAngle(Vector2 playerPos, Vector2 enemyPos)
        {
            //Returns a Radian
            var playerAngle = (float)Math.Atan2(playerPos.Y - enemyPos.Y, playerPos.X - enemyPos.X);
            return playerAngle;
        }
        #endregion
    }
}
