using osu.Framework.Configuration;
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
        public readonly Bindable<WikiSet> CurrentWikiSet = new Bindable<WikiSet>();

        public readonly HomeWikiSet Home = new HomeWikiSet();

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
                index = new WikiIndex()
            };

            breadcrumbs.Current.ValueChanged += value =>
            {
                switch (value)
                {
                    case BreadCrumbState.Home:
                        CurrentWikiSet.Value = Home;
                        break;
                }
            };

            CurrentWikiSet.BindTo(index.CurrentWikiSet);

            CurrentWikiSet.ValueChanged += value =>
            {
                if (value.HeaderBackground != null)
                    background.Texture = value.HeaderBackground;

                if (value.Icon != null)
                    icon.Texture = value.Icon;

                if (value != Home)
                    breadcrumbs.Current.Value = BreadCrumbState.Wiki;
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
