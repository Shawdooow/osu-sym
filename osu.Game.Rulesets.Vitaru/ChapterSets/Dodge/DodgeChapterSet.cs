#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.Chapters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.HitObjects;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.HitObjects.DrawableHitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Dodge
{
    public class DodgeChapterSet : ChapterSet
    {
        public override string Name => $"Dodge";

        public override string Description => $"The simple dodging experiance, no shooting, no killing, no place to hide.";

        public override Vector2 PlayfieldAspectRatio => new Vector2(2, 1);

        public override Vector2 PlayfieldSize => new Vector2(512 + 256, 384);

        public override Vector2 PlayerStartingPosition => PlayfieldSize / 2;

        public override Vector2 ClusterOffset => new Vector2(128, 0);

        public override Chapter[] GetChapters() => new Chapter[]
        {
            new RejectChapter(),
        };

        public override Cluster GetCluster() => new VitaruCluster();

        public override DrawableCluster GetDrawableCluster(Cluster cluster, VitaruPlayfield playfield) => new DrawableVitaruCluster((VitaruCluster)cluster, playfield);

        public override Bullet GetBullet() => new Bullet();

        public override DrawableBullet GetDrawableBullet(Bullet bullet, VitaruPlayfield playfield) => new DrawableBullet(bullet, playfield);

        public override Laser GetLaser() => new Laser();

        public override DrawableLaser GetDrawableLaser(Laser laser, VitaruPlayfield playfield) => new DrawableLaser(laser, playfield);

        public override Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawablePattern) => new Enemy(playfield, (DrawableVitaruCluster)drawablePattern);
    }
}
