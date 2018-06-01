using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Vitaru.Objects;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Game.Audio;
using System.Linq;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Beatmaps.ControlPoints;

namespace osu.Game.Rulesets.Vitaru.Beatmaps
{
    internal class VitaruBeatmapConverter : BeatmapConverter<VitaruHitObject>
    {
        private readonly Gamemodes currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);
        private readonly bool multiplayer = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ShittyMultiplayer);
        private readonly int enemyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.EnemyPlayerCount);

        public static List<HitObject> HitObjectList = new List<HitObject>();

        protected override IEnumerable<Type> ValidConversionTypes { get; } = new[] { typeof(IHasPosition) };

        /// <summary>
        /// osu! is generally slower than taiko, so a factor is added to increase
        /// speed. This must be used everywhere slider length or beat length is used.
        /// </summary>
        private const float legacy_velocity_multiplier = 1.4f;

        /// <summary>
        /// Base osu! slider scoring distance.
        /// </summary>
        private const float osu_base_scoring_distance = 100;

        /// <summary>
        /// Drum roll distance that results in a duration of 1 speed-adjusted beat length.
        /// </summary>
        private const float shape_base_distance = 100;

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

            SampleControlPoint controlPoint = beatmap.ControlPointInfo.SamplePointAt(original.StartTime);

            bool isDrum = controlPoint.SampleBank == "drum";
            bool isSoft = controlPoint.SampleBank == "soft";

            if (original.Samples.Any(s => s.Bank != null))
            {
                if (original.Samples.Any(s => s.Name == "drums"))
                    isDrum = true;
                if (original.Samples.Any(s => s.Name == "soft"))
                    isSoft = true;
            }

            bool isWhistle = original.Samples.Any(s => s.Name == SampleInfo.HIT_WHISTLE);
            bool isFinish = original.Samples.Any(s => s.Name == SampleInfo.HIT_FINISH);
            bool isClap = original.Samples.Any(s => s.Name == SampleInfo.HIT_CLAP);

            if (currentGameMode == Gamemodes.Dodge || currentGameMode == Gamemodes.Gravaru)
            {
                complexity *= 0.66f;
                cs *= 0.5f;
                ar *= 0.5f;
                speed *= 0.5d;
            }

            int patternID = 1;

            if (isDrum)
            {
                if (isWhistle)
                    patternID = 4;
                else if (isFinish)
                    patternID = 3;
                else if (isClap)
                    patternID = 5;
                else
                    patternID = 1;
            }
            else if (isSoft)
            {
                if (isWhistle)
                    patternID = 2;
                else if (isFinish)
                    patternID = 3;
                else if (isClap)
                    patternID = 5;
                else
                    patternID = 1;
            }
            else
            {
                if (isWhistle)
                    patternID = 4;
                else if (isFinish)
                    patternID = 3;
                else if (isClap)
                    patternID = 5;
                else
                    patternID = 1;
            }

            Pattern p = new Pattern
            {
                Convert = true,

                Drum = isDrum,
                Soft = isSoft,

                Whistle = isWhistle,
                Finish = isFinish,
                Clap = isClap,

                Ar = ar,
                StartTime = original.StartTime,
                Position = positionData?.Position ?? Vector2.Zero,
                PatternComplexity = complexity,
                PatternTeam = 1,
                PatternDiameter = cs,
                PatternSpeed = speed,
                PatternID = patternID,
                NewCombo = comboData?.NewCombo ?? false,
            };

            if (original is IHasCurve curveData)
            {
                // Number of spans of the object - one for the initial length and for each repeat
                int spans = curveData?.SpanCount() ?? 1;

                TimingControlPoint timingPoint = beatmap.ControlPointInfo.TimingPointAt(original.StartTime);
                DifficultyControlPoint difficultyPoint = beatmap.ControlPointInfo.DifficultyPointAt(original.StartTime);

                double speedAdjustment = difficultyPoint.SpeedMultiplier;
                double speedAdjustedBeatLength = timingPoint.BeatLength / speedAdjustment;

                double distance = curveData.Distance * spans * legacy_velocity_multiplier;

                double shapeVelocity = shape_base_distance * beatmap.BeatmapInfo.BaseDifficulty.SliderMultiplier * legacy_velocity_multiplier / speedAdjustedBeatLength;
                double shapeDuration = distance / shapeVelocity;

                // The velocity of the osu! hit object - calculated as the velocity of a slider
                double osuVelocity = osu_base_scoring_distance * beatmap.BeatmapInfo.BaseDifficulty.SliderMultiplier * legacy_velocity_multiplier / speedAdjustedBeatLength;
                // The duration of the osu! hit object
                double osuDuration = distance / osuVelocity;

                // osu-stable always uses the speed-adjusted beatlength to determine the velocities, but
                // only uses it for tick rate if beatmap version < 8
                if (beatmap.BeatmapInfo.BeatmapVersion >= 8)
                    speedAdjustedBeatLength *= speedAdjustment;

                // If the drum roll is to be split into hit circles, assume the ticks are 1/8 spaced within the duration of one beat
                double tickSpacing = Math.Min(speedAdjustedBeatLength / beatmap.BeatmapInfo.BaseDifficulty.SliderTickRate, shapeDuration / spans);

                List<List<SampleInfo>> allSamples = curveData != null ? curveData.RepeatSamples : new List<List<SampleInfo>>(new[] { original.Samples });

                int i = 0;
                for (double j = original.StartTime; j <= original.StartTime + shapeDuration + tickSpacing / 8; j += tickSpacing)
                {
                    List<SampleInfo> currentSamples = allSamples[i];

                    string bank = original.SampleControlPoint.SampleBank;

                    if (currentSamples.Any(s => s.Bank != null))
                    {
                        if (currentSamples.Any(s => s.Name == "normal"))
                            bank = "normal";
                        else if (currentSamples.Any(s => s.Name == "drum"))
                            bank = "drum";
                        else if (currentSamples.Any(s => s.Name == "soft"))
                            bank = "soft";
                    }

                    List<SampleInfo> samplesList = new List<SampleInfo>()
                    {
                        new SampleInfo()
                        {
                            Bank = bank,
                            Name = "hitnormal",
                            Volume = original.SampleControlPoint.SampleVolume,
                        }
                    };

                    if (currentSamples.Any(s => s.Name == SampleInfo.HIT_WHISTLE))
                        samplesList.Add(new SampleInfo()
                        {
                            Bank = bank,
                            Name = "hitwhistle",
                            Volume = original.SampleControlPoint.SampleVolume,
                        });

                    if (currentSamples.Any(s => s.Name == SampleInfo.HIT_FINISH))
                        samplesList.Add(new SampleInfo()
                        {
                            Bank = bank,
                            Name = "hitfinish",
                            Volume = original.SampleControlPoint.SampleVolume,
                        });

                    if (currentSamples.Any(s => s.Name == SampleInfo.HIT_CLAP))
                        samplesList.Add(new SampleInfo()
                        {
                            Bank = bank,
                            Name = "hitclap",
                            Volume = original.SampleControlPoint.SampleVolume,
                        });

                    p.BetterRepeatSamples.Add(samplesList);

                    i = (i + 1) % allSamples.Count;
                }

                p.IsSlider = true;
                p.ControlPoints = curveData.ControlPoints;
                p.CurveType = curveData.CurveType;
                p.Distance = curveData.Distance;
                p.RepeatCount = curveData.RepeatCount;
                p.EnemyHealth = 60;

                if (isFinish)
                    p.PatternSpeed = 0.25f;
            }
            else if (endTimeData != null)
            {
                p.IsSpinner = true;
                p.PatternSpeed = 0.25f;
                p.EnemyHealth = 180;
                p.PatternDamage = 10;
                p.PatternID = 6;
                p.EndTime = endTimeData.EndTime;
            }
            else
                if (isFinish)
                    p.PatternSpeed = 0.3f;

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
