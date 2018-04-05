using OpenTK;
using osu.Game.Audio;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Objects;
using osu.Game.Beatmaps;
using System;
using osu.Game.Rulesets.Vitaru.Settings;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public class Pattern : VitaruHitObject, IHasCurve
    {
        public override HitObjectType Type => HitObjectType.Pattern;

        /// <summary>
        /// All Pattern specific stuff
        /// </summary>
        #region Pattern
        public int PatternID { get; set; }
        public double PatternSpeed { get; set; } = 0.25d;
        public double PatternComplexity { get; set; } = 1;

        //Radians
        public double PatternAngle { get; set; } = Math.PI / 2;
        public double PatternDiameter { get; set; } = 20;
        public double PatternDamage { get; set; } = 10;
        public int PatternTeam { get; set; } = 1;
        private double beatLength;
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

        public override Vector2 EndPosition => Position + this.CurvePositionAt(1);
        public Vector2 PositionAt(double t) => Position + this.CurvePositionAt(t);

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

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            createBullets();
        }

        private void createBullets()
        {
            IEnumerable<Bullet> startCircleBullets = createPattern();

            foreach (Bullet b in startCircleBullets)
            {
                b.Ar = Ar;

                b.NewCombo = NewCombo;
                b.IndexInCurrentCombo = IndexInCurrentCombo;
                b.ComboIndex = ComboIndex;
                b.LastInCombo = LastInCombo;

                AddNested(b);
            }

            if (IsSlider)
            {
                for (int repeatIndex = 0, repeat = 1; repeatIndex < RepeatCount + 1; repeatIndex++, repeat++)
                {
                    IEnumerable<Bullet> sliderBullets = createPattern();

                    foreach (Bullet s in sliderBullets)
                    {
                        s.Ar = Ar;

                        s.NewCombo = NewCombo;
                        s.IndexInCurrentCombo = IndexInCurrentCombo;
                        s.ComboIndex = ComboIndex;
                        s.LastInCombo = LastInCombo;

                        s.StartTime = StartTime + repeat * SpanDuration;
                        s.Position = Position + Curve.PositionAt(repeat % 2);
                        AddNested(s);
                    }
                }
            }
        }

        private IEnumerable<Bullet> createPattern()
        {
            switch (PatternID)
            {
                default:
                    return Patterns.Wave(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, Position, StartTime, PatternComplexity, PatternAngle);
                case 1:
                    return Patterns.Wave(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, Position, StartTime, PatternComplexity, PatternAngle);
                case 2:
                    return Patterns.Line((PatternSpeed * (float)Velocity * 2) * 0.75f, (PatternSpeed * (float)Velocity * 2) * 1.5f, PatternDiameter, PatternDamage, Position, StartTime, PatternComplexity, PatternAngle);
                case 3:
                    return Patterns.Triangle(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, Position, StartTime, PatternComplexity, PatternAngle);
                case 4:
                    return Patterns.Wedge(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, Position, StartTime, PatternComplexity, PatternAngle);
                case 5:
                    return Patterns.Circle(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, Position, StartTime, PatternComplexity);
                case 6:
                    return Patterns.Flower(PatternSpeed * (float)Velocity * 2, PatternDiameter, PatternDamage, Position, StartTime, Duration, beatLength, PatternComplexity);
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
