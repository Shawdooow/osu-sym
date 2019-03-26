#region usings

using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables.Pieces
{
    public class StarPiece : Container
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
