using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class AspectLockedPlayfield : Container
    {
        public override Vector2 Size => VitaruPlayfield.BaseSize;

        public new float Margin = 0.8f;

        protected virtual Vector2 AspectRatio => new Vector2(20, 16);

        public AspectLockedPlayfield()
        {
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
