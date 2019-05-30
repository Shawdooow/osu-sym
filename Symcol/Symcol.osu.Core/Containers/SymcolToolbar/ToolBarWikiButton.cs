using osu.Game.Graphics;
using osu.Game.Overlays.Toolbar;

namespace Symcol.osu.Core.Containers.SymcolToolbar
{
    public class ToolBarWikiButton : ToolbarOverlayToggleButton
    {
        public ToolBarWikiButton()
        {
            SetIcon(FontAwesome.fa_question_circle);
            StateContainer = SymcolOsuModSet.WikiOverlay;
            TooltipMain = "Wiki";
            TooltipSub = "documenting the game, ya know?";
        }
    }
}
