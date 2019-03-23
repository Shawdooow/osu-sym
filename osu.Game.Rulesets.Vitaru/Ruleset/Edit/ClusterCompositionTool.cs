#region usings

using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Edit.Tools;
using osu.Game.Rulesets.Vitaru.Ruleset.Edit.Blueprints.ClusterBlueprints;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit
{
    public class ClusterCompositionTool : HitObjectCompositionTool
    {
        public ClusterCompositionTool()
            : base(nameof(Cluster))
        {
        }

        public override PlacementBlueprint CreatePlacementBlueprint() => new ClusterPlacementBlueprint();
    }
}
