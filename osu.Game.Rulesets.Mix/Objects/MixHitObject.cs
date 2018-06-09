using osu.Game.Rulesets.Mix.Objects.Types;
using Symcol.Rulesets.Core.HitObjects;

namespace osu.Game.Rulesets.Mix.Objects
{
    public abstract class MixHitObject : SymcolHitObject, IHasRow
    {
        public virtual int Row { get; set; }
    }
}
