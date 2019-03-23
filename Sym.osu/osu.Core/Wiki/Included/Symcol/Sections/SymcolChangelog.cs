#region usings

using osu.Core.Wiki.Sections;

#endregion

namespace osu.Core.Wiki.Included.Symcol.Sections
{
    public sealed class SymcolChangelog : WikiChangelogSection
    {
        public const string SYM_VERSION = "0.10.0";

        public override string Title => "Changelog";

        protected override string Version => SYM_VERSION;

        protected override string StoragePath => "symcol\\changelogs";

        protected override string FileExtention => ".symcol";

        protected override string VersionChangelog => changelog;

        private string changelog
        {
            get
            {
                string change = "";
                const string tab = "    -";

                if (versions != null)
                {
                    foreach (string version in versions)
                        add(tab + "Updated to osu!lazer version " + version);
                    add("");
                }

                if (features != null)
                {
                    add("Features:\n");
                    foreach (string feature in features)
                        add(tab + feature);
                    add("");
                }

                if (tweaksAndChanges != null)
                {
                    add("Tweaks and Changes:\n");
                    foreach (string tweak in tweaksAndChanges)
                        add(tab + tweak);
                    add("");
                }

                if (fixes != null)
                {
                    add("Fixes:\n");
                    foreach (string fix in fixes)
                        add(tab + fix);
                    add("");
                }

                if (devNotes != null)
                {
                    add("Dev Notes:\n");
                    change = change + "     " + devNotes;
                }

                return change;

                void add(string a)
                {
                    change = change + a + "\n";
                }
            }
        }

        private readonly string[] versions =
        {
            "1414",
            "1303",
            "1227",
        };

        private readonly string[] features = null;

        private readonly string[] tweaksAndChanges = null;

        private readonly string[] fixes =
        {
            "Fix SymcolButton code being a mess (and made it a bit prettier)",
        };

        private readonly string devNotes = null;
    }
}
