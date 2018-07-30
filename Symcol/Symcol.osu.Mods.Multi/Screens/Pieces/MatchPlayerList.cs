using System.Collections.Generic;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Networking.Packets.Match;

namespace Symcol.osu.Mods.Multi.Screens.Pieces
{
    public class MatchPlayerList : Container
    {
        private readonly OsuNetworkingClientHandler osuNetworkingClientHandler;

        public readonly List<MatchPlayer> MatchPlayers = new List<MatchPlayer>();

        public readonly FillFlowContainer MatchPlayersContianer;

        public MatchPlayerList(OsuNetworkingClientHandler osuNetworkingClientHandler)
        {
            this.osuNetworkingClientHandler = osuNetworkingClientHandler;

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
                        break;
                    case PlayerDisconnectedPacket playerDisconnected:
                        break;
                    case PlayerListPacket playerListPacket:
                        break;
                }
            };

            /*
            osuNetworkingClientHandler.OnReceivePlayerList += (players) =>
            {
                restart:
                foreach (MatchPlayer matchPlayer in MatchPlayers)
                    foreach (OsuClientInfo clientInfo in players)
                        if (clientInfo is OsuClientInfo rulesetClientInfo)
                            if (rulesetClientInfo.IP + rulesetClientInfo.Port != matchPlayer.ClientInfo.IP + matchPlayer.ClientInfo.Port)
                            {
                                Add(rulesetClientInfo);
                                players.Remove(clientInfo);
                                goto restart;
                            }
            };
            osuNetworkingClientHandler.RequestPlayerList();

            osuNetworkingClientHandler.OnClientJoin += (clientInfo) =>
            {
                foreach (MatchPlayer matchPlayer in MatchPlayers)
                    if (clientInfo is OsuClientInfo rulesetClientInfo)
                        if (rulesetClientInfo.IP + rulesetClientInfo.Port != matchPlayer.ClientInfo.IP + matchPlayer.ClientInfo.Port)
                        {
                            Add(rulesetClientInfo);
                            break;
                        }
            };

            osuNetworkingClientHandler.OnClientDisconnect += (clientInfo) =>
            {
                foreach (MatchPlayer matchPlayer in MatchPlayers)
                    if (clientInfo is OsuClientInfo rulesetClientInfo)
                        if (rulesetClientInfo.IP + rulesetClientInfo.Port == matchPlayer.ClientInfo.IP + matchPlayer.ClientInfo.Port)
                        {
                            Remove(matchPlayer);
                            break;
                        }
            };
            */
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
