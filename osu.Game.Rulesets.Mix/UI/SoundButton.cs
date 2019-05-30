using OpenTK;
using OpenTK.Graphics;
using OpenTK.Input;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Framework.Input.Bindings;
using osu.Framework.Input.EventArgs;
using osu.Framework.Input.States;
using osu.Game.Graphics.Backgrounds;

namespace osu.Game.Rulesets.Mix.UI
{
    public class SoundButton : Container, IKeyBindingHandler<MixAction>, IRequireHighFrequencyMousePosition
    {
        private readonly KeyBindingContainer<MixAction> bindingContainer;

        private Box hover;
        private Container content;

        private readonly MixAction bind;

        public SoundButton(MixAction bind, KeyBindingContainer<MixAction> bindingContainer)
        {
            this.bindingContainer = bindingContainer;
            this.bind = bind;

            string bank = getBank(bind);
            string name = getName(bind);

            Size = new Vector2(56);

            Color4 color = Color4.Red;

            if (bank == "d")
                color = Color4.Green;
            else if (bank == "s")
                color = Color4.Blue;

            if (bind < MixAction.NormalNormalRight)
            {
                Anchor = Anchor.CentreRight;

                if (bank == "n")
                    Position = new Vector2(-(int)bind * Size.X, -Size.X - 8);
                else if (bank == "d")
                    Position = new Vector2(-((int)bind - 4) * Size.X, 0);
                else
                    Position = new Vector2(-((int)bind - 8) * Size.X, Size.X + 8);
            }
            else
            {
                Anchor = Anchor.CentreLeft;

                if (bank == "n")
                    Position = new Vector2((int)bind * Size.X, -Size.X - 8);
                else if (bank == "d")
                    Position = new Vector2(((int)bind - 4) * Size.X, 0);
                else
                    Position = new Vector2(((int)bind - 8) * Size.X, Size.X + 8);
            }

            Origin = Anchor.Centre;

            Children = new Drawable[]
            {
                content = new CircularContainer
                {
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,

                    BorderColour  = Color4.White,
                    BorderThickness = 4,

                    Masking = true,
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = color.Opacity(0.1f),
                        Type = EdgeEffectType.Shadow,
                        Radius = 4,
                    },
                    Children = new Drawable[]
                    {
                        new Triangles
                        {
                            Depth = -1,
                            TriangleScale = 1,
                            ColourLight = color.Lighten(0.4f),
                            ColourDark = color.Darken(0.4f),
                            RelativeSizeAxes = Axes.Both,
                        },
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = ColourInfo.GradientVertical(color.Darken(0.4f), color.Lighten(0.4f))
                        },
                        hover = new Box
                        {
                            Depth = -2,
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                            Colour = Color4.White,
                        }
                    }
                },
                new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Text = name,
                    TextSize = 12,
                },
                new SpriteText
                {
                    Position = new Vector2(0 , 4),
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Text = bank,
                    TextSize = 10,
                }
            };
        }

        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => content.ReceiveMouseInputAt(screenSpacePos);

        private bool recieveInput = false;

        protected override bool OnHover(InputState state)
        {
            hover.FadeTo(0.25f, 500, Easing.OutQuint);
            recieveInput = true;
            return base.OnHover(state);
        }

        protected override void OnHoverLost(InputState state)
        {
            hover.FadeOut(500, Easing.OutQuint);
            recieveInput = false;
            base.OnHoverLost(state);
        }

        public bool OnPressed(MixAction action)
        {
            if (action == bind)
            {
                content.ScaleTo(0.75f, 2000, Easing.OutQuint);
                hover.FlashColour(Color4.White.Opacity(0.25f), 800, Easing.OutQuint);
            }

            return false;
        }

        public bool OnReleased(MixAction action)
        {
            if (action == bind)
            {
                content.ScaleTo(1, 1000, Easing.OutElastic);
            }

            return false;
        }

        protected override bool OnMouseDown(InputState state, MouseDownEventArgs args)
        {
            if (recieveInput)
            {
                bindingContainer.TriggerPressed(bind);
                return true;
            }
            return false;
        }

        protected override bool OnMouseUp(InputState state, MouseUpEventArgs args)
        {
            bindingContainer.TriggerReleased(bind);
            return base.OnMouseUp(state, args);
        }

        protected override bool OnKeyDown(InputState state, KeyDownEventArgs args)
        {
            if (recieveInput && (args.Key == Key.X || args.Key == Key.Z))
                bindingContainer.TriggerPressed(bind);

            return base.OnKeyDown(state, args);
        }

        protected override bool OnKeyUp(InputState state, KeyUpEventArgs args)
        {
            if (recieveInput && (args.Key == Key.X || args.Key == Key.Z))
                bindingContainer.TriggerReleased(bind);

            return base.OnKeyUp(state, args);
        }

        private string getBank(MixAction bind)
        {
            string bank = "n";

            if ((bind >= MixAction.DrumNormalLeft && bind <= MixAction.DrumClapLeft) || (bind >= MixAction.DrumNormalRight && bind <= MixAction.DrumClapRight))
                bank = "d";
            else if ((bind >= MixAction.SoftNormalLeft && bind <= MixAction.SoftClapLeft) || (bind >= MixAction.SoftNormalRight && bind <= MixAction.SoftClapRight))
                bank = "s";

            return bank;
        }

        private string getName(MixAction bind)
        {
            string name = "Normal";

            if (bind == MixAction.NormalWhistleLeft || bind == MixAction.NormalWhistleRight || bind == MixAction.DrumWhistleLeft || bind == MixAction.DrumWhistleRight || bind == MixAction.SoftWhistleLeft || bind == MixAction.SoftWhistleRight)
                name = "Whistle";
            if (bind == MixAction.NormalFinishLeft || bind == MixAction.NormalFinishRight || bind == MixAction.DrumFinishLeft || bind == MixAction.DrumFinishRight || bind == MixAction.SoftFinishLeft || bind == MixAction.SoftFinishRight)
                name = "Finish";
            if (bind == MixAction.NormalClapLeft || bind == MixAction.NormalClapRight || bind == MixAction.DrumClapLeft || bind == MixAction.DrumClapRight || bind == MixAction.SoftClapLeft || bind == MixAction.SoftClapRight)
                name = "Clap";

            return name;
        }
    }
}

