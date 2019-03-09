using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces
{
    public class LaserPiece : BeatSyncedContainer
    {
        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.LaserVisuals);

        public readonly Box Box;

        public LaserPiece(DrawableLaser drawableLaser)
        {
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            Masking = true;
            RelativeSizeAxes = Axes.Both;

            if (graphics != GraphicsOptions.HighPerformance)
                EdgeEffect = new EdgeEffectParameters
                {
                    Radius = drawableLaser.HitObject.Width,
                    Type = EdgeEffectType.Shadow,
                    Colour = drawableLaser.AccentColour.Opacity(graphics != GraphicsOptions.Old ? 0.5f : 0.2f)
                };

            if (graphics == GraphicsOptions.Old)
            {
                CornerRadius = drawableLaser.HitObject.Width / 4;
                BorderThickness = 8;
                BorderColour = drawableLaser.AccentColour;
            }

            Child = Box = new Box
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
