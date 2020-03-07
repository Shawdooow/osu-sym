#region usings

using System;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Overlays.Toolbar;
using osuTK.Graphics;

#endregion

namespace osu.Core.Containers.SymcolToolbar
{
    public class ToolbarSystemClock : Container
    {
        private OnlineIndicator indicator;

        public ToolbarSystemClock()
        {
            RelativeSizeAxes = Axes.Y;
            AutoSizeAxes = Axes.X;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            Add(new SystemIndicator
            {
                Action = () => indicator.ToggleVisibility()
            });
            Add(indicator = new OnlineIndicator());
        }

        private class SystemIndicator : ToolbarButton
        {
            private static DateTime t = DateTime.Now;
            private string time = t.ToString("hh:mm:ss tt");
            SpriteText clockText;

            public SystemIndicator()
            {
                RelativeSizeAxes = Axes.Y;
                AutoSizeAxes = Axes.X;

                Anchor = Anchor.TopCentre;
                Origin = Anchor.TopCentre;

                clockText = new SpriteText
                {
                    Font = @"Exo2.0-Medium",
                    Text = time,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Position = new osuTK.Vector2(0, 4),
                    TextSize = 28,
                    Colour = Color4.White,
                };
                Add(clockText);
            }

            protected override void Update()
            {
                t = DateTime.Now;
                time = t.ToString("hh:mm:ss tt");
                clockText.Text = time;
            }
        }

        private class OnlineIndicator : OsuFocusedOverlayContainer
        {
            private const float transition_time = 400;

            public OnlineIndicator()
            {
                BypassAutoSizeAxes = Axes.Both;
                Position = new osuTK.Vector2(0, 1);
                RelativePositionAxes = Axes.Y;
                Anchor = Anchor.TopCentre;
                Origin = Anchor.TopCentre;
            }

            [BackgroundDependencyLoader]
            private void load(OsuColour colours)
            {
                Children = new Drawable[]
                {
                    new OsuContextMenuContainer
                    {
                        Width = 200,
                        AutoSizeAxes = Axes.Y,

                        Anchor = Anchor.TopCentre,
                        Origin = Anchor.TopCentre,

                        Children = new Drawable[]
                        {
                            new Box
                            {
                                RelativeSizeAxes = Axes.Both,
                                Colour = Color4.Black,
                                Alpha = 0.6f,
                            },
                            new Container
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Masking = true,
                                AutoSizeDuration = transition_time,
                                AutoSizeEasing = Easing.OutQuint,
                                Children = new Drawable[]
                                {
                                    new SpriteText()
                                    {
                                        Font = @"Exo2.0-Medium",
                                        Text = "Coming Soon...",
                                        Anchor = Anchor.TopCentre,
                                        Origin = Anchor.TopCentre,
                                        Position = new osuTK.Vector2(0, 4),
                                        TextSize = 28,
                                        Colour = Color4.White,
                                    },
                                    new Box
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        Anchor = Anchor.BottomLeft,
                                        Origin = Anchor.BottomLeft,
                                        Height = 3,
                                        Colour = colours.Yellow,
                                        Alpha = 1,
                                    },
                                }
                            }
                        }
                    }
                };
            }

            protected override void PopIn()
            {
                base.PopIn();
                this.FadeIn(transition_time, Easing.OutQuint);
            }

            protected override void PopOut()
            {
                base.PopOut();
                this.FadeOut(transition_time);
            }
        }
    }
}
