namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections.Changelog
{
    internal class _0_9_0 : ChangelogVersion
    {
        public override string VersionTitle => "The Modded Upgrade";

        public override string VersionNumber => "0.9.0";

        protected override string[] Versions => new[]
        {
            "2018.1205.0",
        };

        protected override string[] Features => new[]
        {
            "Modded chapter support!",
            "Modded gamemode support (Made the ruleset super unstable, please bare with me!)",
            "New Ability for Reimu (Started): Ethereal Support",
            "Flandre's Ability: Four of a Kind",
            "Re-Implemented Lasers (Mostly. . .)",
            "Re-Enabled Totems (Used during kiai fights now)",
            "Removed that \"GoodFPS\" option (It is now on all the time, even in the editor now!)",
            @"Mods:
        -Sudden Death
        -Perfect
        -True Perfect
        -True Half Time
        -True Daycore
        -True Double Time
        -True Nightcore
        -Hidden
        -True Hidden
        -Flashlight
        -Accel
        -Deccel (Isn't hooked up yet*)",
            "Live PP calculator in debug mode (NOT FINALIZED!)",
            "Experimental mode (used to test alternate code that could be faster, not gameplay features for now)",
            "Removed Storymode",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Make hit detection feel more accurate (Still isnt perfect)",
            "Mangle the scoring system",
            "Dodge's playfield is now wider",
            "Clusters in dodge now aim directly at the player rather than down (wasn't actually supposed to be down but that just shows how buggy it was)",
            "Tweak easings on clusters (the star + enemies)",
            "Tons of code changes to better prepare for the future",
            "Upgraded changelog to be easier to use (for me)",
            "Performance optimizations (Small gains, nothing too fancy)",
        };

        protected override string[] Balances => new[]
        {
            "Player - Hitbox Diameter increased (4 => 8)",
            "Player - Speed changed (Full speed [0.25 => 0.4], Half speed [0.125 => 0.2])",
            "Touhosu - You now start with half your character's max energy (Rather than 0)",
            "Sakuya - MaxHealth reduced (80 => 60)",
            "Sakuya - MaxEnergy reduced (32 => 24)",
            "Sakuya - EnergyCost increased (2 => 4)",
            "Sakuya - EnergyDrainRate reduced (8 => 4)",
            "Sakuya - Ability now has a base drain independent of rate of change applied to clock of her EnergyCost (4)",

        };

        protected override string[] Fixes => new[]
        {
            "Fix scrubbing in the editor being super buggy and basically not working (now it mostly works)",
            "Fix dodge playfield being wrong size",
            "Fix boss not moving right",
        };

        protected override string DevNotes => null;
    }
}
