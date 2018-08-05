﻿using System.Collections.Generic;
using System.Threading.Tasks;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Game.Beatmaps;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using Symcol.Core.Networking.Packets;
using Symcol.osu.Mods.Multi.Networking;
using Symcol.osu.Mods.Multi.Networking.Packets.Lobby;
using Symcol.osu.Mods.Multi.Networking.Packets.Match;
using Symcol.osu.Mods.Multi.Screens.Pieces;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class Match : MultiScreen
    {
        private readonly MatchListPacket.MatchInfo match;

        private BeatmapManager beatmaps;

        protected MatchTools MatchTools;

        private RulesetStore rulesets;

        public Match(OsuNetworkingClientHandler osuNetworkingClientHandler, JoinedMatchPacket joinedPacket, MatchListPacket.MatchInfo match)
            : base(osuNetworkingClientHandler)
        {
            this.match = match;

            osuNetworkingClientHandler.SendPacket(new GetMapPacket());

            MatchPlayerList playerList;
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
                new Chat(OsuNetworkingClientHandler)
            };

            foreach (OsuClientInfo player in joinedPacket.Players)
                playerList.Add(player);
        }

        [BackgroundDependencyLoader]
        private void load(BeatmapManager beatmaps, RulesetStore rulesets)
        {
            this.beatmaps = beatmaps;
            this.rulesets = rulesets;

            OsuNetworkingClientHandler.OnPacketReceive += setMap;
        }

        private void setMap(Packet packet)
        {
            //Don't freeze anymore :P
            if (packet is SetMapPacket mapPacket)
                Task.Factory.StartNew(() =>
                {
                    Ruleset.Value = rulesets.GetRuleset(mapPacket.RulesetID);
                    MatchTools.MapChange(mapPacket.OnlineBeatmapSetID, mapPacket.RulesetID);
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
                        //try to fallback for old maps
                        else if (mapPacket.BeatmapTitle == beatmapSet.Metadata.Title && mapPacket.BeatmapMapper == beatmapSet.Metadata.Author.Username)
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
                }, TaskCreationOptions.LongRunning);
        }

        protected virtual void Load(List<OsuClientInfo> players)
        {
            if (MatchTools.SelectedBeatmap != null)
                Beatmap.Value = MatchTools.SelectedBeatmap;

            if (MatchTools.SelectedRuleset != null)
                Ruleset.Value = MatchTools.SelectedRuleset;

            Push(new Player(OsuNetworkingClientHandler));
        }

        protected override void Dispose(bool isDisposing)
        {
            OsuNetworkingClientHandler.SendPacket(new LeavePacket());
            // ReSharper disable once DelegateSubtraction
            OsuNetworkingClientHandler.OnPacketReceive -= setMap;
            base.Dispose(isDisposing);
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
                        OnlineBeatmapID = (int)map.BeatmapInfo.OnlineBeatmapID,
                        BeatmapTitle = map.Metadata.Title,
                        BeatmapArtist = map.Metadata.Artist,
                        BeatmapMapper = map.Metadata.Author.Username,
                        BeatmapDifficulty = map.BeatmapInfo.Version,
                        RulesetID = Ruleset.Value.ID.Value
                    });
                }
                catch
                {
                    //try to fallback for old maps
                    OsuNetworkingClientHandler.SendPacket(new SetMapPacket
                    {
                        BeatmapTitle = map.Metadata.Title,
                        BeatmapArtist = map.Metadata.Artist,
                        BeatmapMapper = map.Metadata.Author.Username,
                        BeatmapDifficulty = map.BeatmapInfo.Version,
                        RulesetID = Ruleset.Value.ID.Value
                    });
                }
            };
        }
    }
}
