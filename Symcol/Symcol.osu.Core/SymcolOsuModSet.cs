using osu.Game;
using osu.Game.ModLoader;
using osu.Game.Overlays.Toolbar;
using osu.Game.Screens;
using Symcol.osu.Core.Containers;
using Symcol.osu.Core.Containers.SymcolToolbar;
using Symcol.osu.Core.Screens;
using Symcol.osu.Core.Wiki;

namespace Symcol.osu.Core
{
    public class SymcolOsuModSet : ModSet
    {
        public static WikiOverlay WikiOverlay;

        public override OsuScreen GetMenuScreen() => new SymcolMenu();

        public override Toolbar GetToolbar() => new SymcolModdedToolbar();

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            if (WikiOverlay == null)
                game.Add(WikiOverlay = new WikiOverlay());
        }
    }
}
