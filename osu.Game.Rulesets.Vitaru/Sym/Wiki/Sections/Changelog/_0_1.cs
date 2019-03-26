namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections.Changelog
{
    internal class _0_1_1 : ChangelogVersion
    {
        public override string VersionTitle => "";

        public override string VersionNumber => "0.1.1";

        public override string GetChangelog()
        {
            return TAB + "0.1.1 was bug fixes and all around improvements to some of the problems vitaru had.";
        }
    }

    internal class _0_1_0 : ChangelogVersion
    {
        public override string VersionTitle => "First Public Pre-Release";

        public override string VersionNumber => "0.1.0";

        public override string GetChangelog()
        {
            //https://www.youtube.com/watch?v=QBhe9Atds5s
            return TAB + "On May 11, 2017 Vitaru went public.";
        }
    }
}
