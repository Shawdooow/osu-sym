using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Mix.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => MixRuleset.RulesetVersion;

        protected override string RulesetStorage => "mix";

        protected override string FileExtention => ".mix";

        protected override string VersionChangelog => "-Add Changelog section to wiki\n" +
            "";
    }
}
