using osu.Framework.Graphics;
using osu.Game.Graphics;
using Symcol.osu.Core.Containers.Text;

namespace Symcol.osu.Core.Wiki.Sections.SectionPieces
{
    public class WikiSubSectionLinkHeader : LinkOsuSpriteText
    {
        public WikiSubSectionLinkHeader(string text, string url, string tooltip = "")
        {
            Tooltip = tooltip;
            Url = url;
            OsuColour osu = new OsuColour();
            Colour = osu.Pink;
            Text = text;
            TextSize = 24;
            Font = @"Exo2.0-BoldItalic";
            Margin = new MarginPadding
            {
                Vertical = 10
            };
        }
    }
}
