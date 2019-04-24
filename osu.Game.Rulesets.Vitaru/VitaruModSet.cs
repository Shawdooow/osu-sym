#region usings

using osu.Core.OsuMods;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Framework.IO.Stores;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Sym.Wiki;

#endregion

namespace osu.Game.Rulesets.Vitaru
{
    public sealed class VitaruModSet : OsuModSet
    {
        public override WikiSet GetWikiSet() => new VitaruWikiSet();

        public override void Init(OsuGame game, GameHost host)
        {
            base.Init(game, host);

            SymSection.OnPurge += storage =>
            {
                if (storage.ExistsDirectory("vitaru")) storage.DeleteDirectory("vitaru");
            };

            ResourceStore<byte[]> samples = new ResourceStore<byte[]>(VitaruRuleset.VitaruResources);
            samples.AddStore(new NamespacedResourceStore<byte[]>(VitaruRuleset.VitaruResources, @"Samples"));
            samples.AddStore(new OnlineStore());

            VitaruRuleset.VitaruSamples = game.Audio.GetSampleManager(samples);
        }
    }
}
