#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Chapters;
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

        public override string Description => "The default gamemode in this ruleset which is based on the touhou series danmaku games. " +
                                              "Allows you to kill enemies while dodging bullets to the beat!";

        public override VitaruChapter[] GetChapters() => new[]
        {
            new VitaruChapter(),
        };

        public override Cluster GetCluster() => new Cluster();

        public override DrawableCluster GetDrawableCluster(Cluster cluster, VitaruPlayfield playfield) => new DrawableCluster(cluster, playfield);

        public override Bullet GetBullet() => new Bullet();

        public override DrawableBullet GetDrawableBullet(Bullet bullet, VitaruPlayfield playfield) => new DrawableBullet(bullet, playfield);

        public override Laser GetLaser() => new Laser();

        public override DrawableLaser GetDrawableLaser(Laser laser, VitaruPlayfield playfield) => new DrawableLaser(laser, playfield);

        public override Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawablePattern) => new Enemy(playfield, drawablePattern);
    }
}
