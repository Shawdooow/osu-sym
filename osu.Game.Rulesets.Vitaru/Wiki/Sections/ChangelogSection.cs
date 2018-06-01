using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => VitaruRuleset.RulesetVersion;

        protected override string RulesetStorage => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "" +
            "Features:\n\n" +
            "\n\n" +
            "Tweaks / Changes:\n\n" +
            "\n\n" +
            "Fixes:\n\n" +
            "-Fix boss death crashing game\n\n" + 
            "Additional Patch-Notes:\n\n" +
            "";
    }
}
