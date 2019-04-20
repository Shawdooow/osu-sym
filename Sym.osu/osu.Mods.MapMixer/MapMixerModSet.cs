#region usings

using osu.Core.Containers.Shawdooow;
using osu.Core.OsuMods;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
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
        public static SampleManager ClassicSamples;

        public override void Init(OsuGame game, GameHost host)
        {
            base.Init(game, host);

            if (ClassicSamples == null)
            {
                ClassicResources = new ResourceStore<byte[]>();
                ClassicResources.AddStore(new NamespacedResourceStore<byte[]>(new DllResourceStore("osu.Mods.MapMixer.dll"), "Assets"));
                ClassicResources.AddStore(new DllResourceStore("osu.Mods.MapMixer.dll"));

                ResourceStore<byte[]> samples = new ResourceStore<byte[]>(ClassicResources);
                samples.AddStore(new NamespacedResourceStore<byte[]>(ClassicResources, @"Samples"));
                samples.AddStore(new OnlineStore());

                ClassicSamples = game.Audio.GetSampleManager(samples);
            }
        }
    }
}
