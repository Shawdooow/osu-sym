using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class MultiplayerSection : WikiSection
    {
        public override string Title => "Multiplayer";

        public MultiplayerSection()
        {
            Content.Add(new WikiParagraph("Vitaru comes equiped with both online and offline multiplayer (offline with bots*, \"split screen\" shall appear in the future)."));
            Content.Add(new WikiSubSectionHeader("Offline (Currently Disabled for Updates)"));
            Content.Add(new WikiParagraph("Currently Offline is in very early stages of development."));
            Content.Add(new WikiSubSectionHeader("Online (Currently Disabled for Updates)"));
            Content.Add(new WikiParagraph("Vitaru now comes equiped with online multiplayer (provided you have the required osu.Game Symcol mods)."));
        }
    }
}
