﻿using System.Diagnostics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Input.States;
using osu.Game.Graphics.UserInterface;
using osu.Game.Users;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.Graphics.Containers;
using Symcol.osu.Mods.Multi.Networking;

namespace Symcol.osu.Mods.Multi.Screens.Pieces
{
    public class MatchPlayer : SymcolClickableContainer, IHasContextMenu
    {
        public readonly OsuClientInfo OsuClientInfo;

        private readonly Box dim;

        private readonly DrawableFlag countryFlag;
        private readonly UserCoverBackground profileBackground;
        private readonly UpdateableAvatar profilePicture;

        public MatchPlayer(OsuClientInfo clientInfo)
        {
            OsuClientInfo = clientInfo;

            Alpha = 0;
            Masking = true;
            RelativeSizeAxes = Axes.X;
            Height = 40f;
            CornerRadius = 10;

            Country country = new Country
            {
                FullName = OsuClientInfo.UserCountry,
                FlagName = OsuClientInfo.CountryFlagName,
            };

            User user = new User
            {
                Username = OsuClientInfo.Username,
                Id = OsuClientInfo.UserID,
                Country = country,
                AvatarUrl = OsuClientInfo.UserPic,
                CoverUrl = OsuClientInfo.UserBackground,
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
                    Size = new Vector2(Height * 0.9f, (Height * 0.9f) * 0.66f),
                    Position = new Vector2(-10, 0)
                },
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
                Process.Start("https://osu.ppy.sh/users/" + user.Id);
            };
        }

        protected override bool OnHover(InputState state)
        {
            dim.FadeTo(0.6f, 200);

            return base.OnHover(state);
        }

        protected override void OnHoverLost(InputState state)
        {
            base.OnHoverLost(state);

            dim.FadeTo(0.8f, 200);
        }

        public MenuItem[] ContextMenuItems => new MenuItem[]
        {
            new OsuMenuItem("View Profile", MenuItemType.Standard, () => { }),
            new OsuMenuItem("Promote to Host", MenuItemType.Highlighted, () => { }),
            new OsuMenuItem("Kick", MenuItemType.Destructive, () => { }),
            new OsuMenuItem("Ban", MenuItemType.Destructive, () => { }),
        };
    }
}
