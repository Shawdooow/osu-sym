#region usings

using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;
using osu.Framework.Allocation;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections
{
    public class MappingSection : WikiSection
    {
        public override string Title => "Mapping";

        [BackgroundDependencyLoader]
        private void load()
        {
            Content.Add(new WikiParagraph("As stated in the editor section there are two configurations, however there are many approaches to mapping vitaru because of both gamemode diversity and customizability of patterns and bullets. " +
                "I am going to try and give a starting point for someone who has read the editor section and has a general idea of mapping in general."));
            Content.Add(new WikiSubSectionHeader("Ways to reperesent the Music"));
            Content.Add(new WikiParagraph(""));
            Content.Add(new WikiSubSectionHeader("Ranking"));
            Content.Add(new WikiParagraph("The ranking proccess will be very similar to osu! standard back in 2007, where you ask someone qualified to qualify your map and they take a look and either qualify it or tell you why they won't qualify it."));
            Content.Add(new WikiSubSectionHeader("Ranking Rules"));
            Content.Add(new WikiParagraph("There are rules and hard limits as to what is rankable, although they will be kept to a minimum as to not stent creativity. " +
                "In addition to the \"obvious\" stuff like correct timing and acceptable metadata (that usually apply to all maps regardless of ruleset) you're map must meet the following criteria:\n\n" +
                        "Criteria 1: Your map must be passable without getting hit at all. I know converts don't always follow this rule but there isn't much that can be done about that.\n" +
                        "Criteria 2: If your map uses custom bullets they must be checked to be visually accurate enough to an extent without destroying computers.\n" +
                        "Criteria 3: If your map breaks the game todo something awesome, as long as if fits  the song rythmeticaly you're probably all set.\n\n" +
                        "Congrats, if your map meets all the above ranking criteria there is a good chance your map can be qualified then ranked! " +
                        "If you would like some friendly advice I would advise you to get as many mods as possible before a \"[Qualification] Review\" as we will call it. " +
                        "If a qualifier has a question about something but sees you have already explained why you think it works and agrees then there will be no need to bring it back up, " +
                        "potentially making the proccess much faster."));
        }
    }
}
