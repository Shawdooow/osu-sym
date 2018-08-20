using OpenTK;
using OpenTK.Graphics;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Game.Rulesets.Edit.Types;
using Symcol.Rulesets.Core.HitObjects;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public abstract class VitaruHitObject : SymcolHitObject, IHasComboInformation, IHasEditablePosition
    {
        public virtual double TimePreempt => 600;
        public virtual double TimeFadein => 400;

        public float Ar { get; set; } = -1;

        public event Action<Vector2> PositionChanged;

        private Vector2 position;

        public Vector2 Position
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

        public float X => Position.X;
        public float Y => Position.Y;

        public double EndTime { get; set; }

        public virtual Vector2 EndPosition => Position;

        public float Scale { get; set; } = 1;

        public abstract HitObjectType Type { get; }

        //TODO: make null != White
        public Color4? ColorOverride { get; set; }

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            EffectControlPoint effectPoint = controlPointInfo.EffectPointAt(StartTime);

            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }

        public virtual void OffsetPosition(Vector2 offset) => Position += offset;
    }
}
