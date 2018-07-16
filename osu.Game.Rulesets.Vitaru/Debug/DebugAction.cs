using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Game.Graphics;
using System;
using Symcol.Core.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Debug
{
    public class DebugAction : SymcolClickableContainer
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

        private readonly bool adminRequired;

        private SpriteText text;

        public DebugAction(bool adminRequired = true)
        {
            this.adminRequired = adminRequired;
            load();
        }

        public DebugAction(Action action, bool adminRequired = true)
        {
            this.adminRequired = adminRequired;
            load(action);
        }

        private void load(Action action = null)
        {
            Action = action;

            OsuColour osu = new OsuColour();

            Masking = true;
            CornerRadius = 4;

            RelativeSizeAxes = Axes.X;
            Height = 24;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f
                },
                text = new SpriteText
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    TextSize = 18,
                    Colour = osu.Red
                }
            };
        }

        protected override bool OnClick(InputState state)
        {
            if (adminRequired && !VitaruAPIContainer.Admin)
                return false;

            return base.OnClick(state);
        }
    }
}
