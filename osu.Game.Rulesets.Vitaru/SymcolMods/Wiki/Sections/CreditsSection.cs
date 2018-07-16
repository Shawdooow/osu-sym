﻿using Symcol.osu.Core.Wiki.Sections;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;
using Symcol.osu.Core.Wiki.Sections.Subsection;

namespace osu.Game.Rulesets.Vitaru.SymcolMods.Wiki.Sections
{
    public class CreditsSection : WikiSection
    {
        public override string Title => "Credits";

        public CreditsSection()
        {
            Content.Add(new WikiParagraph("A place of thanks, because these people helped get vitaru where it is today one way or another!"));
            Content.Add(new WikiSubSectionLinkHeader("Jorolf", "https://osu.ppy.sh/users/7004641", "View profile in browser"));
            Content.Add(new WikiParagraph("Started the code base, without Jorolf vitaru would not exist today."));
            Content.Add(new WikiSubSectionLinkHeader("Arrcival", "https://osu.ppy.sh/users/3782165", "View profile in browser"));
            Content.Add(new WikiParagraph("Helped early on with design choices and patterns."));
            Content.Add(new WikiSubSectionLinkHeader("ColdVolcano", "https://osu.ppy.sh/users/7492333", "View profile in browser"));
            Content.Add(new WikiParagraph("Helped with random things early on, helped move things along."));
        }
    }
}