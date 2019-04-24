using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Classic.HitObjects.DrawableHitObjects
{
    public class DrawableClassicHitObject : DrawableCluster
    {
        public DrawableClassicHitObject(ClassicHitObject hitobject)
            : base(hitobject)
        {
        }

        public DrawableClassicHitObject(ClassicHitObject hitobject, VitaruPlayfield playfield)
            : base(hitobject, playfield)
        {
        }
    }
}
