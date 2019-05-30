using osu.Game.Rulesets.Vitaru.SymcolMods.Wiki;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.SymcolMods
{
    public sealed class VitaruModSet : SymcolModSet
    {
        public override WikiSet GetWikiSet() => new VitaruWikiSet();

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            VitaruRuleset.VitaruAudio.Volume.BindTo(game.Audio.Volume);
            VitaruRuleset.VitaruAudio.VolumeSample.BindTo(game.Audio.VolumeSample);
            VitaruRuleset.VitaruAudio.VolumeTrack.BindTo(game.Audio.VolumeTrack);
        }
    }
}
