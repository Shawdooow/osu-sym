#region usings

using System.Collections.Generic;
using System.Linq;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Beatmaps
{
    public class VitaruBeatmap : Beatmap<VitaruHitObject>
    {
        public override IEnumerable<BeatmapStatistic> GetStatistics()
        {
            int patterns = 0;
            int bullets = 0;
            int lasers = 0;

            foreach (VitaruHitObject obj in HitObjects)
            {
                if (obj is Cluster pattern)
                {
                    patterns++;
                    foreach (Bullet unused in pattern.GetProjectiles().OfType<Bullet>())
                        bullets++;
                    foreach (Laser unused in pattern.GetProjectiles().OfType<Laser>())
                        lasers++;
                }
            }

            return new[]
            {
                new BeatmapStatistic
                {
                    Name = @"Cluster Count",
                    Content = patterns.ToString(),
                    Icon = FontAwesome.fa_circle_o
                },
                new BeatmapStatistic
                {
                    Name = @"Bullet Count",
                    Content = bullets.ToString(),
                    Icon = FontAwesome.fa_circle
                },
                new BeatmapStatistic
                {
                    Name = @"Laser Count",
                    Content = lasers.ToString(),
                    Icon = FontAwesome.fa_circle
                },
            };
        }
    }
}
