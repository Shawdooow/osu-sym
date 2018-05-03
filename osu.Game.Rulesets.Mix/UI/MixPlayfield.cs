using osu.Framework.Graphics;
using osu.Game.Rulesets.Mix.Objects;
using osu.Game.Rulesets.Mix.Objects.Drawables;
using OpenTK;
using osu.Game.Rulesets.Mix.Judgements;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.UI.Scrolling;
using OpenTK.Graphics;
using osu.Framework.Graphics.Shapes;

namespace osu.Game.Rulesets.Mix.UI
{
    public class MixPlayfield : ScrollingPlayfield
    {
        public const float DEFAULT_HEIGHT = 120;

        public MixPlayfield() : base(ScrollingDirection.Left)
        {
            AddRange(new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f,
                }
            });
        }

        public override void Add(DrawableHitObject h)
        {
            h.OnJudgement += onJudgement;

            base.Add(h);
        }

        private void onJudgement(DrawableHitObject judgedObject, Judgement judgement)
        {
            var shapeJudgement = (MixJudgement)judgement;
            var shapeObject = (MixHitObject)judgedObject.HitObject;

            DrawableMixJudgement explosion = new DrawableMixJudgement(shapeJudgement, judgedObject)
            {
                Scale = new Vector2(0.5f),
                Alpha = 0.5f,
                Origin = Anchor.Centre,
                Position = judgedObject.Position,
            };

            //judgementLayer.Add(explosion);
        }
    }
}
