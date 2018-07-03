using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki.Included.Home
{
    public class WhatIsTheWiki : WikiSection
    {
        public override string Title => "What is this?";

        public override string Overview => "This is the un-official ingame wiki, " +
            "the wiki's purpose is to be just like any other wiki: house information about osu!lazer, rulesets and even mods loaded from the modloader. " +
            "You can get started by selecting a wiki from the index near the top left of this panel and return to this page at anytime by clicking \"Home\". " +
            "\"But wait! What if I close the wiki and want to get back?\" you might ask, " +
            "well on the toolbar at the top of the screen (toggle-able with Ctrl + T) " +
            "there is a question mark icon to the right of the clock, " +
            "that will both open and close the wiki for you at anytime on any screen. " +
            "Additionally you can close the wiki by clicking off of the sides of this panel on the background.";
    }
}
