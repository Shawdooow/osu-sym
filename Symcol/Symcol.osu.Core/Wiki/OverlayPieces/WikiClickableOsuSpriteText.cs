using osu.Game.Graphics;
using Symcol.osu.Core.Containers.Text;

namespace Symcol.osu.Core.Wiki.OverlayPieces
{
    public class WikiClickableOsuSpriteText : ClickableOsuSpriteText
    {
        public WikiClickableOsuSpriteText()
        {
            OsuColour osu = new OsuColour();
            Colour = osu.Pink;
            HoverContainer.HoverColour = osu.Blue;
        }
    }
}
