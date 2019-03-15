using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osuTK.Graphics;
using Sym.Base.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Debug
{
    public class DebugStat<T> : SymcolContainer
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
                        SpriteText.Text = value;
                }
            }
        }

        public readonly SpriteText SpriteText;

        private string valueName;

        public DebugStat(Bindable<T> bindable)
        {
            Bindable = bindable;

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
                SpriteText = new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    TextSize = 18,
                    Colour = osu.Blue
                }
            };

            if (Bindable != null)
                Bindable.ValueChanged += value =>
                {
                    SpriteText.Text = valueName + " = " + value.ToString();
                };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            Bindable.TriggerChange();
        }
    }
}
