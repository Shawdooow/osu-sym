#region usings

using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;

#endregion

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections
{
    public class CreditsSection : WikiSection
    {
        public override string Title => "Credits";

        public CreditsSection()
        {
            Content.Add(new WikiParagraph("Credit where its due m80. These people helped get vitaru to where it is today, one way or another!"));

            Content.Add(new WikiSubSectionLinkHeader("Jorolf", "https://osu.ppy.sh/users/7004641", "View profile in browser"));
            Content.Add(new WikiParagraph("Started the code base, without Jorolf vitaru would not exist today."));

            Content.Add(new WikiSubSectionLinkHeader("Arrcival", "https://osu.ppy.sh/users/3782165", "View profile in browser"));
            Content.Add(new WikiParagraph("Helped early on with design choices and patterns."));

            Content.Add(new WikiSubSectionLinkHeader("ColdVolcano", "https://osu.ppy.sh/users/7492333", "View profile in browser"));
            Content.Add(new WikiParagraph("Helped with random things early on, helped move things along."));
        }
    }
}
