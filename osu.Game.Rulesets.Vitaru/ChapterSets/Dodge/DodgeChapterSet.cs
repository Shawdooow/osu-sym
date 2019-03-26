#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.Chapters;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Dodge
{
    public class DodgeChapterSet : ChapterSet
    {
        public override string Name => "Dodge";

        public override VitaruChapter[] GetChapters() => new VitaruChapter[]
        {
            new VitaruChapter(),
        };

        public override Cluster GetCluster()
        {
            throw new System.NotImplementedException();
        }

        public override DrawableCluster GetDrawableCluster(Cluster cluster, VitaruPlayfield playfield)
        {
            throw new System.NotImplementedException();
        }

        public override Bullet GetBullet()
        {
            throw new System.NotImplementedException();
        }

        public override DrawableBullet GetDrawableBullet(Bullet bullet, VitaruPlayfield playfield)
        {
            throw new System.NotImplementedException();
        }

        public override Laser GetLaser()
        {
            throw new System.NotImplementedException();
        }

        public override DrawableLaser GetDrawableLaser(Laser laser, VitaruPlayfield playfield)
        {
            throw new System.NotImplementedException();
        }

        public override Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawablePattern)
        {
            throw new System.NotImplementedException();
        }
    }
}
