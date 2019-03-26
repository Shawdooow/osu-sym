#region usings

using osu.Game.Rulesets.Edit;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Edit.Blueprints.ClusterBlueprints
{
    public class ClusterPlacementBlueprint : PlacementBlueprint
    {
        public new Cluster HitObject => (Cluster)base.HitObject;

        public ClusterPlacementBlueprint()
            : base(ChapterStore.GetChapterSet(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode)).GetCluster())
        {
        }
    }
}
