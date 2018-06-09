// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.ComponentModel;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Framework.Graphics;
using System.Linq;
using Symcol.Rulesets.Core.HitObjects;
using osu.Game.Skinning;
using osu.Game.Rulesets.Objects.Types;
using OpenTK.Graphics;
using System.Collections.Generic;
using Symcol.Rulesets.Core.Skinning;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableClassicHitObject : DrawableSymcolHitObject<ClassicHitObject>
    {
        public const float TIME_FADEOUT = 500;

        protected DrawableClassicHitObject(ClassicHitObject hitObject)
            : base(hitObject)
        {
            Alpha = 0;
        }

        public List<SymcolSkinnableSound> SymcolSkinnableSounds = new List<SymcolSkinnableSound>();

        protected virtual void PlayBetterSamples()
        {
            foreach (SymcolSkinnableSound sound in SymcolSkinnableSounds)
            {
                sound.Play();
                Remove(sound);
                sound.Delete();
            }
            SymcolSkinnableSounds = new List<SymcolSkinnableSound>();
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

        protected override void SkinChanged(ISkinSource skin, bool allowFallback)
        {
            base.SkinChanged(skin, allowFallback);

            if (HitObject is IHasComboInformation combo)
                AccentColour = skin.GetValue<SkinConfiguration, Color4>(s => s.ComboColours.Count > 0 ? s.ComboColours[combo.ComboIndex % s.ComboColours.Count] : (Color4?)null) ?? Color4.White;
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
