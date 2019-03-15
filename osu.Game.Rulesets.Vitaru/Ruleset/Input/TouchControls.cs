using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Cursor;
using osuTK;
using Sym.Base.Graphics.Containers;
using Sym.Base.Touch;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Input
{
    public class TouchControls : SymcolContainer
    {
        /// <summary>
        /// Get this fucker from the player else its over!
        /// </summary>
        public VitaruInputContainer VitaruInputContainer { get; internal set; }

        public TouchControls()
        {
            RelativeSizeAxes = Axes.Both;

            VitaruTouchWheelContainer leftHalf;
            VitaruTouchWheelContainer rightHalf;

            VitaruTouchWheelContainer.VitaruTouch up;
            VitaruTouchWheelContainer.VitaruTouch down;
            VitaruTouchWheelContainer.VitaruTouch left;
            VitaruTouchWheelContainer.VitaruTouch right;

            VitaruTouchWheelContainer.VitaruTouch upRight;
            VitaruTouchWheelContainer.VitaruTouch downRight;
            VitaruTouchWheelContainer.VitaruTouch downLeft;
            VitaruTouchWheelContainer.VitaruTouch upLeft;

            VitaruTouchWheelContainer.VitaruToggle slow;
            VitaruTouchWheelContainer.VitaruToggle shoot;
            VitaruTouchWheelContainer.VitaruToggle spell;

            VitaruTouchWheelContainer.VitaruTouch increase;
            VitaruTouchWheelContainer.VitaruTouch decrease;

            Children = new Drawable[]
            {
                //Dummy for this to work well
                new TouchContainer
                {
                    RelativeSizeAxes = Axes.Both,
                    Alpha = 0,
                },

                leftHalf = new VitaruTouchWheelContainer
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                },
                rightHalf = new VitaruTouchWheelContainer
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                },
            };

            leftHalf.Wheel.Buttons.AddRange(new TouchContainer[]
            {
                up = new VitaruTouchWheelContainer.VitaruTouch(Anchor.TopCentre),
                upRight = new VitaruTouchWheelContainer.VitaruTouch(Anchor.TopRight) { Scale = new Vector2(0.75f) },
                down = new VitaruTouchWheelContainer.VitaruTouch(Anchor.BottomCentre),
                downRight = new VitaruTouchWheelContainer.VitaruTouch(Anchor.BottomRight) { Scale = new Vector2(0.75f) },
                left = new VitaruTouchWheelContainer.VitaruTouch(Anchor.CentreLeft),
                downLeft = new VitaruTouchWheelContainer.VitaruTouch(Anchor.BottomLeft) { Scale = new Vector2(0.75f) },
                right = new VitaruTouchWheelContainer.VitaruTouch(Anchor.CentreRight),
                upLeft = new VitaruTouchWheelContainer.VitaruTouch(Anchor.TopLeft) { Scale = new Vector2(0.75f) },
            });

            rightHalf.Wheel.Buttons.AddRange(new TouchContainer[]
            {
                slow = new VitaruTouchWheelContainer.VitaruToggle(Anchor.TopCentre),
                shoot = new VitaruTouchWheelContainer.VitaruToggle(Anchor.CentreRight),
                spell = new VitaruTouchWheelContainer.VitaruToggle(Anchor.CentreLeft),

                increase = new VitaruTouchWheelContainer.VitaruTouch(Anchor.BottomLeft) { Scale = new Vector2(0.75f) },
                decrease = new VitaruTouchWheelContainer.VitaruTouch(Anchor.BottomRight) { Scale = new Vector2(0.75f) },
            });

            up.SpriteText.Text = "Up";
            up.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Up);
            up.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Up);

            down.SpriteText.Text = "Down";
            down.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Down);
            down.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Down);

            left.SpriteText.Text = "Left";
            left.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Left);
            left.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Left);

            right.SpriteText.Text = "Right";
            right.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Right);
            right.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Right);

            upRight.SpriteText.Text = "";
            upRight.OnTap += () =>
            {
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Up);
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Right);
            };
            upRight.OnRelease += () =>
            {
                VitaruInputContainer.Released?.Invoke(VitaruAction.Up);
                VitaruInputContainer.Released?.Invoke(VitaruAction.Right);
            };

            downRight.SpriteText.Text = "";
            downRight.OnTap += () =>
            {
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Down);
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Right);
            };
            downRight.OnRelease += () =>
            {
                VitaruInputContainer.Released?.Invoke(VitaruAction.Down);
                VitaruInputContainer.Released?.Invoke(VitaruAction.Right);
            };

            downLeft.SpriteText.Text = "";
            downLeft.OnTap += () =>
            {
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Down);
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Left);
            };
            downLeft.OnRelease += () =>
            {
                VitaruInputContainer.Released?.Invoke(VitaruAction.Down);
                VitaruInputContainer.Released?.Invoke(VitaruAction.Left);
            };

            upLeft.SpriteText.Text = "";
            upLeft.OnTap += () =>
            {
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Up);
                VitaruInputContainer.Pressed?.Invoke(VitaruAction.Left);
            };
            upLeft.OnRelease += () =>
            {
                VitaruInputContainer.Released?.Invoke(VitaruAction.Up);
                VitaruInputContainer.Released?.Invoke(VitaruAction.Left);
            };

            slow.SpriteText.Text = "Slow";
            slow.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Slow);
            slow.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Slow);

            shoot.SpriteText.Text = "Shoot";
            shoot.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Shoot);
            shoot.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Shoot);

            spell.SpriteText.Text = "Spell";
            spell.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Spell);
            spell.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Spell);

            increase.SpriteText.Text = "+";
            increase.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Increase);
            increase.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Increase);

            decrease.SpriteText.Text = "-";
            decrease.OnTap += () => VitaruInputContainer.Pressed?.Invoke(VitaruAction.Decrease);
            decrease.OnRelease += () => VitaruInputContainer.Released?.Invoke(VitaruAction.Decrease);
        }

        internal class VitaruTouchWheelContainer : TouchWheelContainer
        {
            public override void Tap()
            {
                base.Tap();
                Wheel.Position = SymcolCursor.VitaruCursor.CenterCircle.ToSpaceOfOtherDrawable(Vector2.Zero, this) - new Vector2(340, 380);
            }

            internal class VitaruTouch : TouchWheelContainer.TouchWheelButton
            {
                internal VitaruTouch(Anchor anchor)
                    : base(anchor)
                {
                }
            }

            internal class VitaruToggle : TouchWheelContainer.TouchWheelToggle
            {
                internal VitaruToggle(Anchor anchor)
                    : base(anchor)
                {
                }
            }
        }
    }
}
