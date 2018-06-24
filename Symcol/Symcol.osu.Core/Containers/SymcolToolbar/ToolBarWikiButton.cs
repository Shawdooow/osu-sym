using osu.Game.Graphics;
using osu.Game.Overlays.Toolbar;

namespace Symcol.osu.Core.Containers.SymcolToolbar
{
    public class ToolBarWikiButton : ToolbarOverlayToggleButton
    {
        public ToolBarWikiButton()
        {
            SetIcon(FontAwesome.fa_chevron_circle_up);
            StateContainer = SymcolOsuModSet.WikiOverlay;
            TooltipMain = "Wiki Overlay";
            TooltipSub = "documenting the game, ya know?";
        }
    }
}
