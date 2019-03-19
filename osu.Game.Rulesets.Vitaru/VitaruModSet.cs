using osu.Core.OsuMods;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki;

namespace osu.Game.Rulesets.Vitaru
{
    public sealed class VitaruModSet : OsuModSet
    {
        public override WikiSet GetWikiSet() => new VitaruWikiSet();

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            VitaruRuleset.VitaruAudio.Volume.BindTo(game.Audio.Volume);
            VitaruRuleset.VitaruAudio.VolumeSample.BindTo(game.Audio.VolumeSample);
            VitaruRuleset.VitaruAudio.VolumeTrack.BindTo(game.Audio.VolumeTrack);

            SymSection.OnPurge += () =>
            {
                
            };
        }
    }
}
