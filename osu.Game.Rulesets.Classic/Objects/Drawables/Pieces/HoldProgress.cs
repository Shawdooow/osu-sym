using OpenTK;
using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Containers;
using System;
using System.Linq;

namespace osu.Game.Rulesets.Classic.Objects.Drawables.Pieces
{
    public class HoldProgress : BeatSyncedContainer
    {
        private readonly DrawableHold drawableHold;
        private CircularProgress progress;
        public Bindable<double> Progress = new Bindable<double> { Value = 0 };

        private double trackingLossTime = 0;
        private bool countingDown;

        public HoldProgress(DrawableHold h)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
            Alpha = 0.5f;

            drawableHold = h;
            Children = new Drawable[]
            {
                progress = new CircularProgress
                {
                    Depth = -1,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = h.Hold.ComboColour
                },
            };
            progress.Current.BindTo(Progress);
        }

        private const double early_activation = 60;

        private int lastBeatIndex;

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            {
                base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

                lastBeatIndex = beatIndex;

                var beatLength = timingPoint.BeatLength;

                float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

                if (beatIndex < 0) return;

                this.ScaleTo(1 - 0.02f * amplitudeAdjust, early_activation, Easing.Out);
                using (BeginDelayedSequence(early_activation))
                    this.ScaleTo(1, beatLength * 2, Easing.OutQuint);
            }
        }

        protected override void Update()
        {
            base.Update();

            if (trackingLossTime <= Time.Current && countingDown)
            {
                Tracking = false;
                countingDown = false;
            }

            if (Time.Current < drawableHold.Hold.EndTime)
            {
                bool currentlyTracking = canCurrentlyTrack && lastState != null && base.ReceiveMouseInputAt(lastState.Mouse.NativeState.Position) && ((Parent as DrawableHold)?.OsuActionInputManager?.PressedActions.Any(x => x == ClassicAction.LeftButton || x == ClassicAction.RightButton) ?? false);
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

        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => canCurrentlyTrack || base.ReceiveMouseInputAt(screenSpacePos);

        private bool tracking;
        public bool Tracking
        {
            get { return tracking; }
            private set
            {
                if (value == tracking) return;

                tracking = value;
            }
        }

        private bool canCurrentlyTrack => Time.Current >= drawableHold.Hold.StartTime && Time.Current < drawableHold.Hold.EndTime;
    }
}
