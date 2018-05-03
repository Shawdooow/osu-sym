using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;

namespace osu.Game.Rulesets.Mix.Objects.Drawables.Pieces
{
    public class Note : BeatSyncedContainer
    {
        private readonly CircularContainer circle;

        public Note(Color4 color)
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                circle = new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,

                    Masking = true,

                    BorderColour = Color4.White,
                    BorderThickness = 8,

                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = color,
                        Radius = 4,
                        Type = EdgeEffectType.Shadow
                    },

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = ColourInfo.GradientVertical(color.Darken(0.2f), color.Lighten(0.2f))
                        },
                        new Triangles
                        {
                            RelativeSizeAxes = Axes.Both,

                            TriangleScale = 1,
                            ColourLight = color.Lighten(0.3f),
                            ColourDark = color.Darken(0.3f)
                        }
                    }
                }
            };
        }
    }
}
