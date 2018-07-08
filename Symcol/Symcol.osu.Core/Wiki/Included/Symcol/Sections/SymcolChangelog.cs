using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki.Included.Symcol.Sections
{
    public sealed class SymcolChangelog : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string Version => "0.0.1";

        protected override string StoragePath => "symcol\\changelogs";

        protected override string FileExtention => ".symcol";

        protected override string VersionChangelog => "-Initial release on lazer version 2018.702.0\n\n" +
            "Features:\n\n" +
            "-Add changelog section to wiki\n\n" +
            "Tweaks / Changes:\n\n" +
            "\n\n" +
            "Fixes:\n\n" +
            "\n\n" +
            "Dev Notes:\n\n" +
            "";
    }
}
