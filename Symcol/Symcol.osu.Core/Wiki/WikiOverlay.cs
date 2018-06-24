using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game;
using osu.Game.Graphics;
using osu.Game.Graphics.Containers;
using osu.Game.Overlays;
using OpenTK.Graphics;
using Symcol.osu.Core.Wiki.Header;
using Symcol.osu.Core.Wiki.Index;
using Symcol.osu.Core.Wiki.Sections;
using osu.Game.Graphics.UserInterface;

namespace Symcol.osu.Core.Wiki
{
    //The wiki is my GREATEST POWER!
    public class WikiOverlay : WaveOverlayContainer
    {
        public const float CONTENT_X_MARGIN = 80;

        private WikiHeader header;
        private WikiSection[] sections;

        private WikiSection lastSection;
        private SectionsContainer<WikiSection> sectionsContainer;
        private WikiTabControl tabs;

        private OsuGame game;

        public WikiOverlay()
        {
            Waves.FirstWaveColour = OsuColour.Gray(0.4f);
            Waves.SecondWaveColour = OsuColour.Gray(0.3f);
            Waves.ThirdWaveColour = OsuColour.Gray(0.2f);
            Waves.FourthWaveColour = OsuColour.Gray(0.1f);

            RelativeSizeAxes = Axes.Both;
            RelativePositionAxes = Axes.Both;
            Width = 0.85f;

            Anchor = Anchor.TopCentre;
            Origin = Anchor.TopCentre;

            Masking = true;
            AlwaysPresent = true;

            EdgeEffect = new EdgeEffectParameters
            {
                Colour = Color4.Black.Opacity(0),
                Type = EdgeEffectType.Shadow,
                Radius = 10
            };

            Add(new Box
            {
                RelativeSizeAxes = Axes.Both,
                Colour = OsuColour.Gray(0.2f)
            });

            header = new WikiHeader
            {

            };

            tabs = new WikiTabControl
            {
                RelativeSizeAxes = Axes.X,
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                Height = 30
            };

            AddRange(new Drawable[]
            {
                sectionsContainer = new SectionsContainer<WikiSection>
                {
                    RelativeSizeAxes = Axes.Both,
                    ExpandableHeader = header,
                    FixedHeader = tabs,
                    HeaderBackground = new Box
                    {
                        Colour = OsuColour.Gray(34),
                        RelativeSizeAxes = Axes.Both
                    }
                }
            });

            sectionsContainer.SelectedSection.ValueChanged += s =>
            {
                if (lastSection != s)
                {
                    lastSection = s;
                    //index.Current.Value = lastSection;
                }
            };

            /*
            index.Current.ValueChanged += s =>
            {
                if (lastSection == null)
                {
                    lastSection = sectionsContainer.Children.FirstOrDefault();
                    if (lastSection != null)
                        index.Current.Value = lastSection;
                    return;
                }
                if (lastSection != s)
                {
                    lastSection = s;
                    sectionsContainer.ScrollTo(lastSection);
                }
            };
            */
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame game)
        {
            this.game = game;
        }

        protected override void PopIn()
        {
            base.PopIn();
            FadeEdgeEffectTo(0.5f, WaveContainer.APPEAR_DURATION, Easing.In);
        }

        protected override void PopOut()
        {
            base.PopOut();
            FadeEdgeEffectTo(0, WaveContainer.DISAPPEAR_DURATION, Easing.Out);
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            Padding = new MarginPadding { Top = game.ToolbarOffset };
        }

        private class WikiTabControl : PageTabControl<WikiSection>
        {
            public WikiTabControl()
            {
                TabContainer.RelativeSizeAxes &= ~Axes.X;
                TabContainer.AutoSizeAxes |= Axes.X;
                TabContainer.Anchor |= Anchor.x1;
                TabContainer.Origin |= Anchor.x1;
            }

            protected override TabItem<WikiSection> CreateTabItem(WikiSection value) => new WikiTabItem(value);

            protected override Dropdown<WikiSection> CreateDropdown() => null;

            private class WikiTabItem : PageTabItem
            {
                public WikiTabItem(WikiSection value) : base(value)
                {
                    Text.Text = value.Title;
                }
            }
        }
    }
}
