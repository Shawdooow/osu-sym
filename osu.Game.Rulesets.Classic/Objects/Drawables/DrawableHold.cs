// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Classic.Objects.Drawables.Pieces;
using OpenTK;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Classic.Judgements;
using System.Linq;
using osu.Game.Rulesets.Classic.UI;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Rulesets.Scoring;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableHold : DrawableClassicHitObject, IDrawableHitObjectWithProxiedApproach
    {
        public readonly Hold Hold;
        private readonly DrawableHitCircle initialCircle;
        private readonly HoldProgress progressCircle;
        private readonly CirclePiece circle;
        private readonly CircularContainer preProgressCircle;
        //internal readonly RingPiece ring;

        private readonly Container<DrawableSliderTick> ticks;
        private readonly Container<DrawableRepeatPoint> repeatPoints;

        public DrawableHold(Hold h) : base(h)
        {
            Hold = h;

            Origin = Anchor.Centre;

            Masking = false;
            AlwaysPresent = true;

            Children = new Drawable[]
            {
                progressCircle = new HoldProgress(this)
                {
                    Depth = 3,
                    Position = h.StackedPosition,
                },
                ticks = new Container<DrawableSliderTick>(),
                repeatPoints = new Container<DrawableRepeatPoint>(),
                preProgressCircle = new CircularContainer
                {
                    Origin = Anchor.Centre,
                    Depth = 3,
                    Position = h.StackedPosition,
                    Alpha = 0.5f,
                    Masking = true,
                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.Both
                        }
                    }
                },
                initialCircle = new DrawableHitCircle(new HitCircle
                {
                    //todo: avoid creating this temporary HitCircle.
                    StartTime = h.StartTime,
                    ComboIndex = h.ComboIndex,
                    IndexInCurrentCombo = h.IndexInCurrentCombo,
                    Position = h.StackedPosition,
                    Scale = h.Scale,
                    ID = h.ID,
                    SliderStartCircle = true,
                    Hidden = HitObject.Hidden,
                    First = HitObject.First,
                    Samples = h.Samples,
                    SampleControlPoint = h.SampleControlPoint,
                    TimePreempt = h.TimePreempt,
                    TimeFadein = h.TimeFadein,
                    HitWindow300 = h.HitWindow300,
                    HitWindow100 = h.HitWindow100,
                    HitWindow50 = h.HitWindow50
                }),
                //ring = new RingPiece { AlwaysPresent = true, Depth = 1, Position = h.StackedPosition, Scale = new Vector2(h.Scale) },
                circle = new CirclePiece() { Colour = AccentColour, AlwaysPresent = true, Depth = 2, Position = h.StackedPosition, Scale = new Vector2(h.Scale) },
            };

            progressCircle.Size = circle.Size * 1.25f;
            preProgressCircle.Size = circle.Size * 0.75f;

            var repeatDuration = h.Curve.Distance / h.Velocity;
            foreach (var tick in h.NestedHitObjects.OfType<SliderTick>())
            {
                var repeatStartTime = h.StartTime + tick.SpanIndex * repeatDuration;
                var fadeInTime = repeatStartTime + (tick.StartTime - repeatStartTime) / 2 - (tick.SpanIndex == 0 ? h.TimeFadein : h.TimeFadein / 2);
                var fadeOutTime = repeatStartTime + repeatDuration;

                var drawableTick = new DrawableSliderTick(tick)
                {
                    FadeInTime = fadeInTime,
                    FadeOutTime = fadeOutTime,
                    Position = tick.Position,
                };

                ticks.Add(drawableTick);
                AddNested(drawableTick);
            }

            foreach (var repeatPoint in h.NestedHitObjects.OfType<RepeatPoint>())
            {
                var repeatStartTime = h.StartTime + repeatPoint.RepeatIndex * repeatDuration;
                var fadeInTime = repeatStartTime + (repeatPoint.StartTime - repeatStartTime) / 2 - (repeatPoint.RepeatIndex == 0 ? h.TimeFadein : h.TimeFadein / 2);
                var fadeOutTime = repeatStartTime + repeatDuration;

                var drawableRepeatPoint = new DrawableRepeatPoint(repeatPoint)
                {
                    FadeInTime = fadeInTime,
                    FadeOutTime = fadeOutTime,
                    Position = repeatPoint.Position,
                };

                repeatPoints.Add(drawableRepeatPoint);
                AddNested(drawableRepeatPoint);
            }
        }

        public override Color4 AccentColour
        {
            get { return base.AccentColour; }
            set
            {
                base.AccentColour = value;
                progressCircle.Colour = AccentColour;
                preProgressCircle.Colour = AccentColour;
            }
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (Time.Current >= Hold.EndTime)
            {
                var ticksCount = ticks.Children.Count + repeatPoints.Children.Count + 2;
                var ticksHit = ticks.Children.Count(t => t.Judgements.Any(j => j.IsHit)) + repeatPoints.Children.Count(t => t.Judgements.Any(j => j.IsHit));
                double hitFraction = 0;
                if (initialCircle.Judgements.Any(j => j.IsHit))
                    ticksHit++;
                if (progressCircle.Tracking)
                    ticksHit++;

                if (ticksCount == 2)
                {
                    if (!progressCircle.Tracking && initialCircle.Judgements.Any(j => j.IsHit) || progressCircle.Tracking && !initialCircle.Judgements.Any(j => j.IsHit))
                        hitFraction = 0.5f;
                    else
                        hitFraction = (double)ticksHit / ticksCount;
                }
                else
                    hitFraction = (double)ticksHit / ticksCount;

                if (hitFraction >= 1)
                    AddJudgement(new ClassicJudgement { Result = HitResult.Great });
                else if (hitFraction >= 0.5)
                    AddJudgement(new ClassicJudgement { Result = HitResult.Good });
                else if (hitFraction > 0 || initialCircle.Judgements.Any(j => j.IsHit) || progressCircle.Tracking)
                    AddJudgement(new ClassicJudgement { Result = HitResult.Meh });
                else
                    AddJudgement(new ClassicJudgement { Result = HitResult.Miss });
            }
        }

        protected override void UpdateCurrentState(ArmedState state)
        {
            double duration = ((HitObject as IHasEndTime)?.EndTime ?? HitObject.StartTime) - HitObject.StartTime;

            switch (state)
            {
                case ArmedState.Idle:
                    this.Delay(duration + HitObject.TimePreempt).FadeOut(TIME_FADEOUT);
                    Expire(true);
                    break;
                case ArmedState.Miss:
                    if (!Hold.Hidden)
                        this.FadeOut(TIME_FADEOUT / 5);
                    Expire();
                    break;
                case ArmedState.Hit:
                    const double flash_in = 40;

                    using (BeginDelayedSequence(flash_in, true))
                    {
                        this.FadeOut(200)
                            .ScaleTo(Scale * 1.25f, 200);
                    }
                    this.Delay(800)
                        .Expire();
                    break;
            }
        }

        private int currentRepeat;
        public bool Tracking;
        private bool hidden;
        private bool started;

        protected override void Update()
        {
            base.Update();

            if (Time.Current >= Hold.StartTime && !started)
            {
                preProgressCircle.ScaleTo(new Vector2(0.5f), 200, Easing.OutQuint);
                preProgressCircle.FadeOut(200, Easing.OutQuint);
                started = true;
            }

            Tracking = progressCircle.Tracking;

            if (HitObject.Hidden && Time.Current >= HitObject.StartTime && !hidden)
            {
                progressCircle.FadeOut(Hold.Duration);
                hidden = true;
            }

            ClassicUi.BreakStartTime = Time.Current + 1000;

            double progress = MathHelper.Clamp((Time.Current - Hold.StartTime) / Hold.Duration, 0, 1);

            progressCircle.Progress.Value = progress;

            int repeat = Hold.RepeatAt(progress);
            progress = Hold.ProgressAt(progress);

            if (repeat > currentRepeat)
                currentRepeat = repeat;

            foreach (var t in ticks.Children) t.Tracking = progressCircle.Tracking;
            foreach (var r in repeatPoints.Children) r.Tracking = progressCircle.Tracking;
        }

        public Drawable ProxiedLayer => initialCircle.ApproachCircle;
    }
}
