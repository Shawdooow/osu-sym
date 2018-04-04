using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class MultiplayerSection : WikiSection
    {
        public override string Title => "Multiplayer";

        public MultiplayerSection()
        {
            Content.Add(new WikiParagraph("Vitaru comes equiped with both online and offline multiplayer (with bots, split screen shall appear in the future)."));
            Content.Add(new WikiSubSectionHeader("Offline"));
            Content.Add(new WikiParagraph("Currently Offline is in very early stages of development and the ai doesn't know how to use all the spells. " +
                "I would like to fix this for release but no promises."));
            Content.Add(new WikiSubSectionHeader("Online"));
            Content.Add(new WikiParagraph("Vitaru has wicked buggy online multiplayer (which requires the osu.Game Symcol mods). " +
                "The lobby doesnt work right, packet sharing is still buggy in game, and finishing a map breaks the connection to host. " +
                "There is plenty that needs work, don't expect it to work perfectly quite yet, though a quick mention in the vitaru dev channel if you find something would be nice. " +
                "And as a final \"this is buggy af\" disclaimer, there is no packet loss compensation in place, so missing packets may or may not break eveything. " +
                "Plans to deal with this effectivly are in the works, especially trying to keep them in order.\n\n" +
                "Spells are also destined to break the game, especially the time shifters, however this will be fixed (soon?)."));
        }
    }
}
