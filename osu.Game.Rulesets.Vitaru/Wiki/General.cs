using Symcol.osu.Core.Wiki.Sections;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;
using Symcol.osu.Core.Wiki.Sections.Subsection;

namespace osu.Game.Rulesets.Vitaru.Wiki
{
    public class General : WikiSection
    {
        public override string Title => "General";

        public override WikiSubSection[] GetSubSections() => new WikiSubSection[]
        {
            new SubSections.Difficulty(), 
        };
    }
}
