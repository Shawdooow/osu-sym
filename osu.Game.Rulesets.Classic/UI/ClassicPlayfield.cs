// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

using System;
using System.Collections.Generic;
using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Classic.Objects;
using osu.Game.Rulesets.Classic.Objects.Drawables;
using osu.Game.Rulesets.Classic.Objects.Drawables.Connections;
using osu.Game.Rulesets.UI;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Classic.Judgements;
using osu.Game.Rulesets.Classic.Settings;
using osu.Game.Rulesets.Classic.UI.Cursor;
using OpenTK.Graphics.ES30;
using Symcol.Core.Graphics.Containers;

namespace osu.Game.Rulesets.Classic.UI
{
    public class ClassicPlayfield : Playfield
    {
        public static Action<Judgement> OnJudgement;

        private readonly Container approachCircles;
        private readonly Container judgementLayer;
        private readonly ConnectionRenderer<ClassicHitObject> connectionLayer;

        private readonly Bindable<bool> fast = ClassicSettings.ClassicConfigManager.GetBindable<bool>(ClassicSetting.Accelerando);

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();

        private double startTime = double.MinValue;
        private double endTime = double.MaxValue;

        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);

        private readonly ClassicInputManager classicInputManager;

        public ClassicPlayfield(ClassicInputManager classicInputManager) : base(BASE_SIZE.X)
        {
            this.classicInputManager = classicInputManager;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            ClassicUi.BreakStartTime = -10000;

            AddRange(new Drawable[]
            {
                connectionLayer = new FollowPointRenderer
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = 2,
                },
                judgementLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = 1,
                },
                approachCircles = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Depth = -1,
                }
            });
        }

        [BackgroundDependencyLoader]
        private void load(BindableBeatmap beatmap)
        {
            workingBeatmap.BindTo(beatmap);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddInternal(new GameplayCursor());
        }

        protected override void Update()
        {
            base.Update();

            if (fast)
                applyToClock(workingBeatmap.Value.Track, getSpeed(Time.Current) < 0.75f ? 0.75f : getSpeed(Time.Current));

            foreach (ClassicHitObject h in hitobjects)
            {
                if (Time.Current >= h.StartTime - h.TimePreempt * 2)
                {
                    add(h);
                    hitobjects.Remove(h);
                    break;
                }
            }
        }

        private double getSpeed(double value)
        {
            double scale = (1.5 - 0.75) / (endTime - startTime);
            return 0.75 + (value - startTime) * scale;
        }

        private void applyToClock(IAdjustableClock clock, double speed)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust = speed;
        }

        private readonly List<ClassicHitObject> hitobjects = new List<ClassicHitObject>();

        private void add(ClassicHitObject h, DrawableHitObject drawable = null)
        {
            if (drawable == null)
                switch (h)
                {
                    case HitCircle c:
                        drawable = new DrawableHitCircle(c);
                        break;
                    case Slider s:
                        drawable = new DrawableSlider(s);
                        break;
                    case Hold r:
                        drawable = new DrawableHold(r);
                        break;
                    case Spinner s:
                        drawable = new DrawableSpinner(s);
                        break;
                }

            drawable.OnJudgement += onJudgement;

            drawable.Anchor = Anchor.TopLeft;
            //drawable.Origin = Anchor.Centre;

            if (h is IDrawableHitObjectWithProxiedApproach p)
                approachCircles.Add(p.ProxiedLayer.CreateProxy());

            double q = 0;

            if (drawable is DrawableSlider d && d.HitObject is Slider slider)
                q = slider.EndTime;
            else if (drawable is DrawableSpinner u && u.HitObject is Spinner spinner)
                q = spinner.EndTime;
            else
                q = drawable.HitObject.StartTime;

            SymcolBufferedContainer b = new SymcolBufferedContainer
            {
                Depth = (float)drawable.HitObject.StartTime,
                RelativeSizeAxes = Axes.Both,
            };
            b.Child = new PlayfieldHitobjectContainer(drawable, q);
            b.Attach(RenderbufferInternalFormat.DepthComponent16);

            classicInputManager.SliderBodyContainer.Add(b);
        }

        public override void Add(DrawableHitObject h)
        {
            if (startTime == double.MinValue)
                startTime = h.HitObject.StartTime;

            if (h is DrawableSlider s && s.HitObject is Slider slider)
                endTime = slider.EndTime;
            else if (h is DrawableSpinner p && p.HitObject is Spinner spinner)
                endTime = spinner.EndTime;
            else
                endTime = h.HitObject.StartTime;

            hitobjects.Add((ClassicHitObject)h.HitObject);
        }

        public override void PostProcess()
        {
            connectionLayer.HitObjects = HitObjects.Objects
                .Select(d => d.HitObject)
                .OrderBy(h => h.StartTime).OfType<ClassicHitObject>();
        }

        private void onJudgement(DrawableHitObject judgedObject, Judgement judgement)
        {
            var osuJudgement = (ClassicJudgement)judgement;
            var osuObject = (ClassicHitObject)judgedObject.HitObject;

            OnJudgement?.Invoke(osuJudgement);

            DrawableClassicJudgement explosion = new DrawableClassicJudgement(osuJudgement, judgedObject)
            {
                Origin = Anchor.Centre,
                Position = osuObject.StackedEndPosition + osuJudgement.PositionOffset
            };

            judgementLayer.Add(explosion);
        }

        protected override void Dispose(bool isDisposing)
        {
            OnJudgement = null;
            base.Dispose(isDisposing);
        }

        private class PlayfieldHitobjectContainer : Container
        {
            private readonly DrawableClassicHitObject hitobject;
            private readonly double endTime;

            public PlayfieldHitobjectContainer(DrawableHitObject hitobject, double endTime)
            {
                this.hitobject = (DrawableClassicHitObject)hitobject;
                this.endTime = endTime;

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                Size = BASE_SIZE;
                Add(hitobject);
            }

            protected override void Update()
            {
                base.Update();
                Scale = new Vector2(Parent.DrawSize.Y * 4 / 3 / Size.X, Parent.DrawSize.Y / Size.Y) * 0.8f;

                if (Time.Current >= endTime + 1000)
                {
                    SymcolBufferedContainer parent = (SymcolBufferedContainer)Parent;
                    parent.Delete();
                }
            }
        }
    }
}
