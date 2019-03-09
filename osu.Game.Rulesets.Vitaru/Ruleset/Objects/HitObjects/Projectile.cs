namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects
{
    public class Projectile : VitaruHitObject, IHasTeam
    {
        public int Team { get; set; }

        public double Damage { get; set; } = 10;

        public double Angle { get; set; }
    }
}
