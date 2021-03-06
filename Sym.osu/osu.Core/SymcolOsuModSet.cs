﻿#region usings

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
    public class SymcolOsuModSet : SymcolBaseSet
    {
        public static WikiOverlay WikiOverlay;

        public override OsuScreen GetMenuScreen() => new SymcolMenu();

        public override Toolbar GetToolbar() => new SymcolModdedToolbar();

        public override SettingsSection GetSettings() => new SymSection();

        public static ResourceStore<byte[]> LazerResources;
        public static TextureStore LazerTextures;

        public static ResourceStore<byte[]> SymcolResources;
        public static TextureStore SymcolTextures;
        public static AudioManager SymcolAudio;
        public static SymConfigManager SymConfigManager;

        public override void LoadComplete(OsuGame game, GameHost host)
        {
            base.LoadComplete(game, host);

            if (SymcolResources == null)
            {
                SymcolResources = new ResourceStore<byte[]>();
                SymcolResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Core.dll"), "Assets"));
                SymcolResources.AddStore(new DllResourceStore("osu.Core.dll"));
                SymcolTextures = new TextureStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Textures")));
                SymcolTextures.AddStore(new TextureLoaderStore(new OnlineStore()));

                ResourceStore<byte[]> tracks = new ResourceStore<byte[]>(SymcolResources);
                tracks.AddStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Tracks"));
                tracks.AddStore(new OnlineStore());

                ResourceStore<byte[]> samples = new ResourceStore<byte[]>(SymcolResources);
                samples.AddStore(new NamespacedResourceStore<byte[]>(SymcolResources, @"Samples"));
                samples.AddStore(new OnlineStore());

                SymcolAudio = new AudioManager(tracks, samples);

                LazerResources = new ResourceStore<byte[]>();
                LazerResources.AddStore(new DllResourceStore(@"osu.Game.Resources.dll"));
                LazerTextures = new TextureStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(LazerResources, @"Textures")));
            }

            if (SymConfigManager == null)
                SymConfigManager = new SymConfigManager(host.Storage);

            OsuModStore.ReloadModSets();

            foreach (OsuModSet mod in OsuModStore.LoadedModSets)
                mod.LoadComplete(game);

            if (WikiOverlay == null)
                game.Add(WikiOverlay = new WikiOverlay());
        }

        public override void Dispose()
        {
            SymConfigManager.Save();
            base.Dispose();
        }
    }
}
