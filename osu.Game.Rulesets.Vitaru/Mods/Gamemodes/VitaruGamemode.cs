using osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Mods.Gamemodes
{
    public class VitaruGamemode
    {
        public virtual string Name => "Vitaru";

        public virtual VitaruChapter[] GetChapters() => new[]
        {
            new VitaruChapter(), 
        };

        public virtual float PlayfieldMargin => 0.8f;

        public virtual Vector2 PlayfieldAspectRatio => new Vector2(5, 8);

        public virtual Vector2 PlayfieldSize => new Vector2(512, 820);

        public virtual Vector4 PlayfieldBounds => new Vector4(0, 0, PlayfieldSize.X, PlayfieldSize.Y);

        public virtual Vector2 PlayerStartingPosition => new Vector2(PlayfieldSize.X / 2, 700);

        public virtual Vector2 ClusterOffset => Vector2.Zero;

        public virtual Cluster GetCluster() => new Cluster();

        public virtual DrawableCluster GetDrawableCluster(Cluster cluster, VitaruPlayfield playfield) => new DrawableCluster(cluster, playfield);

        public virtual Bullet GetBullet() => new Bullet();

        public virtual DrawableBullet GetDrawableBullet(Bullet bullet, VitaruPlayfield playfield) => new DrawableBullet(bullet, playfield);

        public virtual Laser GetLaser() => new Laser();

        public virtual DrawableLaser GetDrawableLaser(Laser laser, VitaruPlayfield playfield) => new DrawableLaser(laser, playfield);

        public virtual Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawablePattern) => new Enemy(playfield, drawablePattern);

        public virtual string Description => "The default gamemode in this ruleset which is based on the touhou series danmaku games. " +
                                                     "Allows you to kill enemies while dodging bullets to the beat!";
    }
}
