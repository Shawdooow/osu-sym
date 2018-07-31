using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Input.States;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.Multiplayer;
using osu.Game.Overlays.SearchableList;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using osu.Game.Screens.Multi.Components;
using osu.Game.Screens.Multi.Screens.Lounge;
using osu.Game.Users;
using OpenTK;
using Symcol.osu.Core;
using Symcol.osu.Core.Config;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Networking.Packets.Lobby;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class Lobby : MultiScreen
    {
        private readonly Bindable<string> localip = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.LocalIP);
        private readonly Bindable<int> localport = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.LocalPort);

        private readonly Container content;
        private readonly SearchContainer search;

        protected readonly FilterControl Filter;
        protected readonly FillFlowContainer<DrawableRoom> RoomsContainer;
        protected readonly RoomInspector Inspector;

        public override string Title => "Lobby";

        private IEnumerable<Room> rooms;
        public IEnumerable<Room> Rooms
        {
            get => rooms;
            set
            {
                rooms = value;

                var enumerable = rooms.ToList();

                RoomsContainer.Children = enumerable.Select(r => new DrawableRoom(r)
                {
                    Action = didSelect,
                }).ToList();

                if (!enumerable.Contains(Inspector.Room))
                    Inspector.Room = null;

                filterRooms();
            }
        }

        private RulesetStore rulesets;

        private List<Room> roomList = new List<Room>();

        public Lobby(OsuNetworkingClientHandler osuNetworkingClientHandler)
            : base(osuNetworkingClientHandler)
        {
            OsuNetworkingClientHandler.SendPacket(new GetMatchListPacket { Address = OsuNetworkingClientHandler.Address });

            OsuNetworkingClientHandler.OnPacketReceive += packet =>
            {
                switch (packet)
                {
                    case MatchListPacket matchListPacket:
                        roomList = new List<Room>();
                        foreach (MatchListPacket.MatchInfo info in matchListPacket.MatchInfoList)
                            roomList.Add(new Room
                            {
                                Name = { Value = info.Name },
                                Host = { Value = new User { Username = info.Username, Id = info.UserID, Country = new Country { FlagName = info.UserCountry } } },
                                Status = { Value = new RoomStatusOpen() },
                                Type = { Value = new GameTypeVersus() },
                                Beatmap =
                                {
                                    Value = new BeatmapInfo
                                    {
                                        StarDifficulty = info.BeatmapStars,
                                        Ruleset = rulesets.GetRuleset(info.RulesetID),
                                        Metadata = new BeatmapMetadata
                                        {
                                            Title = info.BeatmapTitle,
                                            Artist = info.BeatmapArtist,
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
                        Rooms = roomList;
                        break;
                    case MatchCreatedPacket matchCreated:
                        roomList.Add(new Room
                        {
                            Name = { Value = matchCreated.MatchInfo.Name },
                            Host = { Value = new User { Username = matchCreated.MatchInfo.Username, Id = matchCreated.MatchInfo.UserID, Country = new Country { FlagName = matchCreated.MatchInfo.UserCountry } } },
                            Status = { Value = new RoomStatusOpen() },
                            Type = { Value = new GameTypeVersus() },
                            Beatmap =
                            {
                                Value = new BeatmapInfo
                                {
                                    StarDifficulty = matchCreated.MatchInfo.BeatmapStars,
                                    Ruleset = rulesets.GetRuleset(matchCreated.MatchInfo.RulesetID),
                                    Metadata = new BeatmapMetadata
                                    {
                                        Title = matchCreated.MatchInfo.BeatmapTitle,
                                        Artist = matchCreated.MatchInfo.BeatmapArtist,
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
                        Rooms = roomList;
                        break;
                }
            };

            Children = new Drawable[]
            {
                Filter = new FilterControl(),
                content = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Children = new Drawable[]
                    {
                        new ScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,
                            Size = new Vector2(0.55f, 0.9f),
                            Padding = new MarginPadding
                            {
                                Vertical = 35 - DrawableRoom.SELECTION_BORDER_WIDTH,
                                Right = 20 - DrawableRoom.SELECTION_BORDER_WIDTH
                            },
                            Child = search = new SearchContainer
                            {
                                RelativeSizeAxes = Axes.X,
                                AutoSizeAxes = Axes.Y,
                                Child = RoomsContainer = new RoomsFilterContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                    Direction = FillDirection.Vertical,
                                    Spacing = new Vector2(10 - DrawableRoom.SELECTION_BORDER_WIDTH * 2),
                                }
                            }
                        },
                        new SettingsButton
                        {
                            Anchor = Anchor.BottomLeft,
                            Origin = Anchor.BottomLeft,
                            Position = new Vector2(12, -12),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.2f,
                            Text = "Create Game",
                            Action = () =>
                            {
                                OsuNetworkingClientHandler.SendPacket(new CreateMatchPacket
                                {
                                    MatchInfo = new MatchListPacket.MatchInfo
                                    {

                                    }
                                });
                            }
                        },
                        Inspector = new RoomInspector
                        {
                            Anchor = Anchor.TopRight,
                            Origin = Anchor.TopRight,
                            RelativeSizeAxes = Axes.Both,
                            Width = 0.45f,
                        }
                    }
                }
            };

            Filter.Search.Current.ValueChanged += s => filterRooms();
            Filter.Tabs.Current.ValueChanged += t => filterRooms();
            Filter.Search.Exit += Exit;
        }

        [BackgroundDependencyLoader]
        private void load(RulesetStore rulesets)
        {
            this.rulesets = rulesets;
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

        protected override void OnFocus(InputState state)
        {
            GetContainingInputManager().ChangeFocus(Filter.Search);
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);
            Filter.Search.HoldFocus = true;
        }

        protected override bool OnExiting(Screen next)
        {
            Filter.Search.HoldFocus = false;
            return base.OnExiting(next);
        }

        protected override void OnResuming(Screen last)
        {
            base.OnResuming(last);
            Filter.Search.HoldFocus = true;
        }

        protected override void OnSuspending(Screen next)
        {
            base.OnSuspending(next);
            Filter.Search.HoldFocus = false;
        }

        private void filterRooms()
        {
            search.SearchTerm = Filter.Search.Current.Value ?? string.Empty;

            foreach (DrawableRoom r in RoomsContainer.Children)
            {
                r.MatchingFilter = r.MatchingFilter &&
                                   r.Room.Availability.Value == (RoomAvailability)Filter.Tabs.Current.Value;
            }
        }

        private void didSelect(DrawableRoom room)
        {
            RoomsContainer.Children.ForEach(c =>
            {
                if (c != room)
                    c.State = SelectionState.NotSelected;
            });

            Inspector.Room = room.Room;

            // open the room if its selected and is clicked again
            if (room.State == SelectionState.Selected)
            {
                OsuNetworkingClientHandler.SendPacket(new JoinMatchPacket
                {
                    OsuClientInfo = OsuNetworkingClientHandler.OsuClientInfo,
                    MatchInfo = new MatchListPacket.MatchInfo
                    {
                        Name = room.Room.Name.Value,
                        Username = room.Room.Host.Value.Username,
                        //Status = { Value = new RoomStatusOpen() },
                        //Type = { Value = new GameTypeVersus() },
                        BeatmapStars = room.Room.Beatmap.Value.StarDifficulty,
                        RulesetID = room.Room.Beatmap.Value.Ruleset.ID.Value,
                        BeatmapTitle = room.Room.Beatmap.Value.Metadata.Title,
                        BeatmapArtist = room.Room.Beatmap.Value.Metadata.Artist,
                        //TODO: Players
                    }
                });
                OsuNetworkingClientHandler.OnPacketReceive += packet =>
                {
                    if (packet is JoinedMatchPacket joinedMatch)
                        Push(new Match(OsuNetworkingClientHandler, joinedMatch));
                };
            }
        }

        private class RoomsFilterContainer : FillFlowContainer<DrawableRoom>, IHasFilterableChildren
        {
            public IEnumerable<string> FilterTerms => new string[] { };
            public IEnumerable<IFilterable> FilterableChildren => Children;

            public bool MatchingFilter
            {
                set
                {
                    if (value)
                        InvalidateLayout();
                }
            }

            public RoomsFilterContainer()
            {
                LayoutDuration = 200;
                LayoutEasing = Easing.OutQuint;
            }
        }
    }
}
