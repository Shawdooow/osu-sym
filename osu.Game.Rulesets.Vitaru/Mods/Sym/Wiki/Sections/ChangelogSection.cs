using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;
using osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections.Changelog;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections
{
    public class ChangelogSection : WikiSection
    {
        public override string Title => "Changelog";

        private readonly ChangelogVersion[] versions = new ChangelogVersion[]
        {
            new _0_10_0(),
        };

        public ChangelogSection()
        {
            for (int i = 0; i < versions.Length; i++)
            {
                ChangelogVersion v = versions[i];

                //TODO: Implement a dropdown to save on loading times and memory
                if (v.VersionTitle != null)
                    Content.Add(new WikiSubSectionHeader($"{v.VersionNumber} - {v.VersionTitle}"));
                else
                    Content.Add(new WikiSubSectionHeader($"{v.VersionNumber}"));

                Content.Add(new WikiParagraph(v.GetChangelog()));
            }
        }
    }
}
