using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Cursor
{
    public class SymcolCursor : CursorContainer
    {
        //public override MultiCursorContainer CreateMultiCursor() => new SymcolCursor();

        protected override Drawable CreateCursor() => new VitaruCursor();

        public SymcolCursor()
        {
            Masking = false;
        }

        public class VitaruCursor : Container
        {
            public static CircularContainer CenterCircle;

            public VitaruCursor()
            {
                Origin = Anchor.Centre;
                Size = new Vector2(32);
                Masking = false;
            }

            [BackgroundDependencyLoader]
            private void load(OsuConfigManager config, OsuColour osu)
            {
                Children = new Drawable[]
                {
                    new Sprite
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(Size.X + Size.X / 3.5f),
                        Texture = VitaruRuleset.VitaruTextures.Get("ring")
                    },
                    CenterCircle = new CircularContainer
                    {
                        Masking = true,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(Size.X / 5),
                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    },
                    new Container
                    {
                        Masking = false,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Rotation = 45,

                        Children = new Drawable[]
                        {
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(Size.X / 3, 0),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(-1 * Size.X / 3, 0),
                                Rotation = 180,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(0, Size.X / 3),
                                Rotation = 90,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new Container
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                CornerRadius = Size.X / 12,
                                Size = new Vector2(Size.X / 3, Size.X / 7),
                                Position = new Vector2(0, -1 * Size.X / 3),
                                Rotation = 270,
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    },
                    new Container
                    {
                        Masking = false,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,

                        Children = new Drawable[]
                        {
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(Size.X / 4, 0),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(-1 * Size.X / 4, 0),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(0, Size.X / 4),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            },
                            new CircularContainer
                            {
                                Masking = true,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Size = new Vector2(Size.X / 8),
                                Position = new Vector2(0, -1 * Size.X / 4),
                                Child = new Box
                                {
                                    RelativeSizeAxes = Axes.Both
                                }
                            }
                        }
                    }
                };
            }
        }
    }
}
