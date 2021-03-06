﻿#region usings

using osu.Game.Graphics;
using osu.Game.Overlays.Toolbar;

#endregion

namespace osu.Core.Containers.SymcolToolbar
{
    public class ToolBarWikiButton : ToolbarOverlayToggleButton
    {
        public ToolBarWikiButton()
        {
            SetIcon(FontAwesome.fa_question_circle_o);
            StateContainer = SymcolOsuModSet.WikiOverlay;
            TooltipMain = "Wiki";
            TooltipSub = "Documenting the game, ya know?";
        }
    }
}
