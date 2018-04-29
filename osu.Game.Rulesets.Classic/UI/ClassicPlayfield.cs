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
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Classic.Judgements;
using osu.Game.Rulesets.Classic.UI.Cursor;

namespace osu.Game.Rulesets.Classic.UI
{
    public class ClassicPlayfield : Playfield
    {
        private readonly Container approachCircles;
        private readonly Container judgementLayer;
        private readonly ConnectionRenderer<ClassicHitObject> connectionLayer;

        //public override bool ProvidingUserCursor => true;

        public static readonly Vector2 BASE_SIZE = new Vector2(512, 384);

        public override Vector2 Size
        {
            get
            {
                var parentSize = Parent.DrawSize;
                var aspectSize = parentSize.X * 0.75f < parentSize.Y ? new Vector2(parentSize.X, parentSize.X * 0.75f) : new Vector2(parentSize.Y * 4f / 3f, parentSize.Y);

                return new Vector2(aspectSize.X / parentSize.X, aspectSize.Y / parentSize.Y) * base.Size;
            }
        }

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

        protected override void LoadComplete()
        {
            base.LoadComplete();
            AddInternal(new GameplayCursor());
        }

        public override void Add(DrawableHitObject h)
        {
            h.Depth = (float)h.HitObject.StartTime;

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
