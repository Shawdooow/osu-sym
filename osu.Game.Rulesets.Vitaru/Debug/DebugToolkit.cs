using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Rulesets.Vitaru.Settings;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Debug
{
    public class DebugToolkit : Container
    {
        public static List<Container> GeneralDebugItems = new List<Container>();
        public static List<Container> MachineLearningDebugItems = new List<Container>();

        private readonly FillFlowContainer<Container> debugItems;

        private readonly DebugConfiguration configuration = VitaruSettings.VitaruConfigManager.GetBindable<DebugConfiguration>(VitaruSetting.DebugConfiguration);

        public DebugToolkit()
        {
            OsuColour osu = new OsuColour();

            Anchor = Anchor.CentreRight;
            Origin = Anchor.CentreRight;

            Position = new Vector2(-20, 0);

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
                    AutoSizeAxes = Axes.Y,

                    Margin = new MarginPadding { Vertical = 6 },

                    Width = 0.96f,
                    Spacing = new Vector2(1)
                }
            };

        }

        public void UpdateItems()
        {
            switch (configuration)
            {
                case DebugConfiguration.General:
                    foreach (Container container in GeneralDebugItems)
                        debugItems.Add(container);
                    break;
                case DebugConfiguration.NeuralNetworking:
                    foreach (Container container in MachineLearningDebugItems)
                        debugItems.Add(container);
                    break;
            }


            GeneralDebugItems = new List<Container>();
            MachineLearningDebugItems = new List<Container>();
        }
    }

    public enum DebugConfiguration
    {
        General,
        [System.ComponentModel.Description("Neural Networking")]
        NeuralNetworking,
        Networking,
    }
}
