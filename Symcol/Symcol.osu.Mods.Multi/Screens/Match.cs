using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Online.Multiplayer;
using osu.Game.Overlays.Settings;
using Symcol.Core.Networking;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Networking.Packets;
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

            OsuNetworkingClientHandler.OnPacketReceive += packet =>
            {
                if (packet is SetMapPacket mapPacket)
                {
                    foreach (BeatmapSetInfo beatmapSet in beatmaps.GetAllUsableBeatmapSets())
                        if (mapPacket.OnlineBeatmapID != -1 && beatmapSet.OnlineBeatmapSetID == mapPacket.OnlineBeatmapSetID)
                        {
                            foreach (BeatmapInfo beatmap in beatmapSet.Beatmaps)
                                if (beatmap.OnlineBeatmapID == mapPacket.OnlineBeatmapID)
                                {
                                    Beatmap.Value = beatmaps.GetWorkingBeatmap(beatmap, Beatmap.Value);
                                    Beatmap.Value.Track.Start();
                                    MatchTools.MapChange(Beatmap);
                                    return;
                                }
                            break;
                        }
                        else if (mapPacket.BeatmapName == beatmapSet.Metadata.Title && mapPacket.Mapper == beatmapSet.Metadata.Author.Username)
                        {
                            foreach (BeatmapInfo beatmap in beatmapSet.Beatmaps)
                                if (mapPacket.BeatmapDifficulty == beatmap.Version)
                                {
                                    Beatmap.Value = beatmaps.GetWorkingBeatmap(beatmap, Beatmap.Value);
                                    Beatmap.Value.Track.Start();
                                    MatchTools.MapChange(Beatmap);
                                    return;
                                }
                            break;
                        }

                    MatchTools.MapChange(mapPacket.OnlineBeatmapSetID);
                }
            };
            playerList.Add(OsuNetworkingClientHandler.OsuClientInfo);
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

            Push(new Player(OsuNetworkingClientHandler));
        }

        private void openSongSelect()
        {
            SongSelect songSelect = new SongSelect(OsuNetworkingClientHandler);
            Push(songSelect);
            songSelect.SelectionFinalised = map =>
            {
                try
                {
                    OsuNetworkingClientHandler.SendPacket(new SetMapPacket
                    {
                        OnlineBeatmapSetID = (int)map.BeatmapSetInfo.OnlineBeatmapSetID,
                        OnlineBeatmapID = (int)map.BeatmapInfo.OnlineBeatmapID
                    });
                }
                catch
                {
                    OsuNetworkingClientHandler.SendPacket(new SetMapPacket
                    {
                        BeatmapName = map.Metadata.Title,
                        BeatmapDifficulty = map.BeatmapInfo.Version,
                        Mapper = map.Metadata.Author.Username
                    });
                }
            };
        }
    }
}

