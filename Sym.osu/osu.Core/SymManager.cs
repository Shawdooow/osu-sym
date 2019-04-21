using System;
using System.Threading;
using osu.Core.Config;
using osu.Core.OsuMods;
using osu.Core.Wiki;
using osu.Framework.Audio.Sample;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics.Textures;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game;

namespace osu.Core
{
    public static class SymManager
    {
        public static WikiOverlay WikiOverlay;

        public static bool ModLoaderActive = false;

        public static ResourceStore<byte[]> LazerResources;
        public static TextureStore LazerTextures;

        public static ResourceStore<byte[]> SymResources;

        public static TextureStore SymTextures;
        public static TrackManager SymTracks;
        public static SampleManager SymSamples;
        
        public static SymConfigManager SymConfigManager;

        private static bool init;

        public static void Init(OsuGame game, GameHost host)
        {
            if (init) return;

            if (SymResources == null)
            {
                SymResources = new ResourceStore<byte[]>();
                SymResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Core.dll"), "Assets"));
                SymResources.AddStore(new DllResourceStore("osu.Core.dll"));
                SymTextures = new TextureStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(SymResources, @"Textures")));
                SymTextures.AddStore(new TextureLoaderStore(new OnlineStore()));

                ResourceStore<byte[]> tracks = new ResourceStore<byte[]>(SymResources);
                tracks.AddStore(new NamespacedResourceStore<byte[]>(SymResources, @"Tracks"));
                tracks.AddStore(new OnlineStore());

                SymTracks = game.Audio.GetTrackManager(tracks);

                ResourceStore<byte[]> samples = new ResourceStore<byte[]>(SymResources);
                samples.AddStore(new NamespacedResourceStore<byte[]>(SymResources, @"Samples"));
                samples.AddStore(new OnlineStore());

                SymSamples = game.Audio.GetSampleManager(samples);

                LazerResources = new ResourceStore<byte[]>();
                LazerResources.AddStore(new DllResourceStore(@"osu.Game.Resources.dll"));
                LazerTextures = new TextureStore(new TextureLoaderStore(new NamespacedResourceStore<byte[]>(LazerResources, @"Textures")));
            }

            if (SymConfigManager == null)
                SymConfigManager = new SymConfigManager(host.Storage);

            OsuModStore.ReloadModSets();

            foreach (OsuModSet mod in OsuModStore.LoadedModSets)
                mod.Init(game, host);

            if (WikiOverlay == null)
                WikiOverlay = new WikiOverlay();

            init = true;
        }

        private static bool complete;

        public static void LoadComplete(OsuGame game, GameHost host)
        {
            if (complete) return;

            foreach (OsuModSet mod in OsuModStore.LoadedModSets)
                mod.LoadComplete(game, host);

            game.Add(WikiOverlay);

            complete = true;
        }
    }
}
