namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects
{
    public class Projectile : VitaruHitObject, IHasTeam
    {
        public int Team { get; set; }

        public float Damage { get; set; } = 10;

        public float Angle { get; set; }
    }
}
