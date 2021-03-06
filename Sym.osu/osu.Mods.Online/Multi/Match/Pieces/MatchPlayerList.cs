﻿#region usings

using System.Collections.Generic;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Game.Graphics.Containers;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Match.Packets;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Match.Pieces
{
    public class MatchPlayerList : MultiplayerContainer
    {
        public readonly List<MatchPlayer> MatchPlayers = new List<MatchPlayer>();

        public readonly FillFlowContainer MatchPlayersContianer;

        public MatchPlayerList(OsuNetworkingHandler osuNetworkingHandler) : base (osuNetworkingHandler)
        {
            Masking = true;
            CornerRadius = 16;
            Anchor = Anchor.TopLeft;
            Origin = Anchor.TopLeft;
            RelativeSizeAxes = Axes.Both;
            Width = 0.49f;
            Height = 0.45f;
            Position = new Vector2(10);

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black.Opacity(0.8f)
                },
                new OsuScrollContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.98f,
                    Height = 0.96f,

                    Child = MatchPlayersContianer = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                        AutoSizeAxes = Axes.Y
                    }
                },
            };
        }

        protected override void OnPacketRecieve(PacketInfo<Host> info)
        {
            switch (info.Packet)
            {
                case PlayerJoinedPacket playerJoined:
                    bool add = true;
                    foreach (MatchPlayer matchPlayer in MatchPlayers)
                        if (playerJoined.User.ID == matchPlayer.OsuUserInfo.ID)
                            add = false;

                    if (add)
                        Add(playerJoined.User);
                    else
                        Logger.Log($"{playerJoined.User.Username} - {playerJoined.User.ID} is joining this match twice!?", LoggingTarget.Network, LogLevel.Error);
                    break;
                case StatuesChangePacket statuesChange:
                    foreach (MatchPlayer matchPlayer in MatchPlayers)
                        if (statuesChange.User.ID == matchPlayer.OsuUserInfo.ID)
                        {
                            matchPlayer.PlayerStatues = statuesChange.User.Statues;
                            break;
                        }
                    break;
                case PlayerDisconnectedPacket playerDisconnected:
                    foreach (MatchPlayer matchPlayer in MatchPlayers)
                        if (playerDisconnected.User.ID == matchPlayer.OsuUserInfo.ID)
                        {
                            Remove(matchPlayer);
                            break;
                        }
                    break;
                case PlayerListPacket playerListPacket:
                    //TODO: handle this packet if we even need it
                    break;
            }
        }

        public void Add(OsuUser user)
        {
            MatchPlayer matchPlayer = new MatchPlayer(user);

            Add(matchPlayer);
        }

        public void Add(MatchPlayer matchPlayer)
        {
            MatchPlayers.Add(matchPlayer);
            MatchPlayersContianer.Add(matchPlayer);
            matchPlayer.FadeInFromZero(200);
        }

        public void Remove(MatchPlayer matchPlayer)
        {
            MatchPlayers.Remove(matchPlayer);
            matchPlayer.FadeOutFromOne(200)
                .Expire();
        }
    }
}
