#region usings

using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Overlays.Toolbar;

#endregion

namespace osu.Core.Containers.SymcolToolbar
{
    public class ToolBarWikiButton : ToolbarOverlayToggleButton
    {
        public ToolBarWikiButton()
        {
            SetIcon(FontAwesome.Solid.QuestionCircle);
            StateContainer = SymcolOsuModSet.WikiOverlay;
            TooltipMain = "Wiki";
            TooltipSub = "Documenting the game, ya know?";
        }
    }
}
