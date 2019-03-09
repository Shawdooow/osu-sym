using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.IEnumerableExtensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.UserInterface;
using osu.Game.Online.Multiplayer;
using osu.Game.Screens.Multi.Lounge.Components;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Lobby;
using osu.Mods.Online.Multi.Screens.Pieces;
using osuTK;

namespace osu.Mods.Online.Multi.Screens.Mods
{
    public class OnlineRoomsContainer : MultiplayerContainer
    {
        public Action<OnlineRoom> JoinRequested;

        private readonly Bindable<OnlineRoom> selectedRoom = new Bindable<OnlineRoom>();
        public IBindable<OnlineRoom> SelectedRoom => selectedRoom;

        private readonly IBindableList<OnlineRoom> rooms = new BindableList<OnlineRoom>();

        public readonly FillFlowContainer<DrawableOnlineRoom> RoomFlow;

        public OnlineRoomsContainer(OsuNetworkingHandler osuNetworkingHandler)
            : base(osuNetworkingHandler)
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;

            InternalChild = RoomFlow = new FillFlowContainer<DrawableOnlineRoom>
            {
                RelativeSizeAxes = Axes.X,
                AutoSizeAxes = Axes.Y,
                Direction = FillDirection.Vertical,
                Spacing = new Vector2(2),
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            rooms.ItemsAdded += addRooms;
            rooms.ItemsRemoved += removeRooms;

            addRooms(rooms);
        }

        private FilterCriteria currentFilter;

        public void Filter(FilterCriteria criteria)
        {
            RoomFlow.Children.ForEach(r =>
            {
                if (criteria == null)
                    r.MatchingFilter = true;
                else
                {
                    bool matchingFilter = true;
                    matchingFilter &= r.FilterTerms.Any(term => term.IndexOf(criteria.SearchString, StringComparison.InvariantCultureIgnoreCase) >= 0);

                    switch (criteria.SecondaryFilter)
                    {
                        default:
                        case SecondaryFilter.Public:
                            r.MatchingFilter = r.Room.Availability.Value == RoomAvailability.Public;
                            break;
                    }

                    r.MatchingFilter = matchingFilter;
                }
            });

            currentFilter = criteria;
        }

        private void addRooms(IEnumerable<OnlineRoom> rooms)
        {
            foreach (var r in rooms)
                RoomFlow.Add(new DrawableOnlineRoom(r) { Action = () => selectRoom(r) });

            Filter(currentFilter);
        }

        private void removeRooms(IEnumerable<OnlineRoom> rooms)
        {
            foreach (var r in rooms)
            {
                var toRemove = RoomFlow.Single(d => d.Room == r);
                toRemove.Action = null;

                RoomFlow.Remove(toRemove);

                selectRoom(null);
            }
        }

        private void selectRoom(OnlineRoom room)
        {
            var drawable = RoomFlow.FirstOrDefault(r => r.Room == room);

            if (drawable != null && drawable.State == SelectionState.Selected)
            {
                MatchListPacket.MatchInfo match = new MatchListPacket.MatchInfo
                {
                    Name = room.Name.Value,
                    Username = room.Host.Value.Username,
                    //Status = { Value = new RoomStatusOpen() },
                    //Type = { Value = new GameTypeVersus() },
                    BeatmapStars = room.Beatmap.Value.StarDifficulty,
                    RulesetID = room.Beatmap.Value.Ruleset.ID,
                    BeatmapTitle = room.Beatmap.Value.Metadata.Title,
                    BeatmapArtist = room.Beatmap.Value.Metadata.Artist,
                    //TODO: Players
                };
                SendPacket(new JoinMatchPacket
                {
                    Match = match,
                    User = OsuNetworkingHandler.OsuUserInfo
                });
            }
            else
                RoomFlow.Children.ForEach(r => r.State = r.Room == room ? SelectionState.Selected : SelectionState.NotSelected);

            selectedRoom.Value = room;
        }
    }
}
