using OpenTK.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Vitaru.Debug
{
    public class DebugStat<T> : Container
        where T : struct
    {
        public readonly Bindable<T> Bindable;

        public string Text
        {
            get
            {
                return valueName;
            }
            set
            {
                if (value != valueName)
                {
                    valueName = value;
                }
            }
        }

        private SpriteText text;

        private string valueName;

        public DebugStat(Bindable<T> bindable)
        {
            Bindable = bindable;

            AutoSizeAxes = Axes.Both;

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
                    TextSize = 20
                }
            };

            Bindable.ValueChanged += (value) =>
            {
                text.Text = valueName + " = " + value.ToString();
            };
        }
    }
}
