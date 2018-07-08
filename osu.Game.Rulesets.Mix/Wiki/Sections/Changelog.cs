using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Mix.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string Version => MixRuleset.RulesetVersion;

        protected override string StoragePath => "mix";

        protected override string FileExtention => ".mix";

        protected override string VersionChangelog => "-Add Changelog section to wiki\n" +
            "";
    }
}
