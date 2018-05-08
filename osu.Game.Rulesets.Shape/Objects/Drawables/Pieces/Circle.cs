using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Shape.Objects.Drawables.Pieces
{
    public class ShapeCircle : Container
    {
        private BaseShape shape;
        private Container ring;

        public ShapeCircle(BaseShape Shape)
        {
            shape = Shape;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Size = new Vector2(shape.ShapeSize * 1.25f);

            Origin = Anchor.Centre;
            Anchor = Anchor.Centre;

            Children = new Drawable[]
            {
                new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Texture = ShapeRuleset.ShapeTextures.Get("circle")
                }
            };
        }
    }
}
