using osu.Game.Rulesets.Classic.Wiki;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace osu.Game.Rulesets.Classic
{
    public sealed class ClassicModSet : SymcolModSet
    {
        public override WikiSet GetWikiSet() => new ClassicWikiSet();
    }
}
