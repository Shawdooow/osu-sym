using System;
using System.Collections.Generic;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Beatmaps
{
    internal class VitaruBeatmapConverter : BeatmapConverter<VitaruHitObject>
    {
        protected override IEnumerable<Type> ValidConversionTypes { get; } = new[] { typeof(IHasPosition) };

        public VitaruBeatmapConverter(IBeatmap beatmap)
        : base(beatmap)
        {
        }

        protected override IEnumerable<VitaruHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap)
        {
            VitaruGamemode gamemode = ChapterStore.GetGamemode(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

            var endTimeData = original as IHasEndTime;
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;

            float complexity = 1;

            float ar = calculateAr(beatmap.BeatmapInfo.BaseDifficulty.ApproachRate);
            float cs = 28 + (beatmap.BeatmapInfo.BaseDifficulty.CircleSize - 4);
            float speed = 0.25f;

            if (gamemode is DodgeGamemode)
            {
                complexity *= 0.66f;
                cs *= 0.5f;
                ar *= 0.5f;
                speed *= 0.5f;
            }

            Cluster c = gamemode.GetCluster();

            c.Convert = true;

            c.BetterSamples = original.Samples;

            c.Ar = ar;

            c.StartTime = original.StartTime;
            c.Position = positionData?.Position ?? Vector2.Zero;
            c.ClusterDensity = complexity;
            c.ClusterDiameter = cs;
            c.ClusterSpeed = speed;
            c.NewCombo = comboData?.NewCombo ?? false;
            c.ComboOffset = comboData?.ComboOffset ?? 0;

            c.Position += gamemode.ClusterOffset;

            if (original is IHasCurve curveData)
            {
                c.IsSlider = true;
                c.Path = curveData.Path;
                c.NodeSamples = curveData.NodeSamples;
                c.RepeatCount = curveData.RepeatCount;
                c.EnemyHealth = 60;
                c.NodeSamples = curveData.NodeSamples;
            }
            else if (endTimeData != null)
            {
                c.IsSpinner = true;
                c.ClusterSpeed = 0.4f;
                c.EnemyHealth = 180;
                c.ClusterDamage = 10;
                c.EndTime = endTimeData.EndTime;
            }

            yield return c;
        }

        private float calculateAr(float ar)
        {
            if (ar >= 5)
            {
                ar = 1200 - (ar - 5) * 150;
                return ar;
            }
            else
            {
                ar = 1800 - ar * 120;
                return ar;
            }
        }

        protected override Beatmap<VitaruHitObject> CreateBeatmap() => new VitaruBeatmap();
    }
}
