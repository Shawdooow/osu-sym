using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit.Blueprints.ClusterBlueprints
{
    public class ClusterPlacementBlueprint : PlacementBlueprint
    {
        public new Cluster HitObject => (Cluster)base.HitObject;

        public ClusterPlacementBlueprint()
            : base(new Cluster())
        {
        }
    }
}
