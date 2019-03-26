namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections.Changelog
{
    internal class _0_0_4 : ChangelogVersion
    {
        public override string VersionTitle => "Kill Zone";

        public override string VersionNumber => "0.0.4";

        public override string GetChangelog()
        {
            //Apr 4 - https://www.youtube.com/watch?v=MYq79XhfeN8
            //May 2 - https://www.youtube.com/watch?v=ujnGe6DHguY
            return TAB + "On April 4, 2017 enemies were being loaded to maps as they should, but they didn't shoot or go away after a bit. "
                       + "They would just kinda fill the screen and destroy the game slowly. "
                       + "It wasnt until May 2 that things really took off, enemies shot back and went away when they should.";
        }
    }

    internal class _0_0_3 : ChangelogVersion
    {
        public override string VersionTitle => "Preperations";

        public override string VersionNumber => "0.0.3";

        public override string GetChangelog()
        {
            //https://www.youtube.com/watch?v=-kanYHrO7eY
            return TAB + "On March 20, 2017 I showed off and enemy that would shoot at the player, "
                       + "but under the hood we were preparing to actually map the enemies to the beat so not a whole lot of visable progress was made.";
        }
    }

    internal class _0_0_2 : ChangelogVersion
    {
        public override string VersionTitle => "Learning Curb";

        public override string VersionNumber => "0.0.2";

        public override string GetChangelog()
        {
            //https://www.youtube.com/watch?v=C5fSYQqOhvA
            return TAB + "By February 17 bullets had been implemented but were kinda not shooting out the front of the player, they were going everywhere! "
                       + "Later that day one of us (I forget if it was me or rusky, it could've even had been ColdVolcano as he started helping around this time) "
                       + "fixed the bug to get an enemy displaying a lovely flower and a Reimu, ready to fight back. "
                       + "This progress also got Arrcival interested in the project, who would help with patterns and heavy design choices.";
        }
    }

    internal class _0_0_1 : ChangelogVersion
    {
        public override string VersionTitle => "Something Tangible";

        public override string VersionNumber => "0.0.1";

        public override string GetChangelog()
        {
            return TAB + "On February 2, 2017 the first iteration of vitaru was born. "
                       + "While it was nothing more than a Reimu sprite that moved around the screen it was something that I could start to play with, "
                       + "something to use and learn how to program on the osu.framework.";
        }
    }

    internal class _0_0_0 : ChangelogVersion
    {
        public override string VersionTitle => "The Beginning";

        public override string VersionNumber => "0.0.0";

        public override string GetChangelog()
        {
            return TAB + "Vitaru originally started way back in late 2016, after I discovered the ever elusive \"touhosu\" gamemode that never released. "
                   + "Around late November 2016 after messing with lazer a bit I decided I would take a crack at making my own \"touhosu\" on lazer as a mod. Why? "
                   + "Because I was impatiant and wanted to play. All this came from me being kind of an ass, and just wanting to play a gamemode that hadn't come out yet. "
                   + "Looking back at it doesn't help, as I had no idea how to program and would end up relying on someone else to start the project. "
                   + "This is where Rusky comes in, HE is the original developer of vitaru, HE started the code, HE is the one that put a player sprite on the screen and made it move.\n\n" + TAB
                   + "It sounds so trivial now but this IS how it all started, this is how touhosu went from a dream to a tangible toy, one called vitaru.";
        }
    }

    internal class Negative_0_0_1 : ChangelogVersion
    {
        public override string VersionTitle => "XD Meme";

        public override string VersionNumber => "-0.0.1";

        public override string GetChangelog()
        {
            //https://www.youtube.com/watch?v=q9Rn013kKNE
            return TAB + "On December 12, 2016 I posted one of my lazer showcase videos showcasing a bug with sliders. "
                       + "If you look closely the broken sliders weren't the only thing that was off, there was a second osu! icon in the gamemode bar at the top left. "
                       + "Vitaru back when it was \"Touhosu\" was Easter Egg in my lazer showcases, suprisingly no one ever noticed (maybe I just didn't get enough views?) "
                       + "This doesn't really have anything todo with the gamemode as it is today though, I just thought it was a fun little piece of history I didn't want to be lost.";
        }
    }
}
