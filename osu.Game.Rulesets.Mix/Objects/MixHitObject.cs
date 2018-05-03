using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Mix.Objects.Types;

namespace osu.Game.Rulesets.Mix.Objects
{
    public abstract class MixHitObject : HitObject, IHasRow
    {
        public virtual int Row { get; set; }
    }
}
