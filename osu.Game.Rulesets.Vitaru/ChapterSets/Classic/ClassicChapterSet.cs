#region usings

using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Vitaru.ChapterSets.Classic.HitObjects;
using osu.Game.Rulesets.Vitaru.ChapterSets.Classic.HitObjects.DrawableHitObjects;
using osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.Chapters;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Classic
{
    public class ClassicChapterSet : ChapterSet
    {
        public override string Name => "Classic";

        public override string Description => "osu! classic";

        public override Texture Icon => VitaruRuleset.VitaruTextures.Get("Classic/icon");

        public override Chapter[] GetChapters() => new Chapter[]
        {
            new RejectChapter(),
        };

        public override Cluster GetCluster() => new ClassicHitObject();

        public override DrawableCluster GetDrawableCluster(Cluster cluster, VitaruPlayfield playfield) => new DrawableClassicHitObject(cluster as ClassicHitObject, playfield);

        public override Bullet GetBullet() => throw new System.NotImplementedException();

        public override DrawableBullet GetDrawableBullet(Bullet bullet, VitaruPlayfield playfield) => throw new System.NotImplementedException();

        public override Laser GetLaser() => throw new System.NotImplementedException();

        public override DrawableLaser GetDrawableLaser(Laser laser, VitaruPlayfield playfield) => throw new System.NotImplementedException();

        public override Enemy GetEnemy(VitaruPlayfield playfield, DrawableCluster drawablePattern) => throw new System.NotImplementedException();
    }
}
