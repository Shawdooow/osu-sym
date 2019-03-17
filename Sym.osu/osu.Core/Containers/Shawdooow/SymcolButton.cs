using System;
using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input.Events;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using osuTK;
using osuTK.Graphics;
using osuTK.Input;

namespace osu.Core.Containers.Shawdooow
{
    public class SymcolButton : BeatSyncedContainer
    {
        private readonly Box background;
        private readonly Triangles triangles;
        private readonly SpriteText buttonText;
        private readonly SpriteText buttonLabel;
        private readonly Box hover;

        public Vector2 StartingPosition = Vector2.Zero;

        public new Vector2 Position
        {
            get => base.Position;
            set
            {
                base.Position = value;
                if (StartingPosition == Vector2.Zero && value != Vector2.Zero)
                    StartingPosition = value;
            }
        }

        public new float Size
        {
            get => base.Size.X;
            set
            {
                if (value != base.Size.X)
                {
                    base.Size = new Vector2(value);

                    BorderThickness = value / 12;
                    CornerRadius = value / 2;

                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = ButtonColorBottom.Opacity(0.5f),
                        Type = EdgeEffectType.Shadow,
                        Radius = value / 4,
                    };

                    buttonText.TextSize = value / 4 * FontSizeMultiplier;
                    buttonLabel.TextSize = value / 4;
                }
            }
        }

        public Color4 ButtonColorTop
        {
            get => triangles.ColourDark;
            set
            {
                triangles.ColourDark = value;
                background.Colour = ColourInfo.GradientVertical(value == null ? Color4.White : value, ButtonColorBottom == null ? Color4.Black : ButtonColorBottom);
            }
        }
        public Color4 ButtonColorBottom
        {
            get => triangles.ColourLight;
            set
            {
                triangles.ColourLight = value;
                background.Colour = ColourInfo.GradientVertical(ButtonColorTop == null ? Color4.White : ButtonColorTop, value == null ? Color4.Black : value);
                EdgeEffect = new EdgeEffectParameters
                {
                    Colour = value.Opacity(0.5f),
                    Type = EdgeEffectType.Shadow,
                    Radius = Size / 4,
                };
            }
        }

        public float FontSizeMultiplier { get; set; } = 1;

        public string ButtonText
        {
            get => buttonText.Text;
            set => buttonText.Text = value;
        }
        public string ButtonLabel
        {
            get => buttonLabel.Text;
            set => buttonLabel.Text = value;
        }

        public Key Bind = Key.Unknown;

        public SymcolButton()
        {
            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;

            BorderColour = Color4.White;
            Masking = true;

            Children = new Drawable[]
            {
                background = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                },
                triangles = new Triangles
                {
                    TriangleScale = 1,
                    RelativeSizeAxes = Axes.Both,
                },
                buttonText = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                buttonLabel = new SpriteText
                {
                    Position = new Vector2(0, -10),
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                },
                hover = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                    Colour = Color4.White,
                },
            };
        }

        private const double early_activation = 60;

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            var beatLength = timingPoint.BeatLength;

            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            if (beatIndex < 0) return;

            this.ScaleTo(1 - 0.02f * amplitudeAdjust, early_activation, Easing.Out);
            using (BeginDelayedSequence(early_activation))
                this.ScaleTo(1, beatLength * 2, Easing.OutQuint);
        }

        private bool recieveInput;

        protected override bool OnHover(HoverEvent e)
        {
            hover.FadeTo(0.25f , 500, Easing.OutQuint);
            recieveInput = true;
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            hover.FadeOut(500, Easing.OutQuint);
            recieveInput = false;
            base.OnHoverLost(e);
        }

        protected override bool OnMouseDown(MouseDownEvent e)
        {
            this.ScaleTo(0.75f, 2000, Easing.OutQuint);
            if (Enabled.Value)
            {
                hover.FlashColour(Color4.White.Opacity(0.25f), 800, Easing.OutQuint);
                Action?.Invoke();
            }
                
            return base.OnMouseDown(e);
        }

        protected override bool OnMouseUp(MouseUpEvent e)
        {
            this.ScaleTo(1, 1000, Easing.OutElastic);
            return base.OnMouseUp(e);
        }

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (recieveInput && (e.Key == Key.X  || e.Key == Key.Z || e.Key == Key.C || e.Key == Key.V) || e.Key == Bind && Bind != Key.Unknown)
                Action?.Invoke();

            return base.OnKeyDown(e);
        }

        public Action Action;
        public readonly BindableBool Enabled = new BindableBool(true);
    }
}
