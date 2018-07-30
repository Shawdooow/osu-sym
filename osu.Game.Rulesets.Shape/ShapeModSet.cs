using osu.Game.Rulesets.Shape.Wiki;
using Symcol.osu.Core.SymcolMods;
using Symcol.osu.Core.Wiki;

namespace osu.Game.Rulesets.Shape
{
    public sealed class ShapeModSet : SymcolModSet
    {
        public override WikiSet GetWikiSet() => new ShapeWikiSet();
    }
}
