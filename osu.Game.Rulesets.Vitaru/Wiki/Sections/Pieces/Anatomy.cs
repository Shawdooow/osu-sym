using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Core.GameObjects;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections.Pieces
{
    /// <summary>
    /// The insides of the player, only used by the wiki
    /// </summary>
    public class Anatomy : Container
    {
        private readonly Sprite characterSprite;
        private readonly CircularContainer hitbox;
        private EdgeEffectParameters edgeEffectParameters;

        public Anatomy()
        {
            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;
            AutoSizeAxes = Axes.Both;

            edgeEffectParameters = new EdgeEffectParameters
            {
                Radius = 4,
                Type = EdgeEffectType.Shadow
            };

            Children = new Drawable[]
            {
                characterSprite = new Sprite
                {
                    Scale = new Vector2(2),
                    Position = new Vector2(-10, 0),
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight
                },
                hitbox = new CircularContainer
                {
                    Position = new Vector2(-4, 0),
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(2),
                    Size = new Vector2(4),
                    BorderThickness = 4 / 3,
                    Masking = true,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    EdgeEffect = edgeEffectParameters
                }
            };

            characterSprite.Texture = VitaruRuleset.VitaruTextures.Get(SelectableCharacters.SakuyaIzayoi.ToString() + "Kiai");
            hitbox.BorderColour = Color4.Navy;
            edgeEffectParameters.Colour = Color4.Navy.Opacity(0.5f);
        }
    }
}
