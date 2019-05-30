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
using osu.Framework.Logging;
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
                            BetterRepeatSamples = curveData.RepeatSamples,
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
                            BetterRepeatSamples = curveData.RepeatSamples,
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
                            RepeatSamples = curveData.RepeatSamples,
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
                            RepeatSamples = curveData.RepeatSamples,
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
                    BetterSamples = original.Samples,

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
                        BetterSamples = original.Samples,
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
                        BetterSamples = original.Samples,
                        StartTime = original.StartTime,
                        Position = positionData?.Position ?? Vector2.Zero,
                        NewCombo = comboData?.NewCombo ?? false,
                        ID = hitCircleCount,
                        First = true
                    };
                }
            }
        }
    }
}
