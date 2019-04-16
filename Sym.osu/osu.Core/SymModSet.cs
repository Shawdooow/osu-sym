#region usings

using osu.Core.Config;
using osu.Core.Containers.SymcolToolbar;
using osu.Core.OsuMods;
using osu.Core.Screens;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game;
using osu.Game.ModLoader;
using osu.Game.Overlays.Settings;
using osu.Game.Overlays.Toolbar;
using osu.Game.Screens;

#endregion

namespace osu.Core
{
    public class SymModSet : SymcolBaseSet
    {
        public override OsuScreen GetMenuScreen() => new SymcolMenu();

        public override Toolbar GetToolbar() => new SymcolModdedToolbar();

        public override SettingsSection GetSettings() => new SymSection();

        public override void LoadComplete(OsuGame game, GameHost host)
        {
            base.LoadComplete(game, host);

            SymManager.Init(game, host);
        }

        public override void Dispose()
        {
            SymManager.SymConfigManager.Save();
            base.Dispose();
        }
    }
}
