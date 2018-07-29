using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.UI.Scrolling;

namespace osu.Game.Rulesets.Mix.UI
{
    public class Row : ScrollingPlayfield
    {
        public readonly int RowNumber;

        public Row(int row)
        {
            Direction.Value = ScrollingDirection.Left;
            RowNumber = row;

            Position = new Vector2(0, (MixPlayfield.DEFAULT_HEIGHT + 4) * (row - 1) + 80);

            RelativeSizeAxes = Axes.X;
            Height = MixPlayfield.DEFAULT_HEIGHT;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f,
                },
                new Box
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = 2,
                    Colour = Color4.White
                },
                new Box
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.TopCentre,
                    RelativeSizeAxes = Axes.X,
                    Height = 2,
                    Colour = Color4.White
                }
            };
        }
    }
}
