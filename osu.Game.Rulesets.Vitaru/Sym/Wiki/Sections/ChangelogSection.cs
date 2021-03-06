﻿#region usings

using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;
using osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections.Changelog;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections
{
    public class ChangelogSection : WikiSection
    {
        public override string Title => "Changelog";

        private readonly ChangelogVersion[] versions =
        {
            new _0_10_0(),

            new _0_9_0(),

            new _0_8_6(),
            new _0_8_5(),
            new _0_8_4(),
            new _0_8_3(),
            new _0_8_2(),
            new _0_8_1(),
            new _0_8_0(),

            new _0_7_7(),
            new _0_7_6(),
            new _0_7_0(),

            new _0_6_2(),
            new _0_6_1(),
            new _0_6_0(),

            new _0_5_0(),

            new _0_4_0(),

            new _0_3_0(),

            new _0_2_0(),

            new _0_1_1(),
            new _0_1_0(),

            new _0_0_4(),
            new _0_0_3(), 
            new _0_0_2(), 
            new _0_0_1(), 
            new _0_0_0(),
            new Negative_0_0_1(), 
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
