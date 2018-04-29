// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Platform;
using osu.Game.Rulesets.Classic.Settings;
using osu.Game.Rulesets.Classic.UI;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class SliderBall : CircularContainer, ISliderProgress
    {
        private readonly Bindable<string> skin = ClassicSettings.ClassicConfigManager.GetBindable<string>(ClassicSetting.Skin);

        private const float width = 128;

        private Color4 accentColour = Color4.Black;
        /// <summary>
        /// The colour that is used for the slider ball.
        /// </summary>
        public Color4 AccentColour
        {
            get { return accentColour; }
            set
            {
                accentColour = value;
                if (ball != null)
                    ball.Colour = value;
            }
        }

        private readonly Slider slider;
        private readonly Sprite follow;
        private Sprite ball;
        private int ballNumber = 0;
        private double ballAnimationSavedTime = 0;
        private double trackingLossTime = 0;
        private bool countingDown;
        private double animationTimeInterval = 25;

        public SliderBall(Slider slider)
        {
            this.slider = slider;
            Masking = false;
            AutoSizeAxes = Axes.Both;
            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                follow = new Sprite
                {
                    Depth = 1,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre
                },
                ball = new Sprite
                {
                    Depth = 0,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Texture = ClassicRuleset.ClassicTextures.Get("sliderb" + ballNumber + "@2x"),
                },
                new Container
                {
                    Depth = 1,
                    Masking = true,
                    Colour = Color4.Black,
                    Size = new Vector2(width),
                    CornerRadius = width / 2,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            follow.Texture = ClassicSkinElement.LoadSkinElement("sliderfollowcircle", storage);
        }

        private InputState lastState;

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
        {
            lastState = state;
            return base.OnMouseDown(state, args);
        }

        protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
        {
            lastState = state;
            return base.OnMouseUp(state, args);
        }

        protected override bool OnMouseMove(InputState state)
        {
            lastState = state;
            return base.OnMouseMove(state);
        }

        // If the current time is between the start and end of the slider, we should track mouse input regardless of the cursor position.
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => canCurrentlyTrack || base.ReceiveMouseInputAt(screenSpacePos);

        private bool started;
        private bool tracking;
        public bool Tracking
        {
            get { return tracking; }
            private set
            {
                if (value == tracking) return;

                tracking = value;

                follow.ScaleTo(tracking ? 1f : 0.5f, 200, Easing.OutQuint);
                follow.FadeTo(tracking ? 1f : 0, 200, Easing.OutQuint);
            }
        }

        private bool canCurrentlyTrack => Time.Current >= slider.StartTime && Time.Current < slider.EndTime;

        protected override void Update()
        {
            base.Update();

            if (slider.StartTime <= Time.Current && !started)
            {
                started = true;
                ballAnimationSavedTime = Time.Current;
            }

            if (trackingLossTime <= Time.Current && countingDown)
            {
                Tracking = false;
                countingDown = false;
            }

            if(Time.Current - animationTimeInterval >= ballAnimationSavedTime && slider.StartTime <= Time.Current)
            {
                if (ballNumber > 9)
                    ballNumber = 0;
                ball.Texture = ClassicRuleset.ClassicTextures.Get("sliderb" + ballNumber + "@2x");
                ballNumber++;
                ballAnimationSavedTime = ballAnimationSavedTime + animationTimeInterval;
            }

            // Make sure to use the base version of ReceiveMouseInputAt so that we correctly check the position.
            if (Time.Current < slider.EndTime)
            {
                bool currentlyTracking = canCurrentlyTrack && lastState != null && base.ReceiveMouseInputAt(lastState.Mouse.NativeState.Position) && ((Parent as DrawableSlider)?.OsuActionInputManager?.PressedActions.Any(x => x == ClassicAction.LeftButton || x == ClassicAction.RightButton) ?? false);
                if (currentlyTracking)
                {
                    countingDown = false;
                    Tracking = currentlyTracking;
                }
                else if (!currentlyTracking && Tracking && !countingDown)
                {
                    countingDown = true;
                    trackingLossTime = Time.Current + 50;
                }
            }
                
        }

        public void UpdateProgress(double progress, int span)
        {
            Position = slider.Curve.PositionAt(progress);
        }
    }
}
