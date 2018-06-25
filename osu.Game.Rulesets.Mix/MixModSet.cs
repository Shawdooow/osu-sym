using osu.Game.Rulesets.Mix.Wiki;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace osu.Game.Rulesets.Mix
{
    public sealed class MixModSet : SymcolModSet
    {
        public override WikiSet GetWikiSet() => new MixWikiSet();
    }
}
