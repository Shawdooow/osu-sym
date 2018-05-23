using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => VitaruRuleset.RulesetVersion;

        protected override string RulesetStorage => "vitaru";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "-Tweak debug ui\n" +
            "-Fix Combo fire never reseting back to 0 on miss\n" +
            "-Give all patterns Non-Linear SpeedEasings (nerf spinners by making everything else hard)";
    }
}
