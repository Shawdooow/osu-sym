using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.MathUtils;
using osu.Game.Rulesets.Shape.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Shape.Judgements;
using System.Linq;
using osu.Game.Rulesets.Scoring;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Shape.Objects.Drawables
{
    public class DrawableBaseShape : DrawableShapeHitObject<ShapeHitObject>
    {
        private BaseDial baseDial;
        private ShapeCircle circle;
        private ShapeSquare square;
        private ShapeTriangle triangle;
        private ShapeX x;

        private bool validKeyPressed;

        private readonly BaseShape shape;

        public DrawableBaseShape(BaseShape Shape) : base(Shape)
        {
            shape = Shape;
            Position = shape.StartPosition;
            Alpha = 0;
            AlwaysPresent = true;
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            LifetimeStart = shape.StartTime - (TIME_PREEMPT + 1000f);
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            if (!userTriggered || !validKeyPressed)
            {
                if (!HitObject.HitWindows.CanBeHit(timeOffset))
                {
                    AddJudgement(new ShapeJudgement { Result = HitResult.Miss });
                    Delete();
                }
                return;
            }

            var result = HitObject.HitWindows.ResultFor(timeOffset);
            if (result == HitResult.None)
                return;

            AddJudgement(new ShapeJudgement
            {
                Result = result,
                PositionOffset = Vector2.Zero
            });
            this.FadeOutFromOne(100)
                .OnComplete((s) => Delete());
        }

        public override bool OnPressed(ShapeAction action)
        {
            if (LifetimeStart <= Time.Current)
            {
                switch (shape.ShapeID)
                {
                    case 1:
                        ShapeAction[] hitActionsNorth = { ShapeAction.SouthLeftButton, ShapeAction.SouthRightButton };
                        validKeyPressed = hitActionsNorth.Contains(action);
                        return UpdateJudgement(true);
                    case 2:
                        ShapeAction[] hitActionsSouth = { ShapeAction.WestLeftButton, ShapeAction.WestRightButton };
                        validKeyPressed = hitActionsSouth.Contains(action);
                        return UpdateJudgement(true);
                    case 3:
                        ShapeAction[] hitActionsWest = { ShapeAction.EastLeftButton, ShapeAction.EastRightButton };
                        validKeyPressed = hitActionsWest.Contains(action);
                        return UpdateJudgement(true);
                    case 4:
                        ShapeAction[] hitActionsEast = { ShapeAction.NorthLeftButton, ShapeAction.NorthRightButton };
                        validKeyPressed = hitActionsEast.Contains(action);
                        return UpdateJudgement(true);
                }
            }
            return UpdateJudgement(false);
        }

        protected override void Update()
        {
            base.Update();

            if (LifetimeStart <= Time.Current)
            {
                if (Time.Current >= (shape.StartTime - TIME_PREEMPT) - 500 && !loaded)
                {
                    preLoad();
                }

                if (Time.Current >= shape.StartTime - TIME_PREEMPT && !started)
                {
                    start();
                }
            }
        }

        private bool loaded = false;
        private bool started = false;

        private void preLoad()
        {
            loaded = true;
            switch (shape.ShapeID)
            {
                case 1:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        circle = new ShapeCircle(shape) { Depth = -2, Colour = OsuColour.FromHex("#fffb14") },
                    };
                    break;
                case 2:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        square = new ShapeSquare(shape) { Depth = -2, Colour = OsuColour.FromHex("#ff2414"), },
                    };
                    break;
                case 3:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        triangle = new ShapeTriangle(shape) { Depth = -2, Colour = OsuColour.FromHex("#21b5ff"), },
                    };
                    break;
                case 4:
                    Children = new Drawable[]
                    {
                        baseDial = new BaseDial(shape)
                        {
                            Depth = -1,
                            ShapeID = shape.ShapeID,
                        },
                        x = new ShapeX(shape) { Depth = -2, Colour = OsuColour.FromHex("#a814ff"), },
                    };
                    break;
            }
        }

        private void start()
        {
            started = true;
            this.FadeIn(TIME_FADEIN);
            baseDial.StartSpinning(TIME_PREEMPT);
            switch (shape.ShapeID)
            {
                case 1:
                    circle.Position = new Vector2(RNG.Next(-200, 200), -400);
                    circle.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
                case 2:
                    square.Position = new Vector2(RNG.Next(-200, 200), -400);
                    square.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
                case 3:
                    triangle.Position = new Vector2(RNG.Next(-200, 200), -400);
                    triangle.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
                case 4:
                    x.Position = new Vector2(RNG.Next(-200, 200), -400);
                    x.MoveTo(baseDial.Position, TIME_PREEMPT);
                    break;
            }
        }
    }
}
