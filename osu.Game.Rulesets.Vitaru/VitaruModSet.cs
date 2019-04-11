#region usings

using osu.Core.OsuMods;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Framework.Audio;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Sym.Wiki;

#endregion

namespace osu.Game.Rulesets.Vitaru
{
    public sealed class VitaruModSet : OsuModSet
    {
        public override WikiSet GetWikiSet() => new VitaruWikiSet();

        public override void LoadComplete(OsuGame game, GameHost host)
        {
            base.LoadComplete(game, host);

            SymSection.OnPurge += storage =>
            {
                if (storage.ExistsDirectory("vitaru")) storage.DeleteDirectory("vitaru");
            };

            ResourceStore<byte[]> tracks = new ResourceStore<byte[]>(VitaruRuleset.VitaruResources);
            tracks.AddStore(new NamespacedResourceStore<byte[]>(VitaruRuleset.VitaruResources, @"Tracks"));

            ResourceStore<byte[]> samples = new ResourceStore<byte[]>(VitaruRuleset.VitaruResources);
            samples.AddStore(new NamespacedResourceStore<byte[]>(VitaruRuleset.VitaruResources, @"Samples"));

            VitaruRuleset.VitaruAudio = new AudioManager(host.AudioThread, tracks, samples);

            VitaruRuleset.VitaruAudio.Volume.BindTo(game.Audio.Volume);
            VitaruRuleset.VitaruAudio.VolumeSample.BindTo(game.Audio.VolumeSample);
            VitaruRuleset.VitaruAudio.VolumeTrack.BindTo(game.Audio.VolumeTrack);
        }
    }
}
