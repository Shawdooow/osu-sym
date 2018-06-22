using osu.Game.Rulesets.Classic.Wiki.Sections;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Classic.Wiki
{
    public class ClassicWikiOverlay : WikiOverlay
    {
        protected override WikiHeader Header => new ClassicWikiHeader();

        protected override WikiSection[] Sections => new WikiSection[]
            {
                new Gameplay()
            };
    }
}
