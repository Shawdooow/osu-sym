using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets;
using osu.Mods.Online.Multi.Packets.Lobby;
using osu.Mods.Online.Multi.Screens.Pieces;
using osuTK;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Screens
{
    public class Lobby : MultiScreen
    {
        public override string Title => "Lobby";

        private RulesetStore rulesets;

        private readonly FillFlowContainer rooms;

        public Lobby(OsuNetworkingHandler osuNetworkingHandler)
            : base(osuNetworkingHandler)
        {
            Children = new Drawable[]
            {
                new ScrollContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,

                    RelativeSizeAxes = Axes.Both,
                    Height = 0.9f,
                    Width = 0.9f,

                    Child = rooms = new FillFlowContainer
                    {
                        RelativeSizeAxes = Axes.X,
                    }
                },
                new SettingsButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Position = new Vector2(12, -12),
                    RelativeSizeAxes = Axes.X,
                    Width = 0.4f,
                    Text = "Create Game",
                    Action = () =>
                    {
                        SendPacket(new CreateMatchPacket
                        {
                            MatchInfo = new MatchInfo
                            {
                                Host = new Host
                                {
                                    Username = @"Shawdooow",
                                    UserID = 7726082,
                                    UserCountry = "US",
                                },
                                Map = new Map
                                {
                                    BeatmapTitle = "Lost Emotion",
                                    BeatmapArtist = "Masayoshi Minoshima feat.nomico",
                                    BeatmapMapper = "Shawdooow",
                                    BeatmapDifficulty = "Last Dance Heaven",
                                    OnlineBeatmapSetID = 734008,
                                    OnlineBeatmapID = 1548917,
                                    BeatmapStars = 4.85d,
                                    RulesetShortname = "osu",
                                },
                            }
                        });
                    }
                },
                new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Position = new Vector2(-12),
                    RelativeSizeAxes = Axes.X,
                    Width = 0.4f,
                    Text = "Refresh",
                    Action = () => { SendPacket(new GetMatchListPacket()); }
                },
            };
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            SendPacket(new GetMatchListPacket());
        }

        protected override void OnPacketRecieve(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MatchListPacket matchListPacket:
                    rooms.Children = new Container();
                    foreach (MatchInfo m in matchListPacket.MatchInfoList)
                        rooms.Add(new MatchTile(OsuNetworkingHandler, m));
                    break;
                case MatchCreatedPacket matchCreated:
                    rooms.Add(new MatchTile(OsuNetworkingHandler, matchCreated.MatchInfo));
                    SendPacket(new JoinMatchPacket
                    {
                        Match = matchCreated.MatchInfo,
                        User = OsuNetworkingHandler.OsuUserInfo
                    });
                    break;
                case JoinedMatchPacket joinedMatch:
                    Push(new Match(OsuNetworkingHandler, joinedMatch));
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets)
        {
            this.rulesets = rulesets;
        }
    }
}
