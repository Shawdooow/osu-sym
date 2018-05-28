using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
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
using osu.Game.Screens.Evast;
using osu.Game.Graphics.Containers;
using osu.Framework.Allocation;
using osu.Framework.IO.Stores;
using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;

namespace osu.Game.Screens.KoziLord
{
    public class KoziScreen : BeatmapScreen
    {
        public FillFlowContainer ColumnContainer;

        public Box ColumnBackground;

        public KoziScreen()
        {
            Children = new Drawable[]
            {
                new Container
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
                                        new ColumnButton(@"Testing Text"),
                                        new ColumnButton(@"Button no.2"),
                                        new ColumnButton(@"'Nother one"),
                                        new ColumnButton(@"'Nother one"),
                                        new ColumnButton(@"'Nother one"),
                                        new ColumnButton(@"'Nother one"),
                                        new ColumnButton(@"'Nother one"),
                                        new ColumnButton(@"'Nother one"),
                                        new ColumnButton(@"'Nother one"),

                                    }
                                }
                            }
                        },
                    },
                }
            };
    
        }
        private class ColumnButton : OsuClickableContainer
        {
            public ColumnButton(string title)
            {
                Anchor = Anchor.TopCentre;
                Origin = Anchor.TopCentre;
                Height = 100;
                Width = 500;
                CornerRadius = 16;
                Masking = true;
                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Colour = Color4.White.Opacity(0.1f),
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
        }

        protected override void OnEntering(Screen last)
        {
            ColumnBackground.ScaleTo(new Vector2(1, 1), 600, Easing.OutQuart);
        }
    }
}
