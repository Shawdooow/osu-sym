using System;
using System.Threading;
using osu.Core.Config;
using osu.Core.OsuMods;
using osu.Core.Wiki;
using osu.Framework.Audio;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osu.Game;

namespace osu.Core
{
    public static class SymManager
    {
        public static WikiOverlay WikiOverlay;

        public static ResourceStore<byte[]> LazerResources;
        public static TextureStore LazerTextures;

        public static ResourceStore<byte[]> SymcolResources;
        public static TextureStore SymcolTextures;
        public static AudioManager SymcolAudio;
        public static SymConfigManager SymConfigManager;

        private static bool init;

        public static void Init(OsuGame game, GameHost host)
        {
            if (init) return;

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

                SymcolAudio = new AudioManager(host.AudioThread, tracks, samples);

                LazerResources = new ResourceStore<byte[]>();
                LazerResources.AddStore(new DllResourceStore(@"osu.Game.Resources.dll"));
                LazerTextures = new TextureStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(LazerResources, @"Textures")));
            }

            if (SymConfigManager == null)
                SymConfigManager = new SymConfigManager(host.Storage);

            OsuModStore.ReloadModSets();

            foreach (OsuModSet mod in OsuModStore.LoadedModSets)
                mod.LoadComplete(game, host);

            if (WikiOverlay == null)
            {
                WikiOverlay = new WikiOverlay();
                if (Thread.CurrentThread.Name == GameThread.PrefixedThreadNameFor("Update")) game.Add(WikiOverlay);
            }

            init = true;
        }
    }
}
