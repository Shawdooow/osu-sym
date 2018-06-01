using OpenTK;
using osu.Game.Audio;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Beatmaps;
using System;
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
        public int PatternID { get; set; }
        public double PatternSpeed { get; set; } = 0.25d;
        public double PatternComplexity { get; set; } = 1;

        //Radians
        public double PatternAngle { get; set; } = Math.PI / 2;
        public double PatternDiameter { get; set; } = 20;
        public double PatternDamage { get; set; } = 20;
        public int PatternTeam { get; set; } = 1;
        private double beatLength;

        public bool Drum { get; set; }
        public bool Soft { get; set; }

        public int Volume { get; set; }

        public bool Whistle { get; set; }
        public bool Finish { get; set; }
        public bool Clap { get; set; }

        public List<List<SampleInfo>> BetterRepeatSamples { get; set; } = new List<List<SampleInfo>>();
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

            if (IsSlider)
                EndTime = StartTime + this.SpanCount() * Curve.Distance / Velocity;
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

        #region Bullet Loading
        public float EnemyHealth { get; set; } = 40;

        public void AddNested(VitaruHitObject hitObject)
        {
            base.AddNested(hitObject);
        }

        public List<Bullet> GetBullets()
        {
            List<Bullet> bullets = new List<Bullet>();

            foreach (Bullet b in createPattern(Position))
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
                for (int repeatIndex = 0, repeat = 1; repeatIndex < RepeatCount + 1; repeatIndex++, repeat++)
                {
                    foreach (Bullet s in createPattern(Position + Curve.PositionAt(repeat % 2)))
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
            }

            return bullets;
        }

        private List<Bullet> createPattern(Vector2 pos)
        {
            switch (PatternID)
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
