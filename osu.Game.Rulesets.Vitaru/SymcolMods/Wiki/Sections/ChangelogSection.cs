using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Vitaru.SymcolMods.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string Version => VitaruRuleset.RULESET_VERSION;

        protected override string StoragePath => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "-Updated to lazer version 2018.728.0\n" +
            "-Updated to lazer version 2018.710.0\n" +
            "-Updated to lazer version 2018.702.0\n" +
            "-Updated to lazer version 2018.619.0\n\n" +
            "Features:\n\n" +
            //TODO: The new patterns!
            //"-New Patterns\n\n" +
            "-AutoV3\n" +
            "-Implement Remilia's ability: Vampuric\n" +
            "-Implement Aya's ability: Snapshot\n\n" +
            "Tweaks / Changes:\n\n" +
            "-Swap to the new skin\n" +
            "-\"Spells\" are now called \"Abilities\"\n" +
            "-Allow characters to specify additional ability stats\n" +
            "-Ryukoy's ability heavily reworked\n" +
            "-Removed dependency on osu.Game hacks and will now utilize the new \"Symcol Modloader\" system in place. " +
            "As such this ruleset is now a \"Mod Enhanced\" ruleset.\n" +
            "-Adjusted all pattern easings (and increased base pattern speed by 25% so most patterns seem to travel at the same speed as before, " +
            "now they will change speed less)\n\n" +
            "Fixes:\n\n" +
            "\n\n" + 
            "Dev Notes:\n\n" +
            "   Ryukoy was just designed poorly, she was either gonna be way too powerful or absolute garbage, " +
            "hopefully the new changes will allow her to find a middle ground where she is boh fun to play and balanced.\n" +
            "   Removing the osu.Game hacks dependency was on my list of things todo for awhile and I am glad that its finally been done, " +
            "however not all is sunshine and rainbows. Unfortunetly this now means the wiki is now a Modloader exclusive feature " +
            "making accessing imporant information about the ruleset all the harder for new players.";
    }
}
