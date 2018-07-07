using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => VitaruRuleset.RULESET_VERSION;

        protected override string RulesetStorage => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "-Updated to lazer version 2018.702.0\n" +
            "-Updated to lazer version 2018.619.0\n\n" +
            "Features:\n\n" +
            //TODO: The new patterns!
            //"-New Patterns\n\n" +
            "\n\n" +
            "Tweaks / Changes:\n\n" +
            "-Removed Ryukoy's spell in preperation for a heavy rework\n" +
            "-Removed dependency on osu.Game hacks and will now utilize the new \"Symcol Modloader\" system in place. " +
            "As such this ruleset is now a \"Mod Enhanced\" ruleset.\n\n" +
            "Fixes:\n\n" +
            "\n\n" + 
            "Dev Notes:\n\n" +
            "   Ryukoy was just designed poorly, she was either gonna be way too powerful or absolute garbage, " +
            "hopefully the planned changes will allow her to find a middle ground where she is boh fun to play and balanced.\n" +
            "   Removing the osu.Game hacks dependency was on my list of things todo for awhile and I am glad that its finally been done, " +
            "however not all is sunshine and rainbows. Unfortunetly this now means the wiki is now a Modloader exclusive feature " +
            "making accessing imporant information about the ruleset all the harder for new players.";
    }
}
