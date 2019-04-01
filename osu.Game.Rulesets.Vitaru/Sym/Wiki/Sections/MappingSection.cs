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
            Content.Add(new WikiParagraph("There are many approaches to mapping vitaru because of both gamemode diversity and customizability of patterns and bullets. " +
                "I am going to try and give a starting point for someone who has read the editor section and has a general idea of mapping in general."));

            Content.Add(new WikiSubSectionHeader("Important Notes"));
            Content.Add(new WikiParagraph("Because vitaru is more of a \"passive\" ruleset like osu!catch (you don't have to play to get score) "
                                          + "hitsounding is very important when compared to the more\"active\" rulesets and can make or break your map. "
                                          + "becasue of this vitaru will include some extra hitsounds and hitsounding options out of the box "
                                          + "for you to use and help you get that perfect noise to match your pattern."));

            Content.Add(new WikiSubSectionHeader("Ways to reperesent the Music"));
            Content.Add(new WikiParagraph("Creating custom patterns is a great way to do this, but not a neccesity. "
                                          + "Most of the time you may find that sharp / high pitched sounds in the music can be reflected best by small, fast patterns with few bullets. "
                                          + "Low pitched or \"bass\" noises go nicely with slow and thick patterns, many fat bullets that move more or less in sync with the sound "
                                          + "(if the sound takes a while to taper off than drag out the pattern a bit to match that).\n\n"

                                          + "Lasers are kind of weird..."));

            Content.Add(new WikiSubSectionHeader("Ranking"));
            Content.Add(new WikiParagraph("The ranking proccess will be very similar to osu! standard back in 2007, "
                                          + "where you ask someone qualified to qualify your map and they take a look and either qualify it or tell you why they won't qualify it."));

            Content.Add(new WikiSubSectionHeader("Ranking Rules"));
            Content.Add(new WikiParagraph("There are rules and hard limits as to what is rankable, although they will be kept to a minimum as to not stent creativity. "
                                          + "In addition to the \"obvious\" stuff like correct timing and acceptable metadata "
                                          + "(that usually apply to all maps regardless of ruleset) you're map must meet the following criteria:\n\n"

                                          + "Criteria 1: Your map must be passable without getting hit at all (if it is a touhosu map this must be possible WITHOUT use of spells / abilities). "
                                          + "I know converts don't always follow this rule but there isn't much that can be done about that.\n"
                                          + "Criteria 2: If your map uses custom bullets they must be checked to be visually accurate enough to an extent without destroying computers "
                                          + "(if this even is still getting implemented).\n\n"

                                          + "Congrats, if your map meets all the above ranking criteria there is a good chance your map can be qualified then ranked! "
                                          + "If you would like some friendly advice I would advise you to get as many mods as possible before a \"[Qualification] Review\" as it will be called. "
                                          + "A qualifier is much less likely to bring up a problem if they see it has already been addressed before in some manor, "
                                          + "allowing you to get qualified more quickly."));
        }
    }
}
