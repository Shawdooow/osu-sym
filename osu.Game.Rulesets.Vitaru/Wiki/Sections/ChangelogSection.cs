using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => VitaruRuleset.RULESET_VERSION;

        protected override string RulesetStorage => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "-Updated to lazer version 2018.607.0\n" +
            "-Updated to lazer version 2018.606.0\n\n" +
            "Features:\n\n" +
            //"-New Patterns\n\n" +
            "\n\n" +
            "Tweaks / Changes:\n\n" +
            "-Adjust Touhosu playfield size + aspect ratio (make it bigger inline with TouhouSharp's playfield [doubled width])\n" +
            "-Patterns are per individual hitsound rather than one pattern each time hitsounds are played\n\n" +
            "Fixes:\n\n" +
            "-Fix hitsounding not being 1:1 with stable" +
            "-Fix boss death crashing game\n\n" + 
            "Additional Patch-Notes:\n\n" +
            "";
    }
}
