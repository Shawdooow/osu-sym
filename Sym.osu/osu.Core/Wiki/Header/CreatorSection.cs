#region usings

using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Platform;
using osu.Game.Graphics.Containers;
using osu.Game.Users;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Core.Wiki.Header
{
    public class CreatorSection : Container
    {
        public Creator Creator
        {
            get => creator;
            set
            {
                creator = value;

                if (creator == null)
                {
                    this.FadeOut(200);
                    return;
                }

                this.FadeIn(200);

                //country.Country = value.Country;

                cover.Child = new UserCoverBackground(value)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fill,
                };

                avatar.User = value;
                avatarContainer.Action = () => openLink("https://osu.ppy.sh/users/" + value.Id);
                quote.Text = value.Quote;
            }
        }

        private Creator creator;

        //private readonly DrawableFlag country;
        private readonly Container cover;

        private readonly ClickableContainer avatarContainer;
        private readonly UpdateableAvatar avatar;

        private readonly OsuTextFlowContainer quote;

        public CreatorSection()
        {
            AlwaysPresent = true;
            Position = new Vector2(-40, 0);
            Size = new Vector2(0.2f, 0.8f);

            Masking = true;

            RelativeSizeAxes = Axes.Both;
            Anchor = Anchor.BottomRight;
            Origin = Anchor.BottomRight;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.4f,
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,

                    Size = new Vector2(0.9f, 0.2f),
                    Position = new Vector2(0, 10),

                    Masking = true,
                    CornerRadius = 8,

                    Children = new Drawable[]
                    {
                        cover = new Container
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = Color4.Black,
                            Alpha = 0.5f,
                        },
                        avatarContainer = new ClickableContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,

                            Size = new Vector2(36),
                            Masking = true,
                            CornerRadius = 4,

                            Child = avatar = new UpdateableAvatar
                            {
                                RelativeSizeAxes = Axes.Both
                            }
                        },
                    }
                },
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    Size = new Vector2(0.9f, 0.76f),

                    Masking = true,

                    Child = quote = new OsuTextFlowContainer(t => { t.TextSize = 16; })
                    {
                        RelativeSizeAxes = Axes.Both,
                        Size = new Vector2(0.98f),
                    }
                }
            };
        }

        private GameHost host;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            this.host = host;
        }

        private void openLink(string link) => host.OpenUrlExternally(link);
    }
}
