using osu.Core.Wiki.Included.Symcol.Sections;
using osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections
{
    public class ChangelogSection : WikiChangelogSection
    {
        public override string Title => "Changelog";

        protected override string Version => SymcolChangelog.SYMCOL_VERSION;

        protected override string StoragePath => "vitaru\\changelogs";

        protected override string FileExtention => ".vitaru";

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
                        add(tab + "Updated to osu!lazer version "  +  version);
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

        private readonly string[] features =
        {
            "[Windows] Score Submission (Leaderboards don't work but scores are uploaded and saved when connected)",
            "OSX Support",
            "iOS Support",
            "Touch screen controls (they are a toggle in settings for now)",
            //"Linux Support",
            //"Added classic enemy visuals",
            "Added experimental bullet visuals",
            "Added touhou sounds (Experimental)",
        };

        private readonly string[] tweaksAndChanges =
        {
            "Get Time.Current less (Bullets + Enemies hurt fps less now)",
        };

        private readonly string[] balances = null;

        private readonly string[] fixes =
        {
            "Fix ComboColors not working basically at all",
            "Fix storymode making Ryukoy crash the game",
        };

        private readonly string devNotes = null;
    }
}
