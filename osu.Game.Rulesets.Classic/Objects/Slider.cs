// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using osu.Game.Rulesets.Objects.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Classic.Objects
{
    public class Slider : ClassicHitObject
    {
        public Vector2 StackedPositionAt(double t) => StackedPosition + PositionAt(t);

        /// <summary>
        /// The position of the cursor at the point of completion of this <see cref="Slider"/> if it was hit
        /// with as few movements as possible. This is set and used by difficulty calculation.
        /// </summary>
        internal Vector2? LazyEndPosition;

        /// <summary>
        /// The distance travelled by the cursor upon completion of this <see cref="Slider"/> if it was hit
        /// with as few movements as possible. This is set and used by difficulty calculation.
        /// </summary>
        internal float LazyTravelDistance;

        private int stackHeight;

        public override int StackHeight
        {
            get { return stackHeight; }
            set
            {
                stackHeight = value;
                Curve.Offset = StackOffset;
            }
        }

        protected override void CreateNestedHitObjects()
        {
            base.CreateNestedHitObjects();

            createTicks();
            createRepeatPoints();
        }

        private void createTicks()
        {
            if (TickDistance == 0) return;

            var length = Curve.Distance;
            var tickDistance = Math.Min(TickDistance, length);
            var spanDuration = length / Velocity;

            var minDistanceFromEnd = Velocity * 0.01;

            for (var span = 0; span < this.SpanCount(); span++)
            {
                var spanStartTime = StartTime + span * spanDuration;
                var reversed = span % 2 == 1;

                for (var d = tickDistance; d <= length; d += tickDistance)
                {
                    if (d > length - minDistanceFromEnd)
                        break;

                    var distanceProgress = d / length;
                    var timeProgress = reversed ? 1 - distanceProgress : distanceProgress;

                    var firstSample = Samples.FirstOrDefault(s => s.Name == SampleInfo.HIT_NORMAL) ?? Samples.FirstOrDefault(); // TODO: remove this when guaranteed sort is present for samples (https://github.com/ppy/osu/issues/1933)
                    var sampleList = new List<SampleInfo>();

                    if (firstSample != null)
                        sampleList.Add(new SampleInfo
                        {
                            Bank = firstSample.Bank,
                            Volume = firstSample.Volume,
                            Name = @"slidertick",
                        });

                    AddNested(new SliderTick
                    {
                        SpanIndex = span,
                        StartTime = spanStartTime + timeProgress * spanDuration,
                        Position = PositionAt(distanceProgress),
                        StackHeight = StackHeight,
                        Scale = Scale,
                        Samples = sampleList
                    });
                }
            }
        }

        private void createRepeatPoints()
        {
            var repeatDuration = Distance / Velocity;

            for (int repeatIndex = 0, repeat = 1; repeatIndex < RepeatCount; repeatIndex++, repeat++)
            {
                var repeatStartTime = StartTime + repeat * repeatDuration;

                AddNested(new RepeatPoint
                {
                    RepeatIndex = repeatIndex,
                    StartTime = repeatStartTime,
                    Position = PositionAt(repeat % 2),
                    StackHeight = StackHeight,
                    Scale = Scale,
                });
            }
        }
    }
}
