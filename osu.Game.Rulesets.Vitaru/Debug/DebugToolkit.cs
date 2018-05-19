using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Debug
{
    public class DebugToolkit : Container
    {
        public static List<Container> DebugItems = new List<Container>();

        private readonly FillFlowContainer<Container> debugItems;

        public DebugToolkit()
        {
            OsuColour osu = new OsuColour();

            Anchor = Anchor.CentreLeft;
            Origin = Anchor.CentreLeft;

            Position = new Vector2(20, 0);

            RelativeSizeAxes = Axes.X;
            Width = 0.18f;
            AutoSizeAxes = Axes.Y;

            Masking = true;
            CornerRadius = 4;
            BorderColour = Color4.White;
            BorderThickness = 4;

            Children = new Drawable[]
            {
                new Box
                {
                    Colour = ColourInfo.GradientVertical(osu.Green.Darken(0.4f), osu.Green.Lighten(0.4f)),
                    RelativeSizeAxes = Axes.Both
                },
                new Triangles
                {
                    RelativeSizeAxes = Axes.Both,
                    ColourDark = osu.Green.Darken(0.4f),
                    ColourLight = osu.Green.Lighten(0.4f)
                },
                debugItems = new FillFlowContainer<Container>
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,

                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y
                }
            };

        }

        public void UpdateItems()
        {
            foreach (Container container in DebugItems)
                debugItems.Add(container);

            DebugItems = new List<Container>();
        }
    }

    public enum DebugConfiguration
    {
        Gameplay,
        Networking,
    }
}
