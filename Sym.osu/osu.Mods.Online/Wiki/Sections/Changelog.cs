using osu.Core.Wiki.Included.Symcol.Sections;
using osu.Core.Wiki.Sections;

namespace osu.Mods.Online.Wiki.Sections
{
    public class Changelog : WikiChangelogSection
    {
        protected override string Version => SymcolChangelog.SYM_VERSION;

        protected override string StoragePath => "online\\changelogs";

        protected override string FileExtention => ".online";

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

                if (balances != null)
                {
                    add("Balance Changes:\n");
                    foreach (string balance in balances)
                        add(tab + balance);
                    add("");
                }

                if (fixes != null)
                {
                    add("Fixes:\n");
                    foreach (string fix in fixes)
                        add(tab + fix);
                    add("");
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
        };

        private readonly string[] features = null;

        private readonly string[] tweaksAndChanges = null;

        private readonly string[] balances = null;

        private readonly string[] fixes =
        {
            "Fix not being able to host or join",
            "Fix incorrect text being displayed when searching for a map",
            "Fix the \"Super Noise Bug\"",
        };
    }
}
