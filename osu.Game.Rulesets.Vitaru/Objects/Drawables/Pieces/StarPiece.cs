using OpenTK;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces
{
    public class StarPiece : Container
    {
        public StarPiece()
        {
            Size = new Vector2(30);

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            InternalChild = new Sprite
            {
                RelativeSizeAxes = Axes.Both,
                Texture = VitaruRuleset.VitaruTextures.Get("star")
            };
        }
    }
}
