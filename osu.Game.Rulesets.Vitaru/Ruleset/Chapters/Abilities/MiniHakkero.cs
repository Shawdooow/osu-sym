using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Rational.Characters.Drawables;
using osuTK.Graphics;
using Sym.Base.Game;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Abilities
{
    public class MiniHakkero : Container
    {
        public readonly Hitbox Hitbox;

        private readonly Container red;
        private readonly Container green;
        private readonly Container blue;

        public MiniHakkero(DrawableMarisa drawableMarisa)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.BottomCentre;

            Children = new Drawable[]
            {
                Hitbox = new Hitbox(Shape.Rectangle)
                {
                    HitDetection = false
                },
                red = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Masking = true,
                    Colour = Color4.Red,
                    Alpha = 0.25f,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                },
                green = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Masking = true,
                    Colour = Color4.Green,
                    Alpha = 0.25f,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                },
                blue = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Masking = true,
                    Colour = Color4.Blue,
                    Alpha = 0.25f,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                },
            };
        }

        public override void Show()
        {
            this.FadeIn(100);
        }

        public override void Hide()
        {
            this.FadeOut(100);
        }
    }
}
