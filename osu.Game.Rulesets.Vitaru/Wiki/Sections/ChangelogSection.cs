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
            "-Added an option that 2x to 10x performance depending on map (when not in editor + its unranked)\n" +
            //"-New Patterns\n\n" +
            "\n\n" +
            "Tweaks / Changes:\n\n" +
            "-Adjust Touhosu playfield size + aspect ratio (make it bigger inline with TouhouSharp's playfield [doubled width])\n" +
            "-Patterns are per individual hitsound rather than one pattern each time hitsounds are played (basically there are gonna be way more bullets now)\n\n" +
            "Fixes:\n\n" +
            "-Fix hitsounding not being 1:1 with stable\n" +
            "-Fix boss death crashing game\n\n" + 
            "Dev Notes:\n\n" +
            "Generally speaking now the more beautiful a map's hitsounds the more likely it is to kill you. " +
            "I also was tinkering with a wider playfield and thought it would be a welcome change in Touhosu with the new patterns that are planned." +
            "";
    }
}
