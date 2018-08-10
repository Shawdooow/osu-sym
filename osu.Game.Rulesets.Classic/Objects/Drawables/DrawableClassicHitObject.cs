// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.ComponentModel;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Framework.Graphics;
using System.Linq;
using osu.Game.Rulesets.Classic.Settings;
using Symcol.Rulesets.Core.HitObjects;
using osu.Game.Skinning;
using osu.Game.Rulesets.Objects.Types;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableClassicHitObject : DrawableSymcolHitObject<ClassicHitObject>
    {
        private readonly bool approaching = ClassicSettings.ClassicConfigManager.Get<bool>(ClassicSetting.Approaching);

        public const float TIME_FADEIN = 400;
        public const float TIME_FADEOUT = 240;

        protected DrawableClassicHitObject(ClassicHitObject hitObject)
            : base(hitObject)
        {
            if (approaching)
            {
                HitObject.TimePreempt /= (float)getSv(HitObject.SliderVelocity); ;

                double getSv(double value)
                {
                    const double fast = 1.25d;
                    const double slow = 0.75d + 0.25d / 2;

                    double scale = (slow - fast) / (ClassicHitObject.MinSliderVelocity - ClassicHitObject.MaxSliderVelocity);
                    return fast + (value - ClassicHitObject.MaxSliderVelocity) * scale;
                }
            }

            Alpha = 0;
            RulesetAudio = ClassicRuleset.ClassicAudio;
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
            this.FadeIn(TIME_FADEIN);
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
