#region usings

using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects
{
    public class Laser : Projectile
    {
        /// <summary>
        /// Basically just bypasses all hitobject functionality (useful for player bullets)
        /// </summary>
        public bool DummyMode { get; set; }

        public float Width => Size.X;

        public float Height => Size.Y;

        public Vector2 Size { get; set; } = new Vector2(2, 8);
    }
}
