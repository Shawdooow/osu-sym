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
            "[WIP] ChapterSets Reborn",
            "[WIP] Brought back base vitaru to be given a whole new set of patterns focused on movement "
            + "(or rather keeping the player moving to the beat, new patterns can be tested in Touhosu when enabled in debug mode). "
            + "Touhosu will not use the new patterns and instead keep the old ones once the vitaru gamemode is up and running "
            + "(Dodge also might get killed since vitaru will be a straight upgrade in most ways).",
            "Scoring V3",
            "New \"Boundless\" mod that removes the playfield borders and makes the enemies target you.",
            "[Experimental] New bullet visuals (TO BE REMOVED!)",
            //"Added classic enemy visuals",

            "Score submission testing (Leaderboards don't work but scores are uploaded and saved when connected)",
            "Multiplayer support (using the Online mod)",
            "[Multi] Live Spectator support",
            "[Multi] Head to Head support",
            "[Multi] CO-OP support",
            "[WIP] [Multi] TAG support",
            "[WIP] [Multi] PVP support\n",

            "OSX Support (Really this was the modloader's fault...)",
            "iOS Support (Also the modloaders's fault mostly, Aya also deserves some blame...)",
            //"Linux Support",
            //"Android Support",
            "Touch screen controls (they are a toggle in settings for now)\n",
            
            "Upgraded changelog again and backfilled all the way to 0.0.1",
            "Removed Storymode",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Made shooting more responsive (now it will always fire when you click, and if you hold it will auto fire on half beats)",
            "Brought back some old wiki sections on mapping and multiplayer",
            "Small optimizations to bullets and enemies",
            "Editor is now locked behind an option in debug mode (its buggy, crashy and useless. if you really want to poke it though, I won't stop you)",
        };

        protected override string[] Fixes => new[]
        {
            "Fix the really bad memory leak when not in editor (where pretty much nothing was getting claimed by the GC, this should help in low RAM situations greatly)",
            "Fix ComboColors not working basically at all",
            "Fix a number of smaller bugs resulting from the ChapterSets changes and optimizations to memory usage",
            "Fix player bullets not shooting as far as they should",
        };
    }
}
