using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using System;

namespace osu.Game.Rulesets.Vitaru.Debug
{
    public class DebugAction : ClickableContainer
    {
        public string Text
        {
            get
            {
                return text.Text;
            }
            set
            {
                if (value != text.Text)
                {
                    text.Text = value;
                }
            }
        }

        private SpriteText text;

        public DebugAction(Action action = null)
        {
            Action = action;

            OsuColour osu = new OsuColour();

            Masking = true;
            CornerRadius = 4;

            BorderColour = Color4.White;
            BorderThickness = 4;

            RelativeSizeAxes = Axes.X;
            Height = 20;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = osu.Red
                },
                text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextSize = 20
                }
            };
        }
    }
}
