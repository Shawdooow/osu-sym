// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Classic.Objects;
using System.Collections.Generic;
using osu.Game.Rulesets.Objects.Types;
using System;
using osu.Game.Rulesets.Classic.UI;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Classic.Settings;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Classic.Beatmaps
{
    internal class ClassicBeatmapConverter : BeatmapConverter<ClassicHitObject>
    {
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

        private readonly Bindable<bool> hold = ClassicSettings.ClassicConfigManager.GetBindable<bool>(ClassicSetting.Hold);

        private int hitCircleCount = 0;
        public static int CurrentHitCircle = 1;

        private bool firstObject = true;

        public ClassicBeatmapConverter(IBeatmap beatmap)
        : base(beatmap)
        {
        }

        protected override IEnumerable<Type> ValidConversionTypes { get; } = new[] { typeof(IHasPosition) };

        protected override IEnumerable<ClassicHitObject> ConvertHitObject(HitObject original, IBeatmap beatmap)
        {
            var curveData = original as IHasCurve;
            var endTimeData = original as IHasEndTime;
            var positionData = original as IHasPosition;
            var comboData = original as IHasCombo;

            if (CurrentHitCircle > 1)
                CurrentHitCircle = 1;

            SampleControlPoint controlPoint = beatmap.ControlPointInfo.SamplePointAt(original.StartTime);

            List<List<SampleInfo>> betterRepeatSamples = new List<List<SampleInfo>>();

            if (curveData != null)
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

                    betterRepeatSamples.Add(getBetterSampleInfoList(currentSamples, original));

                    i = (i + 1) % allSamples.Count;
                }
            }
            if (curveData != null)
            {
                hitCircleCount++;
                if (curveData.Distance < 60 && hold || curveData.Distance < 120 && curveData.RepeatCount == 2 && hold || curveData.RepeatCount > 3 && hold)
                {
                    if (!firstObject)
                        yield return new Hold
                        {
                            StartTime = original.StartTime,
                            ControlPoints = curveData.ControlPoints,
                            CurveType = curveData.CurveType,
                            Distance = curveData.Distance,
                            BetterRepeatSamples = betterRepeatSamples,
                            RepeatCount = curveData.RepeatCount,
                            Position = positionData?.Position ?? Vector2.Zero,
                            NewCombo = comboData?.NewCombo ?? false,
                            ID = hitCircleCount
                        };
                    else
                    {
                        firstObject = false;
                        yield return new Hold
                        {
                            StartTime = original.StartTime,
                            ControlPoints = curveData.ControlPoints,
                            CurveType = curveData.CurveType,
                            Distance = curveData.Distance,
                            BetterRepeatSamples = betterRepeatSamples,
                            RepeatCount = curveData.RepeatCount,
                            Position = positionData?.Position ?? Vector2.Zero,
                            NewCombo = comboData?.NewCombo ?? false,
                            ID = hitCircleCount,
                            First = true
                        };
                    }
                }
                else
                {
                    if (!firstObject)
                        yield return new Slider
                        {
                            StartTime = original.StartTime,
                            ControlPoints = curveData.ControlPoints,
                            CurveType = curveData.CurveType,
                            Distance = curveData.Distance,
                            RepeatSamples = betterRepeatSamples,
                            RepeatCount = curveData.RepeatCount,
                            Position = positionData?.Position ?? Vector2.Zero,
                            NewCombo = comboData?.NewCombo ?? false,
                            ID = hitCircleCount
                        };
                    else
                    {
                        firstObject = false;
                        yield return new Slider
                        {
                            StartTime = original.StartTime,
                            ControlPoints = curveData.ControlPoints,
                            CurveType = curveData.CurveType,
                            Distance = curveData.Distance,
                            RepeatSamples = betterRepeatSamples,
                            RepeatCount = curveData.RepeatCount,
                            Position = positionData?.Position ?? Vector2.Zero,
                            NewCombo = comboData?.NewCombo ?? false,
                            ID = hitCircleCount,
                            First = true
                        };
                    }
                }
            }
            else if (endTimeData != null)
            {
                yield return new Spinner
                {
                    BetterSamples = getBetterSampleInfoList(original.Samples, original),

                    StartTime = original.StartTime,
                    EndTime = endTimeData.EndTime,

                    Position = positionData?.Position ?? ClassicPlayfield.BASE_SIZE / 2
                };
            }
            else
            {
                hitCircleCount++;
                if (!firstObject)
                    yield return new HitCircle
                    {
                        BetterSamples = getBetterSampleInfoList(original.Samples, original),
                        StartTime = original.StartTime,
                        Position = positionData?.Position ?? Vector2.Zero,
                        NewCombo = comboData?.NewCombo ?? false,
                        ID = hitCircleCount
                    };
                else
                {
                    firstObject = false;
                    yield return new HitCircle
                    {
                        BetterSamples = getBetterSampleInfoList(original.Samples, original),
                        StartTime = original.StartTime,
                        Position = positionData?.Position ?? Vector2.Zero,
                        NewCombo = comboData?.NewCombo ?? false,
                        ID = hitCircleCount,
                        First = true
                    };
                }
            }
        }

        private List<SampleInfo> getBetterSampleInfoList(List<SampleInfo> list, HitObject original)
        {
            return list;
        }
    }
}
