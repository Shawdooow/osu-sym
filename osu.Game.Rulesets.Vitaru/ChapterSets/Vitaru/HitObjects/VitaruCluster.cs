#region usings

using System.Collections.Generic;
using osu.Framework.Logging;
using osu.Game.Audio;
using osu.Game.Rulesets.Vitaru.Ruleset;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Vitaru.HitObjects
{
    public class VitaruCluster : Cluster
    {
        public override double TimePreempt => Preemt;
        public override double TimeUnPreempt => 1200;

        protected double Preemt = 600;

        protected override List<Projectile> ProcessProjectiles(List<Projectile> projectiles)
        {
            foreach (Projectile p in projectiles)
                if (p.StartTime < StartTime - Preemt)
                    Preemt = StartTime - p.StartTime;

            return base.ProcessProjectiles(projectiles);
        }

        protected override List<Projectile> GetConvertCluster(Vector2 pos, int id)
        {
            if (!VitaruSettings.Experimental) return base.GetConvertCluster(pos, id);

            switch (id)
            {
                default:
                    return new List<Projectile>();
                case 1:
                    return Patterns.Follow(ClusterSpeed * Velocity * 2, ClusterDiameter, ClusterDamage, StartTime, Team, ClusterDensity);
                case 7:
                    return Patterns.Cross(ClusterSpeed * Velocity * 2, ClusterDiameter, ClusterDamage, StartTime, Team, ClusterDensity);
            }
        }

        protected override int GetConvertPatternID(SampleInfo info)
        {
            if (!VitaruSettings.Experimental) return base.GetConvertPatternID(info);

            if (IsSpinner) return 6;

            switch (info.Bank)
            {
                default:
                    Logger.Log($"Bad SampleInfo: {info.Bank} - {info.Name}", LoggingTarget.Database, LogLevel.Error);
                    return 1;
                case "normal" when info.Name == "hitnormal":
                    return 1;
                case "normal" when info.Name == "hitwhistle":
                    return 2;
                case "normal" when info.Name == "hitfinish":
                    return 7;
                case "normal" when info.Name == "hitclap":
                    return 5;
                case "drum" when info.Name == "hitnormal":
                    return 1;
                case "drum" when info.Name == "hitwhistle":
                    return 2;
                case "drum" when info.Name == "hitfinish":
                    return 3;
                case "drum" when info.Name == "hitclap":
                    return 4;
                case "soft" when info.Name == "hitnormal":
                    return 1;
                case "soft" when info.Name == "hitwhistle":
                    return 2;
                case "soft" when info.Name == "hitfinish":
                    return 7;
                case "soft" when info.Name == "hitclap":
                    return 5;
            }
        }
    }
}
