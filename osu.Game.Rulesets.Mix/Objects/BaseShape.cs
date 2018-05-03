using OpenTK;

namespace osu.Game.Rulesets.Mix.Objects
{
    public class BaseShape : MixHitObject
    {
        public override HitObjectType Type => HitObjectType.Shape;
        public Vector2 StartPosition { get; set; }
        public float ShapeSize { get; set; } = 40;
        public int ShapeID { get; set; }
    }
}
