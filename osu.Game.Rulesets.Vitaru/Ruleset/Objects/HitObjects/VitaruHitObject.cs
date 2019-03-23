#region usings

using System;
using osu.Game.Beatmaps;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osu.Mods.Rulesets.Core.HitObjects;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects
{
    public abstract class VitaruHitObject : SymcolHitObject, IHasComboInformation, IHasPosition
    {
        public virtual double TimePreempt => 600;
        public virtual double TimeUnPreempt => TimePreempt;
        public virtual double TimeFadein => 400;

        #region Mods

        public virtual bool Hidden { get; set; }

        public virtual bool TrueHidden { get; set; }

        public virtual bool Flashlight { get; set; }

        #endregion

        public float Ar { get; set; } = -1;

        public event Action<Vector2> PositionChanged;

        private Vector2 position;

        public virtual Vector2 Position
        {
            get => position;
            set
            {
                if (position == value)
                    return;
                position = value;

                PositionChanged?.Invoke(value);
            }
        }

        public override Judgement CreateJudgement() => new VitaruJudgement();

        public float X => Position.X;
        public float Y => Position.Y;

        public double EndTime { get; set; }

        public virtual Vector2 EndPosition => Position;

        public float Scale { get; set; } = 1;

        public Color4? ColorOverride { get; set; }

        public bool NewCombo { get; set; }

        public int ComboOffset { get; set; }

        public int IndexInCurrentCombo { get; set; }

        public int ComboIndex { get; set; }

        public bool LastInCombo { get; set; }

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);
            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }

        public virtual void OffsetPosition(Vector2 offset) => Position += offset;
    }
}
