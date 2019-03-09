using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osuTK;
using Symcol.Base.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces
{
    public class StarPiece : SymcolContainer
    {
        public StarPiece()
        {
            Size = new Vector2(64);

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
