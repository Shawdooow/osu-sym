using OpenTK;

namespace osu.Game.Rulesets.Shape.Objects
{
    public class BaseShape : ShapeHitObject
    {
        public override HitObjectType Type => HitObjectType.Shape;
        public Vector2 StartPosition { get; set; }
        public float ShapeSize { get; set; } = 40;
        public int ShapeID { get; set; }
    }
}
