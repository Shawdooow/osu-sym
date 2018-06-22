using osu.Game.ModLoader;
using osu.Game.Screens;
using Symcol.osu.Core.Screens;

namespace Symcol.osu.Core
{
    public class SymcolOsuModSet : ModSet
    {
        public override OsuScreen GetMenuScreen() => new SymcolMenu();
    }
}
