using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => VitaruRuleset.RULESET_VERSION;

        protected override string RulesetStorage => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "-Updated to lazer version 2018.619.0\n\n" +
            "Features:\n\n" +
            //"-New Patterns\n\n" +
            "\n\n" +
            "Tweaks / Changes:\n\n" +
            "\n\n" +
            "Fixes:\n\n" +
            "\n\n" + 
            "Dev Notes:\n\n" +
            "";
    }
}
