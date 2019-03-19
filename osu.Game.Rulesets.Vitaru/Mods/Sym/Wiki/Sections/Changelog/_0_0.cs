namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections.Changelog
{
    internal class _0_0_1 : ChangelogVersion
    {
        public override string VersionTitle => "Learning Curb";

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
}
