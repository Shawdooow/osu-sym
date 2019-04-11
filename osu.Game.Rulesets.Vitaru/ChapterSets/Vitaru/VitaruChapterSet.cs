#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.Chapters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.HitObjects;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.HitObjects.DrawableHitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru
{
    public class VitaruChapterSet : ChapterSet
    {
        public override string Name => "Vitaru";

        public override string Description => $"The movement gamemode, Vitaru is all about moving out of the way to the beat.";

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

        public override Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawableCluster) => new Enemy(playfield, (DrawableVitaruCluster)drawableCluster);
    }
}
