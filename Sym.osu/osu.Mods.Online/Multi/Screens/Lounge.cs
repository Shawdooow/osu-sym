using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.Events;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.Multiplayer;
using osu.Game.Online.Multiplayer.GameTypes;
using osu.Game.Online.Multiplayer.RoomStatuses;
using osu.Game.Overlays.SearchableList;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using osu.Game.Screens.Multi.Lounge.Components;
using osu.Game.Users;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Lobby;
using osu.Mods.Online.Multi.Screens.Mods;
using osuTK;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Screens
{
    public class Lounge : MultiScreen
    {
        private RulesetStore rulesets;

        protected readonly FilterControl Filter;

        private readonly Container content;
        private readonly OnlineRoomsContainer rooms;
        private readonly ProcessingOverlay processingOverlay;

        public override string Title => "Lounge";

        public Lounge(OsuNetworkingHandler osuNetworkingHandler)
            : base(osuNetworkingHandler)
        {
            OnlineRoomInspector inspector;

            Children = new Drawable[]
            {
                Filter = new FilterControl { Depth = -1 },
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Width = 0.55f,
                            Children = new Drawable[]
                            {
                                new ScrollContainer
                                {
                                    RelativeSizeAxes = Axes.Both,
                                    ScrollbarOverlapsContent = false,
                                    Padding = new MarginPadding(10),
                                    Child = new SearchContainer
                                    {
                                        RelativeSizeAxes = Axes.X,
                                        AutoSizeAxes = Axes.Y,
                                        Child = rooms = new OnlineRoomsContainer(osuNetworkingHandler) { JoinRequested = joinRequested }
                                    },
                                },
                                processingOverlay = new ProcessingOverlay { Alpha = 0 },
                                new SettingsButton
                                {
                                    Anchor = Anchor.BottomLeft,
                                    Origin = Anchor.BottomLeft,
                                    Position = new Vector2(12, -12),
                                    RelativeSizeAxes = Axes.X,
                                    Width = 0.4f,
                                    Text = "Create Game",
                                    Action = () => { SendPacket(new CreateMatchPacket { MatchInfo = new MatchListPacket.MatchInfo() }); }
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
                            },
                        },
                        inspector = new OnlineRoomInspector
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.Both,
                            Width = 0.45f,
                        },
                    },
                },
            };

            inspector.Room.BindTo(rooms.SelectedRoom);

            Filter.Search.Current.ValueChanged += s => filterRooms();
            Filter.Tabs.Current.ValueChanged += t => filterRooms();
            Filter.Search.Exit += this.Exit;
        }

        protected override void UpdateAfterChildren()
        {
            base.UpdateAfterChildren();

            content.Padding = new MarginPadding
            {
                Top = Filter.DrawHeight,
                Left = SearchableListOverlay.WIDTH_PADDING - DrawableRoom.SELECTION_BORDER_WIDTH,
                Right = SearchableListOverlay.WIDTH_PADDING,
            };
        }

        protected override void OnFocus(FocusEvent e)
        {
            GetContainingInputManager().ChangeFocus(Filter.Search);
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);
            Filter.Search.HoldFocus = true;
            SendPacket(new GetMatchListPacket());
        }

        public override bool OnExiting(IScreen next)
        {
            Filter.Search.HoldFocus = false;
            // no base call; don't animate
            return false;
        }

        public override void OnSuspending(IScreen next)
        {
            base.OnSuspending(next);
            Filter.Search.HoldFocus = false;
        }

        private void filterRooms()
        {
            //rooms.Filter(Filter.CreateCriteria());
        }

        private void joinRequested(Room room)
        {
            processingOverlay.Show();
        }

        protected override void OnPacketRecieve(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MatchListPacket matchListPacket:
                    rooms.RoomFlow.Children = new DrawableOnlineRoom[]{};
                    foreach (MatchListPacket.MatchInfo m in matchListPacket.MatchInfoList)
                        rooms.RoomFlow.Add(getRoom(m));
                    break;
                case MatchCreatedPacket matchCreated:
                    rooms.RoomFlow.Add(getRoom(matchCreated.MatchInfo));
                    SendPacket(new JoinMatchPacket
                    {
                        Match = matchCreated.MatchInfo,
                        User = OsuNetworkingHandler.OsuUserInfo
                    });
                    break;
                case JoinedMatchPacket joinedMatch:
                    processingOverlay.Hide();
                    Push(new Match(OsuNetworkingHandler, joinedMatch));
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets)
        {
            this.rulesets = rulesets;
        }

        private DrawableOnlineRoom getRoom(MatchListPacket.MatchInfo match)
        {
            return new DrawableOnlineRoom(new OnlineRoom
            {
                Name = { Value = match.Name },
                Host = { Value = new User { Username = match.Username, Id = match.UserID, Country = new Country { FlagName = match.UserCountry } } },
                Status = { Value = new RoomStatusOpen() },
                Type = { Value = new GameTypeVersus() },
                Beatmap =
                {
                    Value = new BeatmapInfo
                    {
                        StarDifficulty = match.BeatmapStars,
                        Ruleset = rulesets.GetRuleset(match.RulesetID ?? 0),
                        Metadata = new BeatmapMetadata
                        {
                            Title = match.BeatmapTitle,
                            Artist = match.BeatmapArtist,
                        },
                        BeatmapSet = new BeatmapSetInfo
                        {
                            OnlineInfo = new BeatmapSetOnlineInfo
                            {
                                Covers = new BeatmapSetOnlineCovers
                                {
                                    Cover = @"https://assets.ppy.sh/beatmaps/734008/covers/cover.jpg?1523042189",
                                }
                            }
                        }
                    }
                }
            });
        }
    }
}
