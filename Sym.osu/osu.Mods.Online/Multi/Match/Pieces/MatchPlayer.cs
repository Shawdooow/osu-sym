using System.ComponentModel;
using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.Events;
using osu.Framework.Platform;
using osu.Game.Graphics.UserInterface;
using osu.Game.Users;
using osu.Mods.Online.Base;
using osuTK;
using osuTK.Graphics;
using Sym.Base.Extentions;
using Sym.Base.Graphics.Containers;

namespace osu.Mods.Online.Multi.Match.Pieces
{
    public class MatchPlayer : SymcolClickableContainer, IHasContextMenu
    {
        public readonly OsuUserInfo OsuUserInfo;

        private readonly Box dim;

        private readonly DrawableFlag countryFlag;
        private readonly UserCoverBackground profileBackground;
        private readonly UpdateableAvatar profilePicture;
        private readonly StatuesIcon statues;

        public PlayerStatues PlayerStatues
        {
            get => playerStatues;
            set
            {
                if (playerStatues == value) return;

                playerStatues = value;

                switch (value)
                {
                    case PlayerStatues.JoiningMatch:
                        statues.SetColor(Color4.SkyBlue);
                        break;
                    case PlayerStatues.MissingRuleset:
                        statues.SetColor(Color4.Black);
                        break;
                    case PlayerStatues.MissingMap:
                        statues.SetColor(Color4.Red);
                        break;
                    case PlayerStatues.SearchingForMap:
                        statues.SetColor(Color4.Orange);
                        break;
                    case PlayerStatues.FoundMap:
                        statues.SetColor(Color4.Yellow);
                        break;
                    case PlayerStatues.Ready:
                        statues.SetColor(Color4.Green);
                        break;
                    case PlayerStatues.Loading:
                        statues.SetColor(Color4.Cyan);
                        break;
                    case PlayerStatues.Playing:
                        statues.SetColor(Color4.Blue);
                        break;
                }

                statues.TooltipText = value.GetDescription();
            }
        }

        private PlayerStatues playerStatues;

        public MatchPlayer(OsuUserInfo userInfo)
        {
            OsuUserInfo = userInfo;

            Alpha = 0;
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = 40f;
            CornerRadius = 10;

            Country country = new Country
            {
                FullName = OsuUserInfo.Country,
                FlagName = OsuUserInfo.CountryFlagName,
            };

            User user = new User
            {
                Username = OsuUserInfo.Username,
                Id = OsuUserInfo.ID,
                Country = country,
                AvatarUrl = OsuUserInfo.Pic,
                CoverUrl = OsuUserInfo.Background,
            };

            Children = new Drawable[]
            {
                profileBackground = new UserCoverBackground(user)
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    FillMode = FillMode.Fill,
                    OnLoadComplete = d => d.FadeInFromZero(200),
                },
                dim = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f
                },
                profilePicture = new UpdateableAvatar
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Size = new Vector2(Height * 0.8f),
                    Position = new Vector2(6, 0),
                    User = user,
                    Masking = true,
                    CornerRadius = 6,
                },
                countryFlag = new DrawableFlag(country)
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Size = new Vector2(Height * 0.9f, Height * 0.9f * 0.66f),
                    Position = new Vector2(-10, 0)
                },
                statues = new StatuesIcon(),
                new SpriteText
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Position = new Vector2(Height * 1.1f, 0),
                    TextSize = Height * 0.9f,
                    Text = user.Username
                }
            };

            Action = () =>
            {
                openLink("https://osu.ppy.sh/users/" + user.Id);
            };
        }

        private GameHost host;

        [BackgroundDependencyLoader]
        private void load(GameHost host)
        {
            this.host = host;
        }

        private void openLink(string link) => host.OpenUrlExternally(link);

        protected override bool OnHover(HoverEvent e)
        {
            dim.FadeTo(0.6f, 200);
            return base.OnHover(e);
        }

        protected override void OnHoverLost(HoverLostEvent e)
        {
            dim.FadeTo(0.8f, 200);
            base.OnHoverLost(e);
        }

        public MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem("View Profile", MenuItemType.Standard, () => { }),
            new OsuMenuItem("Promote to Host", MenuItemType.Highlighted, () => { }),
            new OsuMenuItem("Kick", MenuItemType.Destructive, () => { }),
            new OsuMenuItem("Ban", MenuItemType.Destructive, () => { }),
        };

        private class StatuesIcon : SymcolContainer, IHasTooltip
        {
            public string TooltipText { get; set; }

            public void SetColor(Color4 color)
            {
                this.FadeColour(color, 150);
                TweenEdgeEffectTo(new EdgeEffectParameters
                {
                    Radius = 10,
                    Colour = color.Opacity(0.25f),
                    Type = EdgeEffectType.Shadow,
                }, 150);
            }

            public StatuesIcon()
            {
                Size = new Vector2(40f, 20f);
                CornerRadius = 10f;
                Masking = true;

                Anchor = Anchor.CentreRight;
                Origin = Anchor.CentreRight;
                Position = new Vector2(-100, 0);

                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.White,
                };

                SetColor(Color4.SkyBlue);
            }
        }
    }

    public enum PlayerStatues
    {
        [Description("Joining...")]
        JoiningMatch,
        [Description("Missing Ruleset")]
        MissingRuleset,
        [Description("Missing Map")]
        MissingMap,
        [Description("Searching For Map...")]
        SearchingForMap,
        [Description("Not Ready")]
        FoundMap,
        [Description("Ready")]
        Ready,
        [Description("Loading...")]
        Loading,
        [Description("Playing")]
        Playing,
    }
}
