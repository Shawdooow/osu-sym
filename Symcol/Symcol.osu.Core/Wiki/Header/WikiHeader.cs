using System;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.Graphics.Containers;
using Symcol.osu.Core.Wiki.Index;

namespace Symcol.osu.Core.Wiki.Header
{
    public class WikiHeader : Container
    {
        private const float icon_size = 200;
        private const float header_margin = 50;
        private const float rulesetname_height = 60;

        public event Action<WikiSet> OnWikiSetChange;

        private readonly Sprite background;
        private readonly Sprite icon;

        private readonly BreadcrumbControl<BreadCrumbState> breadcrumbs;
        private readonly WikiIndex index;

        public WikiHeader()
        {
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = header_margin + icon_size + rulesetname_height;

            Children = new Drawable[]
            {
                background = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    FillMode  = FillMode.Fill,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    //Texture = HeaderBackground
                },
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.5f,
                },
                icon = new Sprite
                {
                    Size = new Vector2(icon_size),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    //Texture = RulesetIcon
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

        private class HeaderBackButton : SymcolClickableContainer
        {
            public HeaderBackButton()
            {

            }
        }
    }

    public enum BreadCrumbState
    {
        Home,
        Wiki,
        SectionExplanation
    }
}
