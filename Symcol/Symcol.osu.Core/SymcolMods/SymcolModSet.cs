using osu.Game;
using osu.Game.Screens;
using Symcol.osu.Core.Containers.Shawdooow;
using Symcol.osu.Core.Wiki;

namespace Symcol.osu.Core.SymcolMods
{
    public abstract class SymcolModSet
    {
        public virtual SymcolButton GetMenuButton() => null;

        public virtual OsuScreen GetScreen() => null;

        public virtual WikiSet GetWikiSet() => null;

        public virtual void LoadComplete(OsuGame game) { }
    }
}
