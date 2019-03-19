namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections.Changelog
{
    internal class _0_8_6 : ChangelogVersion
    {
        public override string VersionNumber => "0.8.6";

        protected override string[] Versions => new[]
        {
            "2018.1104.0",
            "2018.1025.0",
            "2018.928.0",
        };

        protected override string[] Features => new[]
        {
            "Started re-implementing proper online multi",
            "Implemented beatmap statistics (see how many patterns, bullets and lasers are in a map in song select now)",
            "Add new experimental gamemode",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Tweak scoring metrics (tried to make grazing more rewarding)",
            "Re-organized a lot of stuff under the hood",
            "Made gamemodes more modular so adding new ones is easier (and less buggy)",
            "Hid \"Editor\" wiki section",
            "Hid \"Mapping\" wiki section",
            "Hid \"Multiplayer\" wiki section",
            "Hid \"Code\" wiki section",
        };

        protected override string[] Fixes => new[]
        {
            "Fix non goodFPS mode's scoring not working",
            "Fix retrying sometimes breaking scoring",
            "Fix the cursor destroying the game",
            "Fix editor mode not loading bullets",
            "Fix broken playfield sizing",
        };
    }

    internal class _0_8_5 : ChangelogVersion
    {
        public override string VersionNumber => "0.8.5";

        public override string GetChangelog() => TAB + "0.8.5 was a weird continuation of 0.8.4 after it got updated to lazer "
                                                 + "2018.928.0 because for a long period of time vitaru was super buggy and unusable. "
                                                 + "Eventually all the new features made their way into the current 0.8.6.";
    }

    internal class _0_8_4 : ChangelogVersion
    {
        public override string VersionNumber => "0.8.4";

        protected override string[] Versions => new[]
        {
            "2018.728.0",
            "2018.710.0",
            "2018.702.0",
            "2018.619.0",
        };

        protected override string[] Features => new[]
        {
            "AutoV3",
            "Implement Remilia's ability: Vampuric",
            "Implement Aya's ability: Snapshot",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Swap to the new skin",
            "\"Spells\" are now called \"Abilities\"",
            "Allow characters to specify additional ability stats",
            "Ryukoy's ability heavily reworked",
            "Removed dependency on osu.Game hacks and will now utilize the new \"Symcol Modloader\" system in place. As such this ruleset is now a \"Mod Enhanced\" ruleset.",
            "Adjusted all pattern easings (and increased base pattern speed by 25% so most patterns seem to travel at the same speed as before, now they will change speed less)",
        };

        protected override string DevNotes => "Ryukoy was just designed poorly, she was either gonna be way too powerful or absolute garbage, hopefully the new changes will allow her to find a middle ground where she is boh fun to play and balanced.\n\n" + TAB + 
            "Removing the osu.Game hacks dependency was on my list of things todo for awhile and I am glad that its finally been done, however not all is sunshine and rainbows. Unfortunetly this now means the wiki is now a Modloader exclusive feature making accessing imporant information about the ruleset all the harder for new players.";
    }

    internal class _0_8_3 : ChangelogVersion
    {
        public override string VersionNumber => "0.8.3";

        protected override string[] Versions => new[]
        {
            "2018.607.0",
            "2018.606.0",
        };

        protected override string[] Features => new[]
        {
            "Added an option that will 2x to 10x ruleset performance depending on map (when not in editor) under the \"Custom\" graphics settings",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Adjust Touhosu playfield size + aspect ratio (make it bigger inline with TouhouSharp's playfield [doubled width])",
            "Adjust StandardV2 bullet animations",
            "Patterns are per individual hitsound rather than one pattern each time hitsounds are played (basically there are gonna be way more bullets now)",
            "Bring back the ranked play detector as a debug option (too buggy to go live yet)",
            "Killing enemies now will grant 300 extra score",
        };

        protected override string[] Fixes => new[]
        {
            "Fix hitsounding not being 1:1 with stable",
            "Fix boss death crashing game",
        };

        protected override string DevNotes => "Generally speaking now the more beautiful a map's hitsounds the more likely it is to kill you. "
                                              + "I also was tinkering with a wider playfield and thought it would be a welcome change in Touhosu with the new patterns that are planned.";
    }

    internal class _0_8_2 : ChangelogVersion
    {
        public override string VersionNumber => "0.8.2";

        protected override string[] Versions => new[]
        {
            "2018.531.0",
            "2018.526.0",
        };

        protected override string[] Features => new[]
        {
            "Implement Reimu's Spell \"Leader\"",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Tweak debug ui",
            "Give all patterns Non-Linear SpeedEasings (nerf spinners by making everything else harder)",
        };

        protected override string[] Balances => new []
        {
            "Tweak Reimu's stats based on now implemented spell",
        };

        protected override string[] Fixes => new[]
        {
            "Fix Combo fire never reseting back to 0 on miss",
            "",
            "",
            "",
        };

        protected override string DevNotes => "Around this time I: \"Started work on Auto under the hood (both hardcoded ai and neural networking)\" "
                                              + "Unfortunetly this never really went anywhere.";
    }

    internal class _0_8_1 : ChangelogVersion
    {
        public override string VersionNumber => "0.8.1";

        protected override string[] Features => new[]
        {
            "Debug mode (Must be a verified developer to use, contains cheats and whatnot to help debug stuff)",
            "Bug player to checkout settings once",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Some serious optimizations (around 2x updates)",
            "Use standard Get functions for characters (community made characters soon!)",
            "Added Sakuya's Background",
        };

        protected override string[] Balances => new[]
        {
            "Nerf Ryukoy's Health (80 => 60)",
            "Nerf Ryukoy's Max Energy (24 => 20)",
            "Nerf Tomaji's Max Energy (16 => 12)",
            "Tomaji's full blink charge time reduced (2000ms => 600ms)",
            "Tomaji's distance covered at full charge reduced (400 => 200)",
            "Nerf Sakuya's Health (100 => 80)",
            "Nerf Sakuya's Max Energy (36 => 32)",
        };
    }

    internal class _0_8_0 : ChangelogVersion
    {
        public override string VersionTitle => "The vitaru!online Upgrade";

        public override string VersionNumber => "0.8.0";

        protected override string[] Versions => new[]
        {
            "2018.517.0",
            "2018.514.0",
            "2018.511.0",
        };   

        protected override string[] Features => new[]
        {
            "Co-op online multiplayer",
            "Tomaji's spell (also adjusted Tomaji's stats based on his now implemented spell)",
            "Rezzurect boss",
            "StandardV2 graphics option (give it a try, we have AnimatedSprites now!)",
        };

        protected override string[] TweaksAndChanges => new[]
        {
            "Split old \"Gameplay\" wiki section into three new sections: General, Gamemodes and Characters",
            "Make Healing and energy gain per bullet and the amount distance based",
            "Change most of the patterns to have variable bullet size and damage",
            "Rework how graphics options are done to be more like most games (presets + \"custom\" for fine tunning to what you like)",
            "Re-enable ComboFire",
        };

        protected override string[] Balances => new[]
        {
            "Nerf Sakuya's energy drain rate (6 => 8)",
        };

        protected override string[] Fixes => new[]
        {
            "Fix Seal not loading properly for non touhosu characters",
            "Fix ComboFire memory leakage",
        };
    }
}
