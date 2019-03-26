using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.HitObjects.DrawableHitObjects
{
    public class DrawableTouhosuCluster : DrawableCluster
    {
        public DrawableTouhosuCluster(TouhosuCluster cluster)
            : base(cluster)
        {
        }

        public DrawableTouhosuCluster(Cluster cluster, VitaruPlayfield playfield)
            : base(cluster, playfield)
        {
        }
    }
}
