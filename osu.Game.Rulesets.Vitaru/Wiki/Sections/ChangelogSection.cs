using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string RulesetVersion => VitaruRuleset.RulesetVersion;

        protected override string RulesetStorage => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

        protected override string VersionChangelog => "-Updated to lazer version 2018.531.0\n" +
            "-Updated to lazer version 2018.526.0\n\n" +
            "Features:\n\n" +
            "-Implement Reimu's Spell \"Leader\"\n\n" +
            "Tweaks / Changes:\n\n" +
            "-Tweak Reimu's stats based on now implemented spell\n" +
            "-Tweak debug ui\n" +
            "-Give all patterns Non-Linear SpeedEasings (nerf spinners by making everything else harder)\n\n" +
            "Fixes:\n\n" +
            "-Fix Combo fire never reseting back to 0 on miss\n\n" + 
            "Additional Patch-Notes:\n\n" +
            "-Started work on Auto under the hood (both hardcoded ai and neural networking)";
    }
}
