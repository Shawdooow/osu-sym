using osu.Game.Rulesets.Shape.Wiki.Sections;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Shape.Wiki
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
