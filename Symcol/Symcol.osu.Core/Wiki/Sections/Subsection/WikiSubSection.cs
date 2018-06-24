using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using OpenTK;

namespace Symcol.osu.Core.Wiki.Sections.Subsection
{
    public abstract class WikiSubSection : FillFlowContainer
    {
        public abstract string Title { get; }

        public readonly WikiSubSectionHeader SubSectionHeaderText;

        private readonly FillFlowContainer content;

        protected override Container<Drawable> Content => content;

        protected WikiSubSection()
        {
            OsuColour osu = new OsuColour();
            Direction = FillDirection.Vertical;
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
            InternalChildren = new Drawable[]
            {
                SubSectionHeaderText = new WikiSubSectionHeader
                {
                    Colour = osu.Yellow,
                    Text = Title,
                    TextSize = 32,
                    Font = @"Exo2.0-Bold",
                    Margin = new MarginPadding
                    {
                        Horizontal = WikiOverlay.CONTENT_X_MARGIN,
                        Vertical = 12
                    }
                },
                content = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Padding = new MarginPadding
                    {
                        Horizontal = WikiOverlay.CONTENT_X_MARGIN,
                        Bottom = 20
                    }
                },
                new Box
                {
                    RelativeSizeAxes = Axes.X,
                    Height = 1,
                    Colour = OsuColour.Gray(34),
                    EdgeSmoothness = Vector2.One
                }
            };
        }
    }
}
