namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections.Changelog
{
    internal class _0_7_7 : ChangelogVersion
    {
        public override string VersionNumber => "0.7.7";

        protected override string[] TweaksAndChanges => new[]
        {
            "Tweak changelog section code to be a bit easier to use (getting ready to absract it for all rulesets to use)",
            "Update wiki to be more accurate + hide some sections",
            "Made circle pattern harder (increased default bullet count from 8 to 12)",
            "Show current clock rate for Sakuya",
        };

        protected override string[] Balances => new[]
        {
            "Nerf Sakuya's energy drain per second (increased from 4 to 6)",
            "Change characters who use energy over time to rather than also having a initiation cost by default a minimum energy to start (still listed on wiki and in code as EnergyCost for now)",
        };

        protected override string[] Fixes => new[]
        {
            "Fix wedge pattern",
        };
    }

    internal class _0_7_6 : ChangelogVersion
    {
        public override string VersionTitle => "The Changelog Upgrade";

        public override string VersionNumber => "0.7.6";

        protected override string[] Features => new[]
        {
            "Implemented cumulative changelog",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            
            "Fix character hiearchy",
            "Custom character support prep (characterName.vitaru in your map or something)",
        };

        protected override string[] Fixes => new[]
        {
            "Fix several multiplayer crashes",
        };

        protected override string DevNotes => "At some point between 0.7.0 and now the patterns got \"wider\", they became faster and less dense.";
    }

    internal class _0_7_0 : ChangelogVersion
    {
        public override string VersionTitle => "The Rhthym Battles Upgrade";

        public override string VersionNumber => "0.7.0";

        protected override string[] Versions => new[]
        {
            "around December 27, 2017",
        };

        protected override string[] Features => new[]
        {
            "Added support for playing with up to 7 or against 8 bots",
            "Added a pattern editor window to the editor",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Improvements to the Wiki",
        };

        protected override string DevNotes => "It was around this time I began thinking about actually writing some networking code to try and add online multiplayer, "
                                              + "especially now that the core game supported more than one player loaded at a time.";
    }
}
