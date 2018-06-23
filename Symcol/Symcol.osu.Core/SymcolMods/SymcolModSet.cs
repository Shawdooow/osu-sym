using osu.Game.Screens;
using Symcol.osu.Core.Containers.Shawdooow;

namespace Symcol.osu.Core.SymcolMods
{
    public abstract class SymcolModSet
    {
        public abstract SymcolButton GetMenuButton();

        public abstract OsuScreen GetMenuScreen();

        //public virtual WikiSet GetWikiSet() => new WikiSet();
    }
}
