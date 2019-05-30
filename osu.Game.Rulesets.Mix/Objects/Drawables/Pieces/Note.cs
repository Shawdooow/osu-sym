using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using System;

namespace osu.Game.Rulesets.Mix.Objects.Drawables.Pieces
{
    public class Note : BeatSyncedContainer
    {
        private readonly CircularContainer circle;

        public Note(MixNote note)
        {
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                circle = new CircularContainer
                {
                    RelativeSizeAxes = Axes.Both,

                    Masking = true,

                    BorderColour = Color4.White,
                    BorderThickness = 8,

                    EdgeEffect = new EdgeEffectParameters
                    {
                        Colour = note.Color.Opacity(0.5f),
                        Radius = 8,
                        Type = EdgeEffectType.Shadow
                    },

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = ColourInfo.GradientVertical(note.Color.Darken(0.4f), note.Color.Lighten(0.4f))
                        },
                        new Triangles
                        {
                            RelativeSizeAxes = Axes.Both,

                            TriangleScale = 1,
                            ColourLight = note.Color.Lighten(0.4f),
                            ColourDark = note.Color.Darken(0.4f)
                        }
                    }
                }
            };

            if (note.Whistle)
            {
                Add(new Triangle
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                    Rotation = 180,
                    Size = new Vector2(0.4f)
                });
            }
            else if (note.Finish)
            {
                Add(new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                    Size = new Vector2(0.4f),
                    CornerRadius = 2,
                    Child = new Box { RelativeSizeAxes = Axes.Both }
                });
            }
            else if (note.Clap)
            {
                Add(new CircularContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                    Size = new Vector2(0.4f),
                    Masking = true,
                    Child = new Box { RelativeSizeAxes = Axes.Both }
                });
            }
            else
            {

            }
        }
    }
}
