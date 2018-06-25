using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Game;
using osu.Game.ModLoader;
using osu.Game.Overlays.Toolbar;
using osu.Game.Screens;
using Symcol.osu.Core.Containers.SymcolToolbar;
using Symcol.osu.Core.Screens;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace Symcol.osu.Core
{
    public class SymcolOsuModSet : ModSet
    {
        public static WikiOverlay WikiOverlay;

        public override OsuScreen GetMenuScreen() => new SymcolMenu();

        public override Toolbar GetToolbar() => new SymcolModdedToolbar();

        public static ResourceStore<byte[]> LazerResources;
        public static TextureStore LazerTextures;

        public static ResourceStore<byte[]> SymcolResources;
        public static TextureStore SymcolTextures;
        public static AudioManager SymcolAudio;

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            if (SymcolResources == null)
            {
                SymcolResources = new ResourceStore<byte[]>();
                SymcolResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("Symcol.osu.Core.dll"), "Assets"));
                SymcolResources.AddStore(new DllResourceStore("Symcol.osu.Core.dll"));
                SymcolTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Textures")));
                SymcolTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));

                var tracks = new ResourceStore<byte[]>(SymcolResources);
                tracks.AddStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Tracks"));
                tracks.AddStore(new OnlineStore());

                var samples = new ResourceStore<byte[]>(SymcolResources);
                samples.AddStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Samples"));
                samples.AddStore(new OnlineStore());

                SymcolAudio = new AudioManager(tracks, samples);

                LazerResources = new ResourceStore<byte[]>();
                LazerResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Game.Resources.dll"), ""));
                LazerResources.AddStore(new DllResourceStore("osu.Game.Resources.dll"));
                LazerTextures = new TextureStore(new RawTextureLoaderStore(new NamespacedResourceStore<byte[]>(LazerResources, @"Textures")));
                LazerTextures.AddStore(new RawTextureLoaderStore(new OnlineStore()));
            }

            SymcolModStore.ReloadModSets();

            if (WikiOverlay == null)
                game.Add(WikiOverlay = new WikiOverlay());
        }
    }
}
