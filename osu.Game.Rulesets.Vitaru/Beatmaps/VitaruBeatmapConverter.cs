using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Settings;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    internal class VitaruBeatmapConverter : BeatmapConverter<VitaruHitObject>
    {
        private readonly Bindable<Gamemodes> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);
        private readonly bool multiplayer = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ShittyMultiplayer);
        private readonly int enemyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.EnemyPlayerCount);

        public static List<HitObject> HitObjectList = new List<HitObject>();

        protected override IEnumerable<Type> ValidConversionTypes { get; } = new[] { typeof(IHasPosition) };

        public VitaruBeatmapConverter(IBeatmap beatmap)
        : base(beatmap)
        {
        }

        protected override IEnumerable<VitaruHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap)
        {
            var endTimeData = original as IHasEndTime;
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;

            double complexity = 1;

            float ar = calculateAr(beatmap.BeatmapInfo.BaseDifficulty.ApproachRate);
            float cs = 20 + (beatmap.BeatmapInfo.BaseDifficulty.CircleSize - 4);
            double speed = 0.2d;

            if (gamemode == Gamemodes.Dodge || gamemode == Gamemodes.Gravaru)
            {
                complexity *= 0.66f;
                cs *= 0.5f;
                ar *= 0.5f;
                speed *= 0.5d;
            }

            Pattern p = new Pattern
            {
                Convert = true,

                BetterSamples = original.Samples,

                Ar = ar,
                StartTime = original.StartTime,
                Position = positionData?.Position ?? Vector2.Zero,
                PatternComplexity = complexity,
                PatternTeam = 1,
                PatternDiameter = cs,
                PatternSpeed = speed,
                NewCombo = comboData?.NewCombo ?? false,
            };

            if (gamemode == Gamemodes.Touhosu)
                p.Position = new Vector2(p.Position.X + 256, p.Position.Y);

            if (original is IHasCurve curveData)
            {
                p.IsSlider = true;
                p.ControlPoints = curveData.ControlPoints;
                p.CurveType = curveData.CurveType;
                p.Distance = curveData.Distance;
                p.RepeatCount = curveData.RepeatCount;
                p.EnemyHealth = 60;
                p.RepeatSamples = curveData.RepeatSamples;
            }
            else if (endTimeData != null)
            {
                p.IsSpinner = true;
                p.PatternSpeed = 0.25f;
                p.EnemyHealth = 180;
                p.PatternDamage = 10;
                p.EndTime = endTimeData.EndTime;
            }

            if (multiplayer && enemyPlayerCount > 0)
                HitObjectList.Add(p);

            yield return p;
        }

        private float calculateAr(float ar)
        {
            if (ar >= 5)
            {
                ar = 1200 - ((ar - 5) * 150);
                return ar;
            }
            else
            {
                ar = 1800 - (ar * 120);
                return ar;
            }
        }
    }
}
