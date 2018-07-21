using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.GameObjects;
using Symcol.Core.Graphics.Containers;
// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities
{
    public class Camera : SymcolContainer
    {
        public Box CameraBox;
        public SymcolHitbox Hitbox;

        private readonly OsuSpriteText xPos;
        private readonly OsuSpriteText yPos;

        private readonly OsuSpriteText xSize;
        private readonly OsuSpriteText ySize;

        public Camera()
        {
            Origin = Anchor.Centre;
            Size = new Vector2(200, 120);

            Children = new Drawable[]
            {
                Hitbox = new SymcolHitbox
                {
                    RelativeSizeAxes = Axes.Both,
                    AlwaysPresent = true,
                    Alpha = 0
                },
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
                xPos = new OsuSpriteText
                {
                    Position = new Vector2(-8, 8),
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Font = "Venera",
                    TextSize = 12,
                    Alpha = 0.75f
                },
                yPos = new OsuSpriteText
                {
                    Position = new Vector2(-8),
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Font = "Venera",
                    TextSize = 12,
                    Alpha = 0.75f
                },
                xSize = new OsuSpriteText
                {
                    Position = new Vector2(8),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Font = "Venera",
                    TextSize = 12,
                    Alpha = 0.75f
                },
                ySize = new OsuSpriteText
                {
                    Position = new Vector2(8, -8),
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Font = "Venera",
                    TextSize = 12,
                    Alpha = 0.75f
                },
            };
        }

        protected override void Update()
        {
            base.Update();

            xPos.Text = "x: " + (int)CameraBox.ScreenSpaceDrawQuad.TopLeft.X;
            yPos.Text = "y: " + (int)CameraBox.ScreenSpaceDrawQuad.TopLeft.Y;
            xSize.Text = "w: " + (int)CameraBox.ScreenSpaceDrawQuad.Width;
            ySize.Text = "h: " + (int)CameraBox.ScreenSpaceDrawQuad.Height;
        }

        private class Corner : SymcolContainer
        {
            internal const int height = 5;
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
