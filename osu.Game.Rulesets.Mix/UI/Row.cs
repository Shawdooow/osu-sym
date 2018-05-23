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

        public Row(int row) : base(ScrollingDirection.Left)
        {
            RowNumber = row;

            Position = new Vector2(0, (MixPlayfield.DEFAULT_HEIGHT * (row - 1)) + 8);

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
                    RelativeSizeAxes = Axes.X,
                    Height = 4,
                    Colour = Color4.White
                }
            };
        }
    }
}
