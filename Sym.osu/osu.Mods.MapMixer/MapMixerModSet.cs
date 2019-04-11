#region usings

using osu.Core.Containers.Shawdooow;
using osu.Core.OsuMods;
using osu.Framework.Audio;
using osu.Framework.Graphics;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game;
using osu.Game.Screens;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Mods.MapMixer
{
    public class MapMixerModSet : OsuModSet
    {
        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonText = "Map Mixer",
            FontSizeMultiplier = 0.8f,
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Purple,
            ButtonColorBottom = Color4.HotPink,
            Size = 120,
            Position = new Vector2(-200, -150),
        };

        public override OsuScreen GetScreen() => new MapMixer();

        //public override WikiSet GetWikiSet() => new MapMixerWikiSet();

        public static ResourceStore<byte[]> ClassicResources;
        public static AudioManager ClassicAudio;

        public override void LoadComplete(OsuGame game, GameHost host)
        {
            base.LoadComplete(game, host);

            if (ClassicAudio == null)
            {
                ClassicResources = new ResourceStore<byte[]>();
                ClassicResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Mods.MapMixer.dll"), "Assets"));
                ClassicResources.AddStore(new DllResourceStore("osu.Mods.MapMixer.dll"));

                ResourceStore<byte[]> tracks = new ResourceStore<byte[]>(ClassicResources);
                tracks.AddStore(new NamespacedResourceStore<byte[]>(ClassicResources, @"Tracks"));
                tracks.AddStore(new OnlineStore());

                ResourceStore<byte[]> samples = new ResourceStore<byte[]>(ClassicResources);
                samples.AddStore(new NamespacedResourceStore<byte[]>(ClassicResources, @"Samples"));
                samples.AddStore(new OnlineStore());

                ClassicAudio = new AudioManager(host.AudioThread, tracks, samples);

                ClassicAudio.Volume.BindTo(game.Audio.Volume);
                ClassicAudio.VolumeSample.BindTo(game.Audio.VolumeSample);
                ClassicAudio.VolumeTrack.BindTo(game.Audio.VolumeTrack);
            }
        }
    }
}
