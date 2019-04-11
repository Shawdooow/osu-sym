#region usings

using System.Collections.Generic;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osuTK;

#endregion

namespace osu.Core.Wiki.Sections.OptionExplanations
{
    public class WikiOptionExplanation<T> : Container
    {
        public OsuTextFlowContainer Description;

        public WikiOptionExplanation(Bindable<T> bindable, IEnumerable<T> items)
        {
            OsuColour osu = new OsuColour();
            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;
            AutoSizeAxes = Axes.Y;
            RelativeSizeAxes = Axes.X;
            Masking = true;

            SettingsDropdown<T> dropdown;

            Children = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = osu.Yellow,
                    Masking = true,
                    RelativeSizeAxes = Axes.Y,
                    Size = new Vector2(10, 0.98f),
                    CornerRadius = 5,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                },
                new Container
                {
                    Position = new Vector2(-10, 0),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    AutoSizeAxes = Axes.Y,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.45f,

                    Child = dropdown = new SettingsDropdown<T>
                    {
                        Items = items
                    }
                },
                new Container
                {
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.45f,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 100,
                    AutoSizeEasing = Easing.OutQuint,

                    Child = Description = new OsuTextFlowContainer(t => { t.TextSize = 20; })
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y
                    }
                }
            };

            dropdown.Bindable = bindable;
        }
    }
}
