using osu.Game.Rulesets.Vitaru.SymcolMods.Wiki;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.SymcolMods
{
    public sealed class VitaruModSet : SymcolModSet
    {
        public override WikiSet GetWikiSet() => new VitaruWikiSet();
    }
}
