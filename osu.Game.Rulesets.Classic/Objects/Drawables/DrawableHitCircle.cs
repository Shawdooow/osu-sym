// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Classic.Objects.Drawables.Pieces;
using OpenTK;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Classic.Judgements;
using osu.Game.Rulesets.Classic.Beatmaps;
using osu.Game.Rulesets.Classic.UI;
using osu.Game.Rulesets.Scoring;
using OpenTK.Graphics;
using Symcol.Rulesets.Core.Skinning;
using osu.Game.Audio;

namespace osu.Game.Rulesets.Classic.Objects.Drawables
{
    public class DrawableHitCircle : DrawableClassicHitObject, IDrawableHitObjectWithProxiedApproach
    {
        public ApproachCircle ApproachCircle;
        private readonly CirclePiece circle;
        private readonly RingPiece ring;
        private readonly NumberPiece number;
        private readonly ClassicHitObject hitObject;
        private bool jiggling;
        private double savedTime;

        public DrawableHitCircle(ClassicHitObject h) : base(h)
        {
            hitObject = h;

            Origin = Anchor.Centre;

            AlwaysPresent = true;

            Position = HitObject.StackedPosition;
            Scale = new Vector2(HitObject.Scale);

            Children = new Drawable[]
            {
                circle = new CirclePiece
                {
                    Hit = () =>
                    {
                        if (AllJudged)
                            return false;

                        UpdateJudgement(true);
                        return true;
                    },
                },
                number = new NumberPiece
                {
                    AlwaysPresent = true,
                    Text = h is Spinner ? 0 : (HitObject.IndexInCurrentCombo + 1)
                },
                ring = new RingPiece { AlwaysPresent = true },
                ApproachCircle = new ApproachCircle
                {
                    Alpha = 0,
                }
            };

            foreach (SampleInfo info in h.BetterSamples)
            {
                SymcolSkinnableSound sound;
                SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                Add(sound);
            }

            //may not be so correct
            Size = circle.DrawSize;
        }

        public override Color4 AccentColour
        {
            get { return base.AccentColour; }
            set
            {
                base.AccentColour = value;
                circle.Colour = AccentColour;
                ApproachCircle.Colour = AccentColour;
            }
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (!userTriggered)
            {
                if (timeOffset > HitObject.HitWindowFor(HitResult.Meh))
                {
                    AddJudgement(new ClassicJudgement { Result = HitResult.Miss });
                    ClassicBeatmapConverter.CurrentHitCircle = hitObject.ID + 1;
                }
                return;
            }

            if (ClassicBeatmapConverter.CurrentHitCircle < hitObject.ID || timeOffset <= -400)
            {
                jiggle();
            }

            else if (ClassicBeatmapConverter.CurrentHitCircle >= hitObject.ID)
            {
                if(!hitObject.SliderStartCircle)
                AddJudgement(new ClassicJudgement
                {
                    Result = HitObject.ScoreResultForOffset(Math.Abs(timeOffset)),
                    PositionOffset = Vector2.Zero //todo: set to correct value
                });
                else if (hitObject.SliderStartCircle)
                {
                    AddJudgement(new ClassicJudgement
                    {
                        Result = HitResult.Great,
                        PositionOffset = Vector2.Zero //todo: set to correct value
                    });
                }
                PlayBetterSamples();

                if (ClassicBeatmapConverter.CurrentHitCircle <= hitObject.ID)
                {
                    ClassicBeatmapConverter.CurrentHitCircle = hitObject.ID + 1;
                }
            }
        }

        protected override void UpdateInitialState()
        {
            base.UpdateInitialState();

            // sane defaults
            ring.Show();
            circle.Show();
            number.Show();

            ApproachCircle.ScaleTo(new Vector2(4));
        }

        protected override void UpdatePreemptState()
        {
            base.UpdatePreemptState();

            if (!hitObject.Hidden || hitObject.First)
                ApproachCircle.FadeIn(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt));
            ApproachCircle.ScaleTo(1.1f, HitObject.TimePreempt);
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
                    if (!hitObject.Hidden)
                    {
                        ApproachCircle.FadeOut(50);
                        this.FadeOut(TIME_FADEOUT / 5);
                    }
                    if (hitObject.Hidden && hitObject.First)
                        ApproachCircle.Expire();
                    Expire();
                    break;
                case ArmedState.Hit:
                    if (!hitObject.Hidden)
                        ApproachCircle.FadeOut(50);
                    if (hitObject.Hidden && hitObject.First)
                        ApproachCircle.Expire();

                    const double flash_in = 40;

                    using (BeginDelayedSequence(flash_in, true))
                    {
                        if (!hitObject.Hidden)
                        {
                            this.FadeOut(300, Easing.OutQuad)
                                .ScaleTo(Scale * 1.25f, 300, Easing.OutQuart);
                        }
                    }

                    Expire();
                    break;
            }
        }

        private bool hidden;

        protected override void Update()
        {
            base.Update();

            ClassicUi.BreakStartTime = Time.Current + 1000;

            if (hitObject.Hidden && Time.Current >= (hitObject.StartTime - HitObject.TimePreempt) + HitObject.TimeFadein && !hidden)
            {
                ring.FadeOut(HitObject.TimeFadein);
                circle.FadeOut(HitObject.TimeFadein);
                number.FadeOut(HitObject.TimeFadein);
                hidden = true;
            }

            if (savedTime + 200 <= Time.Current)
                jiggling = false;
        }

        private void jiggle()
        {
            if (!jiggling && !hitObject.Hidden)
            {
                jiggling = true;
                savedTime = Time.Current;
                this.MoveTo(new Vector2(Position.X - 8, Position.Y), 50, Easing.OutSine)
                    .Delay(50)
                    .MoveTo(hitObject.StackedPosition, 150, Easing.OutElastic);
            }
        }

        public Drawable ProxiedLayer => ApproachCircle;
    }
}
