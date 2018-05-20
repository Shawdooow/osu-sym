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

                    if (Bindable == null)
                        text.Text = value;
                }
            }
        }

        private SpriteText text;

        private string valueName;

        public DebugStat(Bindable<T> bindable)
        {
            Bindable = bindable;

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
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    TextSize = 18
                }
            };

            if (Bindable != null)
                Bindable.ValueChanged += (value) =>
                {
                    text.Text = valueName + " = " + value.ToString();
                };
        }
    }
}
