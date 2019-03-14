using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets;
using osu.Mods.Online.Multi.Packets.Lobby;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Screens.Pieces;
using Symcol.Networking.Packets;

namespace osu.Mods.Online.Multi.Screens
{
    public class Match : MultiScreen
    {
        protected MatchTools MatchTools;

        private Bindable<RulesetInfo> ruleset;

        public Match(OsuNetworkingHandler osuNetworkingHandler, JoinedMatchPacket joinedPacket)
            : base(osuNetworkingHandler)
        {
            Name = "Match";

            MatchPlayerList playerList;
            Children = new Drawable[]
            {
                new Container
                {
                    Name = "Left Buttons",

                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,

                    RelativeSizeAxes = Axes.X,
                    Width = 0.5f,

                    Children = new Drawable[]
                    {
                        new SettingsButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.46f,
                            Text = "Leave",
                            Action = this.Exit
                        },
                        new SettingsButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.46f,
                            Text = "Open Song Select",
                            Action = openSongSelect
                        },
                    }
                },
                new Container
                {
                    Name = "Right Buttons",

                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,

                    RelativeSizeAxes = Axes.X,
                    Width = 0.5f,

                    Children = new Drawable[]
                    {
                        new SettingsButton
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.92f,
                            Text = "Start Match",
                            Action = () => SendPacket(new LoadPlayerPacket())
                        },
                        //TODO: Ready Up!
                        /*
                        new SettingsButton
                        {
                            Alpha = 0,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.46f,
                            Text = "Ready Up",
                            Action = toggleReady
                        },
                        */
                    }
                },
                playerList = new MatchPlayerList(OsuNetworkingHandler),
                MatchTools = new MatchTools(OsuNetworkingHandler),
                new Chat(OsuNetworkingHandler)
            };

            foreach (OsuUserInfo user in joinedPacket.MatchInfo.Users)
                playerList.Add(user);
        }

        [BackgroundDependencyLoader]
        private void load(Bindable<RulesetInfo> ruleset)
        {
            this.ruleset = ruleset;
            SendPacket(new GetMapPacket());
        }

        protected override void OnPacketRecieve(PacketInfo info)
        {
            if (info.Packet is PlayerLoadingPacket loading)
                Load(loading.Match);
        }

        protected virtual void Load(MatchInfo match)
        {
            if (MatchTools.SelectedBeatmap != null && !Beatmap.Disabled)
                Beatmap.Value = MatchTools.Beatmaps.GetWorkingBeatmap(MatchTools.SelectedBeatmap, Beatmap);

            if (MatchTools.SelectedRuleset != null && !ruleset.Disabled)
                ruleset.Value = MatchTools.SelectedRuleset;

            Push(new MultiPlayer(OsuNetworkingHandler, match));
        }

        protected override void Dispose(bool isDisposing)
        {
            SendPacket(new LeavePacket());
            base.Dispose(isDisposing);
        }

        private void openSongSelect()
        {
            SongSelect songSelect = new SongSelect();
            Push(songSelect);
            songSelect.SelectionFinalised = map =>
            {
                try
                {
                    OsuNetworkingHandler.SendToServer(new SetMapPacket(new Map
                    {
                        OnlineBeatmapSetID = (int)map.BeatmapSetInfo.OnlineBeatmapSetID,
                        OnlineBeatmapID = (int)map.BeatmapInfo.OnlineBeatmapID,
                        BeatmapTitle = map.Metadata.Title,
                        BeatmapArtist = map.Metadata.Artist,
                        BeatmapMapper = map.Metadata.Author.Username,
                        BeatmapDifficulty = map.BeatmapInfo.Version,
                        RulesetShortname = ruleset.Value.ShortName,
                    }));
                }
                catch
                {
                    //try to fallback for old maps
                    OsuNetworkingHandler.SendToServer(new SetMapPacket(new Map
                    {
                        BeatmapTitle = map.Metadata.Title,
                        BeatmapArtist = map.Metadata.Artist,
                        BeatmapMapper = map.Metadata.Author.Username,
                        BeatmapDifficulty = map.BeatmapInfo.Version,
                        RulesetShortname = ruleset.Value.ShortName,
                    }));
                }
            };
        }
    }
}

