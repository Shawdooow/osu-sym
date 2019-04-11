#region usings

using osu.Core.Containers.Text;
using osu.Game.Graphics;

#endregion

namespace osu.Core.Wiki.OverlayPieces
{
    public class WikiClickableOsuSpriteText : ClickableOsuSpriteText
    {
        public WikiClickableOsuSpriteText()
        {
            OsuColour osu = new OsuColour();
            IdleColour = osu.Pink;
            HoverContainer.HoverColour = osu.Blue;
        }
    }
}
