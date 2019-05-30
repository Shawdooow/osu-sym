using System.Collections.Generic;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Networking.Packets.Match;

namespace Symcol.osu.Mods.Multi.Screens.Pieces
{
    public class MatchPlayerList : Container
    {
        public readonly List<MatchPlayer> MatchPlayers = new List<MatchPlayer>();

        public readonly FillFlowContainer MatchPlayersContianer;

        public MatchPlayerList(OsuNetworkingClientHandler osuNetworkingClientHandler)
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
                MatchPlayersContianer = new FillFlowContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.98f,
                    Height = 0.96f
                }
            };

            osuNetworkingClientHandler.OnPacketReceive += packet =>
            {
                switch (packet)
                {
                    case PlayerJoinedPacket playerJoined:
                        bool add = true;
                        foreach (MatchPlayer matchPlayer in MatchPlayers)
                            if (playerJoined.Player.UserID == matchPlayer.OsuClientInfo.UserID)
                                add = false;

                        if (add)
                            Add(playerJoined.Player);
                        else
                            Logger.Log($"{playerJoined.Player.Username} - {playerJoined.Player.UserID} is joining this match twice!?", LoggingTarget.Network, LogLevel.Error);
                        break;
                    case PlayerDisconnectedPacket playerDisconnected:
                        foreach (MatchPlayer matchPlayer in MatchPlayers)
                            if (playerDisconnected.Player.UserID == matchPlayer.OsuClientInfo.UserID)
                            {
                                Remove(matchPlayer);
                                break;
                            }
                        break;
                    case PlayerListPacket playerListPacket:
                        //TODO: handle this packet if we even need it
                        break;
                }
            };
        }

        public void Add(OsuClientInfo clientInfo)
        {
            MatchPlayer matchPlayer = new MatchPlayer(clientInfo);

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
