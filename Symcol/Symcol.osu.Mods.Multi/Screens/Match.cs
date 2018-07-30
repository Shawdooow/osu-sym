using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Game.Beatmaps;
using osu.Game.Online.Multiplayer;
using osu.Game.Overlays.Settings;
using Symcol.Core.Networking;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Screens.Pieces;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class Match : MultiScreen
    {
        private readonly MatchPlayerList playerList;

        private BeatmapManager beatmaps;

        protected MatchTools MatchTools;

        private readonly Chat chat;

        public Match(OsuNetworkingClientHandler osuNetworkingClientHandler, Room room)
            : base(osuNetworkingClientHandler)
        {
            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.35f,
                    Text = "Leave",
                    Action = Exit
                },
                new SettingsButton
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Open Song Select",
                    Action = openSongSelect
                },
                new SettingsButton
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.35f,
                    Text = "Start Match",
                    //Action = () => OsuNetworkingClientHandler.StartLoadingGame()
                },
                playerList = new MatchPlayerList(OsuNetworkingClientHandler),
                MatchTools = new MatchTools(),
                chat = new Chat(OsuNetworkingClientHandler)
            };
        }

        [BackgroundDependencyLoader]
        private void load(BeatmapManager beatmaps)
        {
            this.beatmaps = beatmaps;
        }

        protected virtual void Load(List<ClientInfo> playerList)
        {
            if (MatchTools.SelectedBeatmap != null)
                Beatmap.Value = MatchTools.SelectedBeatmap;
            else
                Logger.Log("Match started for a map we don't have!", LoggingTarget.Network, LogLevel.Error);

            Push(new Player(OsuNetworkingClientHandler));
        }

        private void openSongSelect()
        {
            SongSelect songSelect = new SongSelect(OsuNetworkingClientHandler);
            Push(songSelect);
            //songSelect.SelectionFinalised = (map) => OsuNetworkingClientHandler.SetMap(map);
        }
    }
}

