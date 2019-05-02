#region usings

using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables.Pieces
{
    public class LaserPiece : BeatSyncedContainer
    {
        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.Get<GraphicsOptions>(VitaruSetting.LaserVisuals);

        public readonly Box Box;

        public LaserPiece(Color4 accent, float width)
        {
            Anchor = Anchor.BottomCentre;
            Origin = Anchor.BottomCentre;

            Masking = true;
            RelativeSizeAxes = Axes.Both;

            if (graphics != GraphicsOptions.HighPerformance)
                EdgeEffect = new EdgeEffectParameters
                {
                    Radius = width,
                    Type = EdgeEffectType.Shadow,
                    Colour = accent.Opacity(graphics != GraphicsOptions.Old ? 0.5f : 0.2f)
                };

            if (graphics == GraphicsOptions.Old)
            {
                CornerRadius = width / 4;
                BorderThickness = 8;
                BorderColour = accent;
            }

            Child = Box = new Box
            {
                RelativeSizeAxes = Axes.Both
            };
        }
    }
}
