using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Sprites;
using OpenTK;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;
using Symcol.osu.Core.Wiki.Sections.Subsection;

namespace Symcol.osu.Core.Wiki.Sections
{
    public abstract class WikiSection : FillFlowContainer
    {
        public abstract string Title { get; }

        public virtual string Overview { get; }

        public virtual WikiSubSection[] GetSubSections() => null;

        protected readonly OsuSpriteText SectionHeaderText;

        protected override Container<Drawable> Content => content;

        private readonly FillFlowContainer content;

        protected WikiSection()
        {
            OsuColour osu = new OsuColour();
            Direction = FillDirection.Vertical;
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
            InternalChildren = new Drawable[]
            {
                SectionHeaderText = new OsuSpriteText
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

            if (Overview != null)
                Content.Add(new WikiParagraph(Overview));

            if (GetSubSections() != null)
                foreach (WikiSubSection subSection in GetSubSections())
                    Content.Add(subSection);
        }
    }
}
