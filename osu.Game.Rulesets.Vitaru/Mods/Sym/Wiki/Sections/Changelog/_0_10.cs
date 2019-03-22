namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections.Changelog
{
    internal class _0_10_0 : ChangelogVersion
    {
        public override string VersionTitle => "The Cross Platform Upgrade";

        public override string VersionNumber => "0.10.0";

        protected override string[] Versions => new[]
        {
            "2019.214.0",
            "2019.103.0",
            "2018.1227.0",
        };

        protected override string[] Features => new[]
        {
            "[WIP] Patterns V3",
            "Scoring V3",
            "[Experimental] New bullet visuals",
            "[Experimental] Touhou sounds",
            "[Desktop] Score Submission (Leaderboards don't work but scores are uploaded and saved when connected)",
            "[WIP] Multiplayer Support (using the Online mod)",
            "[WIP] [Multi] Live Spectator Support",
            "[Multi] Head to Head Support",
            "[WIP] [Multi] CO-OP Support",
            "[WIP] [Multi] TAG Support",
            "[WIP] [Multi] PVP Support",
            "OSX Support (Really this was the modloader's fault...)",
            "iOS Support",
            "Touch screen controls (they are a toggle in settings for now)",
            //"Linux Support",
            //"Added classic enemy visuals",
            "Remove Storymode",
            "Upgraded Changelog again (and will start backfilling old versions back to pre 0.1.0)",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Made shooting more responsive",
            "Brought back some old wiki sections on mapping and multiplayer (wonder what those could be for...)",
            "Get Time.Current less (Bullets + Enemies hurt fps less now)",
        };

        protected override string[] Fixes => new[]
        {
            "Fix ComboColors not working basically at all",
            //"Fix storymode making Ryukoy crash the game",
        };
    }
}
