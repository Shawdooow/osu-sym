namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections.Changelog
{
    internal abstract class ChangelogVersion
    {
        public abstract string VersionNumber { get; }

        public virtual string VersionTitle => null;

        protected const string TAB = "    ";

        public virtual string GetChangelog()
        {
            string change = "";
            const string tab = TAB + "-";

            if (Versions != null)
            {
                foreach (string version in Versions)
                    add(tab + "Updated to osu!lazer version " + version);
                add("");
            }

            if (Features != null)
            {
                add("Features:\n");
                foreach (string feature in Features)
                    add(tab + feature);
                add("");
            }

            if (TweaksAndChanges != null)
            {
                add("Tweaks and Changes:\n");
                foreach (string tweak in TweaksAndChanges)
                    add(tab + tweak);
                add("");
            }

            if (Balances != null)
            {
                add("Balance Changes:\n");
                foreach (string balance in Balances)
                    add(tab + balance);
                add("");
            }

            if (Fixes != null)
            {
                add("Fixes:\n");
                foreach (string fix in Fixes)
                    add(tab + fix);
                add("");
            }

            if (DevNotes != null)
            {
                add("Dev Notes:\n");
                change = change + TAB + DevNotes;
            }

            return change;

            void add(string a)
            {
                change = change + a + "\n";
            }
        }

        protected virtual string[] Versions => null;

        protected virtual string[] Features => null;

        protected virtual string[] TweaksAndChanges => null;

        protected virtual string[] Balances => null;

        protected virtual string[] Fixes => null;

        protected virtual string DevNotes => null;
    }
}
