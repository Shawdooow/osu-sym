﻿using osu.Game.Rulesets.Objects;
using OpenTK;
using OpenTK.Graphics;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects.Types;
using System;

namespace osu.Game.Rulesets.Vitaru.Objects
{
    public abstract class VitaruHitObject : HitObject, IHasComboInformation
    {
        public double TimePreempt = 600;
        public double TimeFadein = 400;

        public float Ar { get; set; } = -1;

        public float Cs { get; set; } = -1;

        public Vector2 Position { get; set; }

        public double EndTime { get; set; }

        public virtual Vector2 EndPosition => Position;

        public float Scale { get; set; } = 1;

        public abstract HitObjectType Type { get; }

        //TODO: make null != White
        public Color4 ColorOverride { get; set; } = Color4.White;

        public virtual bool NewCombo { get; set; }

        public int IndexInCurrentCombo { get; set; }

        public int ComboIndex { get; set; }

        public bool LastInCombo { get; set; }

        protected override void ApplyDefaultsToSelf(ControlPointInfo controlPointInfo, BeatmapDifficulty difficulty)
        {
            base.ApplyDefaultsToSelf(controlPointInfo, difficulty);

            EffectControlPoint effectPoint = controlPointInfo.EffectPointAt(StartTime);

            Scale = (1.0f - 0.7f * (difficulty.CircleSize - 5) / 5) / 2;
        }
    }
}
