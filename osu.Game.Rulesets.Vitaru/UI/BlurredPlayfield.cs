using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class BlurredPlayfield : Container<Drawable>
    {
        public override Vector2 Size => VitaruPlayfield.BaseSize;

        public new virtual float Margin => 0.8f;

        public virtual Vector2 AspectRatio => new Vector2(20, 16);

        public BlurredPlayfield()
        {
            Name = "BlurredPlayfield";
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Scale = new Vector2(Parent.DrawSize.Y * AspectRatio.X / AspectRatio.Y / Size.X, Parent.DrawSize.Y / Size.Y) * Margin;
        }
    }
}
