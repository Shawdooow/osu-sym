using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.Graphics.Containers;
// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities
{
    public class Camera : SymcolContainer
    {
        public Box CameraBox;

        public Camera()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(200, 120);

            Children = new Drawable[]
            {
                CameraBox = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                },
                new Corner(),
                new Corner
                {
                    Anchor = Anchor.TopRight,
                    Rotation = 90
                },
                new Corner
                {
                    Anchor = Anchor.BottomRight,
                    Rotation = 180
                },
                new Corner
                {
                    Anchor = Anchor.BottomLeft,
                    Rotation = 270
                },
            };
        }

        private class Corner : SymcolContainer
        {
            internal const int height = 6;
            internal const int width = 16;

            internal Corner()
            {
                Children = new Drawable[]
                {
                    new Box
                    {
                        Size = new Vector2(width, height),
                        Colour = Color4.White
                    },
                    new Box
                    {
                        Size = new Vector2(height, width),
                        Colour = Color4.White
                    }
                };
            }
        }
    }
}
