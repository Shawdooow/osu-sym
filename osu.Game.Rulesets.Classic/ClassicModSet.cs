using osu.Game.Rulesets.Classic.Wiki;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace osu.Game.Rulesets.Classic
{
    public sealed class ClassicModSet : SymcolModSet
    {
        public override WikiSet GetWikiSet() => new ClassicWikiSet();

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            ClassicRuleset.ClassicAudio.VolumeSample.BindTo(game.Audio.VolumeSample);
            ClassicRuleset.ClassicAudio.VolumeTrack.BindTo(game.Audio.VolumeTrack);
        }
    }
}
