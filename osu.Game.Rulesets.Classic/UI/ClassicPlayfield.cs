// Copyright (c) 2007-2017 ppy Pty Ltd <contact@ppy.sh>.
// Licensed under the MIT Licence - https://raw.githubusercontent.com/ppy/osu/master/LICENCE

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

namespace osu.Game.Rulesets.Classic.UI
{
    public class ClassicPlayfield : Playfield
    {
        private readonly Container approachCircles;
        private readonly Container judgementLayer;
        private readonly ConnectionRenderer<ClassicHitObject> connectionLayer;

        private readonly Bindable<bool> fast = ClassicSettings.ClassicConfigManager.GetBindable<bool>(ClassicSetting.Accelerando);

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();

        private double startTime = double.MinValue;
        private double endTime = double.MaxValue;

        //public override bool ProvidingUserCursor => true;

        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);

        public ClassicPlayfield() : base(BASE_SIZE.X)
        {
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
        private void load(OsuGameBase game)
        {
            workingBeatmap.BindTo(game.Beatmap);
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
        }

        private double getSpeed(double value)
        {
            double scale = (1.5 - 0.75) / (endTime - startTime);
            return 0.75 + ((value - startTime) * scale);
        }

        private void applyToClock(IAdjustableClock clock, double speed)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust = speed;
        }

        public override void Add(DrawableHitObject h)
        {
            h.Depth = (float)h.HitObject.StartTime;

            if (startTime == double.MinValue)
                startTime = h.HitObject.StartTime;

            if (h is DrawableSlider s && s.HitObject is Slider slider)
                endTime = slider.EndTime;
            else if (h is DrawableSpinner p && p.HitObject is Spinner spinner)
                endTime = spinner.EndTime;
            else
                endTime = h.HitObject.StartTime;

            h.OnJudgement += onJudgement;

            var c = h as IDrawableHitObjectWithProxiedApproach;
            if (c != null)
                approachCircles.Add(c.ProxiedLayer.CreateProxy());

            base.Add(h);
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

            DrawableClassicJudgement explosion = new DrawableClassicJudgement(osuJudgement, judgedObject)
            {
                Origin = Anchor.Centre,
                Position = osuObject.StackedEndPosition + osuJudgement.PositionOffset
            };

            judgementLayer.Add(explosion);
        }
    }
}
