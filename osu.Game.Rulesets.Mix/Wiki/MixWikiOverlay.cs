using osu.Game.Rulesets.Mix.Wiki.Sections;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Mix.Wiki
{
    public class MixWikiOverlay : WikiOverlay
    {
        protected override WikiHeader Header => new MixWikiHeader();

        protected override WikiSection[] Sections => new WikiSection[]
            {
                new Gameplay()
            };
    }
}
