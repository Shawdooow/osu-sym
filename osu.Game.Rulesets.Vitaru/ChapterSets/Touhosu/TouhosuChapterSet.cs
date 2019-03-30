#region usings

using osu.Game.Rulesets.Vitaru.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Rational;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Scarlet;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Worship;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.HitObjects;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.HitObjects.DrawableHitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu
{
    public class TouhosuChapterSet : ChapterSet
    {
        public override string Name => "Touhosu";

        public override string Description => "The original bullet dodging experiance, bullets are sent your way to the beat.";

        public override Chapter[] GetChapters() => new TouhosuChapter[]
        {
            new WorshipChapter(),
            new ScarletChapter(),
            new RationalChapter(), 
        };

        public override Cluster GetCluster() => new TouhosuCluster();

        public override DrawableCluster GetDrawableCluster(Cluster cluster, VitaruPlayfield playfield) => new DrawableTouhosuCluster((TouhosuCluster)cluster, playfield);

        public override Bullet GetBullet() => new Bullet();

        public override DrawableBullet GetDrawableBullet(Bullet bullet, VitaruPlayfield playfield) => new DrawableBullet(bullet, playfield);

        public override Laser GetLaser() => new Laser();

        public override DrawableLaser GetDrawableLaser(Laser laser, VitaruPlayfield playfield) => new DrawableLaser(laser, playfield);

        public override Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawablePattern) => new Enemy(playfield, (DrawableTouhosuCluster)drawablePattern);
    }
}
