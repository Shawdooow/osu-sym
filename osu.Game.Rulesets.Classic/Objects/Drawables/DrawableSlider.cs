// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Classic.Objects.Drawables.Pieces;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Classic.Judgements;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Objects.Types;
using OpenTK.Graphics;
using osu.Game.Audio;
using Symcol.Rulesets.Core.Skinning;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableSlider : DrawableClassicHitObject, IDrawableHitObjectWithProxiedApproach
    {
        private readonly Slider slider;

        private readonly DrawableHitCircle initialCircle;

        private readonly List<Drawable> components = new List<Drawable>();

        private readonly Container<DrawableSliderTick> ticks;
        private readonly Container<DrawableRepeatPoint> repeatPoints;

        public readonly SliderBody Body;
        public readonly SliderBall Ball;

        public DrawableSlider(Slider s) : base(s)
        {
            slider = s;

            Position = s.StackedPosition;

            Children = new Drawable[]
            {
                Body = new SliderBody(s)
                {
                    PathWidth = s.Scale * 64,
                },
                ticks = new Container<DrawableSliderTick>(),
                repeatPoints = new Container<DrawableRepeatPoint>(),
                Ball = new SliderBall(s)
                {
                    Scale = new Vector2(s.Scale),
                },
                initialCircle = new DrawableHitCircle(new HitCircle
                {
                    StartTime = s.StartTime,
                    ComboIndex = s.ComboIndex,
                    IndexInCurrentCombo = s.IndexInCurrentCombo,
                    Scale = s.Scale,
                    ID = s.ID,
                    SliderStartCircle = true,
                    Hidden = s.Hidden,
                    First = s.First,
                    SampleControlPoint = s.SampleControlPoint,
                    TimePreempt = s.TimePreempt,
                    TimeFadein = s.TimeFadein,
                    HitWindow300 = s.HitWindow300,
                    HitWindow100 = s.HitWindow100,
                    HitWindow50 = s.HitWindow50
                })
            };

            foreach (SampleInfo info in slider.GetRepeatSamples(0))
            {
                SymcolSkinnableSound sound;
                initialCircle.SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                initialCircle.Add(sound);
            }

            foreach (SampleInfo info in slider.GetRepeatSamples(1))
            {
                SymcolSkinnableSound sound;
                SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                Add(sound);
            }

            components.Add(Body);
            components.Add(Ball);

            var spanDuration = s.Curve.Distance / s.Velocity;
            foreach (var tick in s.NestedHitObjects.OfType<SliderTick>())
            {
                var spanStartTime = s.StartTime + tick.SpanIndex * spanDuration;
                var fadeInTime = spanStartTime + (tick.StartTime - spanStartTime) / 2 - (tick.SpanIndex == 0 ? HitObject.TimeFadein : HitObject.TimeFadein / 2);
                var fadeOutTime = spanStartTime + spanDuration;

                var drawableTick = new DrawableSliderTick(tick)
                {
                    FadeInTime = fadeInTime,
                    FadeOutTime = fadeOutTime,
                    Position = tick.Position,
                    Alpha = 0,
                    AlwaysPresent = true,
                };

                ticks.Add(drawableTick);
                AddNested(drawableTick);
            }
            foreach (var repeatPoint in s.NestedHitObjects.OfType<RepeatPoint>())
            {
                var repeatStartTime = s.StartTime + (repeatPoint.RepeatIndex + 1) * spanDuration;
                var fadeInTime = repeatStartTime + (repeatPoint.StartTime - repeatStartTime) / 2 - (repeatPoint.RepeatIndex == 0 ? HitObject.TimeFadein : HitObject.TimeFadein / 2);
                var fadeOutTime = repeatStartTime + spanDuration;

                var drawableRepeatPoint = new DrawableRepeatPoint(repeatPoint)
                {
                    FadeInTime = fadeInTime,
                    FadeOutTime = fadeOutTime,
                    Position = repeatPoint.Position,
                    Alpha = 0,
                    AlwaysPresent = true,
                };

                repeatPoints.Add(drawableRepeatPoint);
                components.Add(drawableRepeatPoint);
                AddNested(drawableRepeatPoint);
            }
        }

        public override Color4 AccentColour
        {
            get { return base.AccentColour; }
            set
            {
                base.AccentColour = value;
                Body.AccentColour = AccentColour;
                Ball.AccentColour = AccentColour;
            }
        }

        private int currentSpan;
        public bool Tracking;
        private bool hidden;

        protected override void Update()
        {
            base.Update();

            ClassicUi.BreakStartTime = Time.Current + 1000;

            if (HitObject.Hidden && Time.Current >= HitObject.StartTime && !hidden)
            {
                Body.FadeOut(slider.Duration);
                hidden = true;
            }

            if (Time.Current >= slider.EndTime && slider.Hidden)
                Body.Expire();

            Tracking = Ball.Tracking;

            double progress = MathHelper.Clamp((Time.Current - slider.StartTime) / slider.Duration, 0, 1);

            int span = slider.SpanAt(progress);
            progress = slider.ProgressAt(progress);

            if (span > currentSpan && span < slider.SpanCount())
            {
                currentSpan = span;
                if (Ball.Tracking)
                    PlayBetterRepeatSamples(span + 1);
            }

            //todo: we probably want to reconsider this before adding scoring, but it looks and feels nice.
            if (!initialCircle.Judgements.Any(j => j.IsHit))
                initialCircle.Position = slider.PositionAt(progress);

            foreach (var c in components.OfType<ISliderProgress>()) c.UpdateProgress(progress, span);
            foreach (var c in components.OfType<ITrackSnaking>()) c.UpdateSnakingPosition(slider.PositionAt(Body.SnakedStart ?? 0), slider.PositionAt(Body.SnakedEnd ?? 0));
            foreach (var t in ticks.Children) t.Tracking = Tracking;
            foreach (var r in repeatPoints.Children) r.Tracking = Tracking;
        }

        protected void PlayBetterRepeatSamples(int repeat)
        {
            PlayBetterSamples();

            foreach (SymcolSkinnableSound sound in SymcolSkinnableSounds)
                sound.Delete();

            SymcolSkinnableSounds = new List<SymcolSkinnableSound>();

            foreach (SampleInfo info in slider.GetRepeatSamples(repeat))
            {
                SymcolSkinnableSound sound;

                slider.SampleControlPoint = slider.GetSampleControlPoint(repeat);

                SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                Add(sound);
            }
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (Time.Current >= slider.EndTime)
            {
                var ticksCount = ticks.Children.Count + repeatPoints.Children.Count + 2;
                var ticksHit = ticks.Children.Count(t => t.Judgements.Any(j => j.IsHit)) + repeatPoints.Children.Count(t => t.Judgements.Any(j => j.IsHit));
                double hitFraction = 0;
                if (initialCircle.Judgements.Any(j => j.IsHit))
                    ticksHit++;
                if (Ball.Tracking)
                    ticksHit++;

                if (ticksCount == 2)
                {
                    if (!Ball.Tracking && initialCircle.Judgements.Any(j => j.IsHit) || Ball.Tracking && !initialCircle.Judgements.Any(j => j.IsHit))
                        hitFraction = 0.5f;
                    else
                        hitFraction = (double)ticksHit / ticksCount;
                }
                else
                    hitFraction = (double)ticksHit / ticksCount;

                if (hitFraction >= 1)
                {
                    PlayBetterRepeatSamples(0);
                    AddJudgement(new ClassicJudgement { Result = HitResult.Great });
                }
                else if (hitFraction >= 0.5)
                {
                    PlayBetterRepeatSamples(0);
                    AddJudgement(new ClassicJudgement { Result = HitResult.Good });
                }
                else if (hitFraction > 0 || initialCircle.Judgements.Any(j => j.IsHit) || Ball.Tracking)
                {
                    PlayBetterRepeatSamples(0);
                    AddJudgement(new ClassicJudgement { Result = HitResult.Meh });
                }
                else
                    AddJudgement(new ClassicJudgement { Result = HitResult.Miss });
            }
        }

        protected override void UpdateInitialState()
        {
            base.UpdateInitialState();
            Body.Alpha = 1;

            //we need to be present to handle input events. note that we still don't get enough events (we don't get a position if the mouse hasn't moved since the slider appeared).
            Ball.AlwaysPresent = true;
            Ball.Alpha = 0;
        }

        protected override void UpdateCurrentState(ArmedState state)
        {
            Ball.FadeIn();

            using (BeginDelayedSequence(slider.Duration, true))
            {
                Ball.FadeOut(160);

                if (!HitObject.Hidden)
                {
                    Body.FadeOut(160);

                    this.FadeOut(800)
                        .Expire();
                }
                else
                {
                    this.Delay(800)
                        .Expire();
                }
            }

        }

        public Drawable ProxiedLayer => initialCircle.ApproachCircle;
    }

    internal interface ISliderProgress
    {
        void UpdateProgress(double progress, int repeat);
    }
}
