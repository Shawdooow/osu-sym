using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using Symcol.Core.Graphics.Containers;

namespace Symcol.osu.Core.Wiki.Header
{
    public class WikiHeader : Container
    {
        private const float icon_size = 200;
        private const float header_margin = 50;
        private const float rulesetname_height = 60;

        public WikiHeader()
        {
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = header_margin + icon_size + rulesetname_height;
        }

        private class HeaderBackButton : SymcolClickableContainer
        {
            public HeaderBackButton()
            {

            }
        }
    }
}
