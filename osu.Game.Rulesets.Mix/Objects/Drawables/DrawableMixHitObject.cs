﻿using osu.Game.Rulesets.Objects.Drawables;
using System;
using System.ComponentModel;
using osu.Game.Rulesets.Mix.Judgements;
using osu.Framework.Input.Bindings;
using Symcol.Rulesets.Core.HitObjects;

namespace osu.Game.Rulesets.Mix.Objects.Drawables
{
    public abstract class DrawableMixHitObject<ShapeObject> : DrawableSymcolHitObject<MixHitObject>, IKeyBindingHandler<ShapeAction>
        where ShapeObject : MixHitObject
    {
        public float TIME_PREEMPT = 800;
        public float TIME_FADEIN = 400;
        public float TIME_FADEOUT = 400;

        public DrawableMixHitObject(MixHitObject hitObject)
            : base(hitObject)
        {
        }

        protected override void UpdateState(ArmedState state)
        {

        }

        protected virtual void UpdatePreemptState()
        {

        }

        protected virtual void UpdateInitialState()
        {

        }

        public abstract bool OnPressed(ShapeAction action);

        public virtual bool OnReleased(ShapeAction action) => false;
    }

    public enum ComboResult
    {
        [Description(@"")]
        None,
        [Description(@"Good")]
        Good,
        [Description(@"Amazing!")]
        Perfect
    }
}
