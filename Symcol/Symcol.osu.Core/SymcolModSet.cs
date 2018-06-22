using osu.Game.Screens;
using Symcol.osu.Core.Screens;

namespace Symcol.osu.Core
{
    public class SymcolModSet : ModSet
    {
        public override OsuScreen GetMenuScreen() => new SymcolMenu();
    }
}
