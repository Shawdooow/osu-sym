using OpenTK;
using osu.Game.Audio;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Beatmaps;
using System;
using osu.Framework.Audio.Sample;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class Pattern : VitaruHitObject, IHasCurve
    {
        public override HitObjectType Type => HitObjectType.Pattern;

        /// <summary>
        /// All Pattern specific stuff
        /// </summary>
        #region Pattern
        public bool Convert { get; set; }
        public double PatternSpeed { get; set; } = 0.25d;
        public double PatternComplexity { get; set; } = 1;

        //Radians
        public double PatternAngle { get; set; } = Math.PI / 2;
        public double PatternDiameter { get; set; } = 20;
        public double PatternDamage { get; set; } = 20;
        public int PatternTeam { get; set; } = 1;
        private double beatLength;

        public List<SampleInfo> BetterSamples = new List<SampleInfo>();

        public List<SampleControlPoint> SampleControlPoints = new List<SampleControlPoint>();
        #endregion

        /// <summary>
        /// All Slider specific stuff
        /// </summary>
        #region Slider
        public bool IsSlider { get; set; } = false;
        public List<List<SampleInfo>> RepeatSamples { get; set; } = new List<List<SampleInfo>>();
        private const float base_scoring_distance = 100;
        public double Duration => EndTime - StartTime;
        public SliderCurve Curve { get; } = new SliderCurve();
        public int RepeatCount { get; set; }
        public double Velocity;
        public double SpanDuration => Duration / this.SpanCount();

        private List<SampleInfo> ree;
        public List<SampleInfo> GetRepeatSamples(int repeat)
        {
            if (RepeatSamples.Count > repeat)
                ree = RepeatSamples[repeat];
            return ree;
        }

        private SampleControlPoint reee;
        public SampleControlPoint GetSampleControlPoint(int repeat)
        {
            if (SampleControlPoints.Count > repeat)
                reee = SampleControlPoints[repeat];
            return reee;
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

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            TimingControlPoint timingPoint = controlPointInfo.TimingPointAt(StartTime);
            DifficultyControlPoint difficultyPoint = controlPointInfo.DifficultyPointAt(StartTime);

            double scoringDistance = base_scoring_distance * difficulty.SliderMultiplier * difficultyPoint.SpeedMultiplier;

            Velocity = scoringDistance / timingPoint.BeatLength;

            beatLength = timingPoint.BeatLength;

            reee = SampleControlPoint;
            ree = Samples;

            if (IsSlider)
            {
                EndTime = StartTime + this.SpanCount() * Curve.Distance / Velocity;

                for (double i = StartTime + SpanDuration; i <= EndTime; i += SpanDuration)
                {
                    SampleControlPoint point = controlPointInfo.SamplePointAt(i);
                    SampleControlPoints.Add(new SampleControlPoint
                    {
                        SampleBank = point.SampleBank,
                        SampleBankCount = point.SampleBankCount,
                        SampleVolume = point.SampleVolume,
                    });
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

        public void AddNested(VitaruHitObject hitObject)
        {
            base.AddNested(hitObject);
        }

        public List<Bullet> GetBullets()
        {
            if (Convert)
            {
                List<Bullet> bullets = new List<Bullet>();

                if (!IsSlider)
                    foreach (SampleInfo info in GetAdjustedSamples())
                        foreach (Bullet b in getPattern(Position, getPatternID(info)))
                        {
                            b.Ar = Ar;

                            if (IsSpinner)
                                b.Abstraction = 2;
                            else if (!IsSlider)
                                b.Abstraction = 1;

                            b.NewCombo = NewCombo;
                            b.IndexInCurrentCombo = IndexInCurrentCombo;
                            b.ComboIndex = ComboIndex;
                            b.LastInCombo = LastInCombo;

                            bullets.Add(b);
                        }

                if (IsSlider)
                {
                    foreach (SampleInfo info in GetAdjustedSamples(0))
                        foreach (Bullet b in getPattern(Position, getPatternID(info)))
                        {
                            b.Ar = Ar;

                            if (IsSpinner)
                                b.Abstraction = 2;
                            else if (!IsSlider)
                                b.Abstraction = 1;

                            b.NewCombo = NewCombo;
                            b.IndexInCurrentCombo = IndexInCurrentCombo;
                            b.ComboIndex = ComboIndex;
                            b.LastInCombo = LastInCombo;

                            bullets.Add(b);
                        }

                    for (int repeatIndex = 0, repeat = 1; repeatIndex < RepeatCount + 1; repeatIndex++, repeat++)
                        foreach (SampleInfo info in GetAdjustedSamples(repeat))
                            foreach (Bullet s in getPattern(Position + Curve.PositionAt(repeat % 2), getPatternID(info)))
                            {
                                s.Ar = Ar;

                                s.NewCombo = NewCombo;
                                s.IndexInCurrentCombo = IndexInCurrentCombo;
                                s.ComboIndex = ComboIndex;
                                s.LastInCombo = LastInCombo;

                                s.StartTime = StartTime + repeat * SpanDuration;
                                s.Position = Position + Curve.PositionAt(repeat % 2);
                                bullets.Add(s);
                            }
                }

                return bullets;
            }

            throw new NotImplementedException("Native vitaru! mapping doesn't exist yet!");
        }

        private List<Bullet> getPattern(Vector2 pos, int id)
        {
            switch (id)
            {
                default:
                    return BulletPatterns.Wave(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, pos, StartTime, PatternComplexity, PatternAngle);
                case 1:
                    return BulletPatterns.Wave(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, pos, StartTime, PatternComplexity, PatternAngle);
                case 2:
                    return BulletPatterns.Line((PatternSpeed * (float)Velocity * 2) * 0.75f, (PatternSpeed * (float)Velocity * 2) * 1.5f, PatternDiameter, PatternDamage, pos, StartTime, PatternComplexity, PatternAngle);
                case 3:
                    return BulletPatterns.Triangle(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, pos, StartTime, PatternComplexity, PatternAngle);
                case 4:
                    return BulletPatterns.Wedge(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, pos, StartTime, PatternComplexity, PatternAngle);
                case 5:
                    return BulletPatterns.Circle(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, pos, StartTime, PatternComplexity);
                case 6:
                    return BulletPatterns.Flower(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, pos, StartTime, Duration, beatLength, PatternComplexity);
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
            var playerAngle = (float)Math.Atan2((playerPos.Y - enemyPos.Y), (playerPos.X - enemyPos.X));
            return playerAngle;
        }
        #endregion
    }
}
