using System.Collections.Generic;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects;
using OpenTK;

namespace Symcol.Rulesets.Core.HitObjects
{
    public abstract class SymcolHitObject : HitObject
    {
        public Vector2 Position { get; set; }
        public float X => Position.X;
        public float Y => Position.Y;

        public List<SampleInfo> BetterSamples { get; set; } = new List<SampleInfo>();

        public virtual Vector2 EndPosition => Position;
    }
}
