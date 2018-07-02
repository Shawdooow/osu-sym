using Symcol.osu.Core.Wiki.Sections;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;
using Symcol.osu.Core.Wiki.Sections.Subsection;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class MultiplayerSection : WikiSection
    {
        public override string Title => "Multiplayer";

        public MultiplayerSection()
        {
            Content.Add(new WikiParagraph("Vitaru comes equiped with both online and offline multiplayer."));
            Content.Add(new WikiSubSectionHeader("Offline (Bots)"));
            Content.Add(new WikiParagraph("Playing offline with bots is implemented as both a way to test new / custom characters and as a way to access multiplayer only content without friends."));
            Content.Add(new WikiSubSectionHeader("Offline (Split screen)"));
            Content.Add(new WikiParagraph("\"Split screen\" currently doesn't exist and it won't actually split the screen. " +
                "Instead both players will be on the same field and work together to defeat the hordes of musical enemies (or try and kill eachother when kiai battles get implemented)."));
            Content.Add(new WikiSubSectionHeader("Online"));
            Content.Add(new WikiParagraph("Vitaru now comes equiped with online multiplayer (provided you have the required osu.Game Symcol mods). " +
                "Currently only Co-op works but more modes are planned."));
        }
    }
}
