#region usings

using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects
{
    public class Laser : Projectile
    {
        public float Width => Size.X;

        public float Height => Size.Y;

        public Vector2 Size { get; set; } = new Vector2(2, 8);
    }
}
