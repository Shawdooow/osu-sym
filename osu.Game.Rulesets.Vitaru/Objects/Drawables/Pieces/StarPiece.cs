using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using Symcol.Core.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces
{
    public class StarPiece : SymcolContainer
    {
        public override bool HandleMouseInput => false;
        public override bool HandleKeyboardInput => false;

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
