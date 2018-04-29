// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.ComponentModel;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Framework.Graphics;
using System.Linq;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableClassicHitObject : DrawableHitObject<ClassicHitObject>
    {
        public const float TIME_FADEOUT = 500;

        protected DrawableClassicHitObject(ClassicHitObject hitObject)
            : base(hitObject)
        {
            AccentColour = HitObject.ComboColour;
            Alpha = 0;
        }

        protected sealed override void UpdateState(ArmedState state)
        {
            FinishTransforms();

            using (BeginAbsoluteSequence(HitObject.StartTime - HitObject.TimePreempt, true))
            {
                UpdateInitialState();

                UpdatePreemptState();

                using (BeginDelayedSequence(HitObject.TimePreempt + (Judgements.FirstOrDefault()?.TimeOffset ?? 0), true))
                    UpdateCurrentState(state);
            }
        }

        protected virtual void UpdateInitialState()
        {
            Hide();
        }

        protected virtual void UpdatePreemptState()
        {
            this.FadeIn(HitObject.TimeFadein);
        }

        protected virtual void UpdateCurrentState(ArmedState state)
        {
        }

        private ClassicInputManager osuActionInputManager;
        internal ClassicInputManager OsuActionInputManager => osuActionInputManager ?? (osuActionInputManager = GetContainingInputManager() as ClassicInputManager);
    }

    public enum ComboResult
    {
        [Description(@"")]
        None,
        [Description(@"Good")]
        Good,
        [Description(@"Amazing")]
        Perfect
    }
}
