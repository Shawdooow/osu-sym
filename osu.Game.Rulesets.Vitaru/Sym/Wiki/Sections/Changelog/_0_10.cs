namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections.Changelog
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
            //"Added classic enemy visuals",
            "[Experimental] Touhou sounds\n",

            "Score Submission (Leaderboards don't work but scores are uploaded and saved when connected)",
            "[WIP] Multiplayer Support (using the Online mod)",
            "[WIP] [Multi] Live Spectator Support",
            "[Multi] Head to Head Support",
            "[WIP] [Multi] CO-OP Support",
            "[WIP] [Multi] TAG Support",
            "[WIP] [Multi] PVP Support\n",

            "OSX Support (Really this was the modloader's fault...)",
            "iOS Support",
            //"Linux Support",
            //"Android Support",
            "Touch screen controls (they are a toggle in settings for now)\n",
            
            "Upgraded Changelog again (and will start backfilling old versions back to pre 0.1.0)",
            "Removed Storymode",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Made shooting more responsive (now it will always fire when you click, and if you hold it will auto fire on half beats)",
            "Brought back some old wiki sections on mapping and multiplayer (wonder what those could be for...)",
            "Get Time.Current less (Bullets + Enemies hurt fps less now)",
        };

        protected override string[] Fixes => new[]
        {
            "Fix the really bad memory leak (where pretty much nothing was getting claimed by the GC, this should help in low RAM situations greatly)",
            "Fix ComboColors not working basically at all",
            //"Fix storymode making Ryukoy crash the game",
        };
    }
}
