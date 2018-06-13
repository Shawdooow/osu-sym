using osu.Game.Graphics;
using osu.Game.Online.API;

namespace osu.Game.Overlays.Toolbar
{
    public class ToolbarServerSwitcher : ToolbarButton
    {
        private bool symcol;

        public ToolbarServerSwitcher()
        {
            Action += () =>
            {
                symcol = !symcol;

                if (symcol)
                {
                    APIAccess.Endpoint = @"10.0.0.25";
                    Icon = FontAwesome.fa_chevron_circle_down;
                }
                else
                {
                    APIAccess.Endpoint = @"https://osu.ppy.sh";
                    Icon = FontAwesome.fa_chevron_circle_up;
                }
            };

            Icon = FontAwesome.fa_chevron_circle_up;
            TooltipMain = "Server Switcher";
            TooltipSub = "Swap between osu! and Symcol servers!";
        }
    }
}
