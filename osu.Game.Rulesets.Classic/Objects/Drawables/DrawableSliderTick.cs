// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using OpenTK;
using OpenTK.Graphics;
using osu.Game.Rulesets.Classic.Judgements;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.Scoring;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableSliderTick : DrawableClassicHitObject
    {
        private readonly SliderTick sliderTick;

        public double FadeInTime;
        public double FadeOutTime;

        public bool Tracking;

        public override bool RemoveWhenNotAlive => false;

        public DrawableSliderTick(SliderTick sliderTick) : base(sliderTick)
        {
            this.sliderTick = sliderTick;
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;
            Masking = false;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            Children = new Drawable[]
            {
                new Sprite
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Texture =  ClassicSkinElement.LoadSkinElement("sliderscorepoint", storage),
                }
            };
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (timeOffset >= 0)
                AddJudgement(new ClassicJudgement { Result = Tracking ? HitResult.Great : HitResult.Miss });
        }

        protected override void UpdatePreemptState()
        {
            var animIn = Math.Min(150, sliderTick.StartTime - FadeInTime);

            this.Animate(
                d => d.FadeIn(animIn),
                d => d.ScaleTo(0.5f).ScaleTo(1.2f, animIn)
            ).Then(
                d => d.ScaleTo(1, 150, Easing.Out)
            );
        }

        protected override void UpdateCurrentState(ArmedState state)
        {
            switch (state)
            {
                case ArmedState.Idle:
                    this.Delay(FadeOutTime - sliderTick.StartTime).FadeOut();
                    break;
                case ArmedState.Miss:
                    this.FadeOut(160)
                        .FadeColour(Color4.Red, 80);
                    break;
                case ArmedState.Hit:
                    this.FadeOut(120, Easing.OutQuint)
                        .ScaleTo(Scale * 1.5f, 120, Easing.OutQuint);
                    break;
            }
        }
    }
}
