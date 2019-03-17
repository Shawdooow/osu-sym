using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Lobby.Packets;
using osu.Mods.Online.Multi.Lobby.Pieces;
using osu.Mods.Online.Multi.Match;
using osuTK;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Lobby
{
    public class LobbyScreen : MultiScreen
    {
        private readonly Bindable<RulesetInfo> ruleset = new Bindable<RulesetInfo>();

        private readonly Bindable<WorkingBeatmap> beatmap = new Bindable<WorkingBeatmap>();

        private readonly FillFlowContainer rooms;

        public LobbyScreen(OsuNetworkingHandler osuNetworkingHandler)
            : base(osuNetworkingHandler)
        {
            Name = "Lobby";

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
                    Action = createMatch,
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
                    Push(new MatchScreen(OsuNetworkingHandler, joinedMatch));
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(Bindable<RulesetInfo> ruleset, Bindable<WorkingBeatmap> beatmap)
        {
            this.ruleset.BindTo(ruleset);
            this.beatmap.BindTo(beatmap);
        }

        private void createMatch()
        {
            Map map;

            try
            {
                map = new Map
                {
                    BeatmapTitle = beatmap.Value.Metadata.Title,
                    BeatmapArtist = beatmap.Value.Metadata.Artist,
                    BeatmapMapper = beatmap.Value.Metadata.Author.Username,
                    BeatmapDifficulty = beatmap.Value.BeatmapInfo.Version,
                    OnlineBeatmapSetID = (int)beatmap.Value.BeatmapSetInfo.OnlineBeatmapSetID,
                    OnlineBeatmapID = (int)beatmap.Value.BeatmapInfo.OnlineBeatmapID,
                    BeatmapStars = beatmap.Value.BeatmapInfo.StarDifficulty,
                    RulesetShortname = ruleset.Value.ShortName,
                };
            }
            catch
            {
                map = new Map
                {
                    BeatmapTitle = beatmap.Value.Metadata.Title,
                    BeatmapArtist = beatmap.Value.Metadata.Artist,
                    BeatmapMapper = beatmap.Value.Metadata.Author.Username,
                    BeatmapDifficulty = beatmap.Value.BeatmapInfo.Version,
                    BeatmapStars = beatmap.Value.BeatmapInfo.StarDifficulty,
                    RulesetShortname = ruleset.Value.ShortName,
                };
            }

            SendPacket(new CreateMatchPacket
            {
                MatchInfo = new MatchInfo
                {
                    Host = new Host
                    {
                        Username = OsuNetworkingHandler.OsuUserInfo.Username,
                        UserID = OsuNetworkingHandler.OsuUserInfo.ID,
                        UserCountry = OsuNetworkingHandler.OsuUserInfo.Country,
                    },
                    Map = map,
                }
            });
        }
    }
}
