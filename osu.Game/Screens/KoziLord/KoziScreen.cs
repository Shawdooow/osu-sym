using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Screens.Backgrounds;
using osu.Game.Screens.Menu;
using osu.Game.Graphics.Sprites;
using osu.Game.Screens.Symcol.Pieces;
using osu.Game.Screens.Symcol.Screens;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Screens.KoziLord;
using osu.Game.Screens.KoziLord.MusicPlayer;
using osu.Game.Screens.Evast;
using osu.Game.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using System;

namespace osu.Game.Screens.KoziLord
{
    public class KoziScreen : BeatmapScreen
    {
        public FillFlowContainer ColumnContainer;

        public Box ColumnBackground;

        public ColumnButton ColumnElement;

        public Container MainContainer;

        public KoziScreen()
        {
            Children = new Drawable[]
            {
                MainContainer = new Container
                {
                    RelativeSizeAxes = Axes.Y,
                    Width = 600,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Children = new Drawable[]
                    {
                        ColumnBackground = new Box
                        {
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            Scale = new Vector2(0,1),
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black.Opacity(0.4f),
                        },
                        new ScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Children = new Drawable[]
                            {
                                ColumnContainer = new FillFlowContainer
                                {
                                    Padding = new MarginPadding{Top = 20},
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Spacing = new Vector2(0,20),
                                    Direction = FillDirection.Vertical,
                                    Children = new Drawable[]
                                    {
                                       ColumnElement = new ColumnButton(@"FullScreen Player", () => Push(new FullscreenPlayer())),//Action crashes right now for some reason ¯\_(ツ)_/¯ 
                                       ColumnElement = new ColumnButton(@"Dummy button"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),
                                       ColumnElement = new ColumnButton(@"'Nother one"),

                                    }
                                }
                            }
                        },
                    },
                }
            };
    
        }
        public class ColumnButton : OsuClickableContainer
        {
            public Box ItemBackground;
            public ColumnButton(string title, Action onPressed = null)
            {
                Action = onPressed;

                Alpha = 0;
                Scale = new Vector2(0.8f);
                Anchor = Anchor.TopCentre;
                Origin = Anchor.TopCentre;
                Height = 100;
                Width = 500;
                CornerRadius = 16;
                Masking = true;
                Children = new Drawable[]
                {
                   ItemBackground  = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White.Opacity(0.2f),
                        Alpha = 0.5f
                    },
                    new OsuSpriteText
                    {
                        TextSize = 26,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Text = title
                    }
                };
             
            }
            
            protected override bool OnHover(InputState state)
            {
                ItemBackground.FadeIn(50, Easing.Out);
                return base.OnHover(state);
            }

            protected override void OnHoverLost(InputState state)
            {
                ItemBackground.FadeTo(0.5f, 150, Easing.Out);
                base.OnHoverLost(state);
            }
        }
        protected override void OnEntering(Screen last)
        {

            int delaySequence = 0;
            foreach (ColumnButton button in ColumnContainer)
            {
                button.Delay(50 * delaySequence)
                    .FadeInFromZero(600, Easing.Out)
                    .ScaleTo(1, 400, Easing.OutCubic);
                delaySequence++;
            }
            
            ColumnBackground.ScaleTo(new Vector2(1, 1), 600, Easing.OutQuart);
        }
       /* protected override bool OnExiting(Screen next)
        {
            MainContainer.FadeOut(200, Easing.In);
            ColumnBackground.ScaleTo(new Vector2(0, 1), 200, Easing.InCubic);

            return base.OnExiting(next);
        }*/

    }
}
