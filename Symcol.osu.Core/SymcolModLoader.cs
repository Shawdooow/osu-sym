using osu.Game.Screens;
using osu.Game.Symcol;
using Symcol.osu.Core.Screens;

namespace Symcol.osu.Core
{
    public class SymcolModLoader : ModSet
    {
        public override OsuScreen GetMenuScreen() => new SymcolMenu();
    }
}
