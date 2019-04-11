#region usings

using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables
{
    public abstract class DrawableCluster : DrawableVitaruHitObject
    {
        public static Bindable<int> CLUSTER_COUNT = new Bindable<int>();

        protected readonly bool disable_bullets = VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.DisableBullets);

        public new readonly Cluster HitObject;

        protected DrawableCluster(Cluster cluster)
            : base(cluster)
        {
        }

        protected DrawableCluster(Cluster cluster, VitaruPlayfield playfield)
            : base(cluster, playfield)
        {
            AlwaysPresent = true;

            HitObject = cluster;
        }
    }
}
