using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using OpenTK.Graphics;
using Symcol.osu.Core.Wiki.Index;
using Container = osu.Framework.Graphics.Containers.Container;

namespace Symcol.osu.Core.Wiki.Header
{
    public class WikiHeader : Container
    {
        public event Action<WikiSet> OnWikiSetChange;

        private readonly Sprite background;
        private readonly Sprite icon;

        private readonly BreadcrumbControl<BreadCrumbState> breadcrumbs;
        private readonly WikiIndex index;

        public WikiHeader()
        {
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = 310;

            Children = new Drawable[]
            {
                background = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode  = FillMode.Fill,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                },
                icon = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode  = FillMode.Fit,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                },
                breadcrumbs = new BreadcrumbControl<BreadCrumbState>
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.5f
                },
                index = new WikiIndex
                {

                }
            };
        }
    }

    public enum BreadCrumbState
    {
        Home,
        Wiki,
        Section
    }
}
