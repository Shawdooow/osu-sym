using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Lobby.Packets;
using osu.Mods.Online.Multi.Match.Packets;
using osu.Mods.Online.Multi.Match.Pieces;
using osu.Mods.Online.Multi.Player;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Match
{
    public class MatchScreen : MultiScreen
    {
        private readonly MatchTools matchTools;

        private Bindable<RulesetInfo> ruleset;

        private readonly Bindable<bool> ready = new Bindable<bool> { Default = false };

        private readonly SettingsButton startButton;
        private readonly SettingsButton readyButton;

        public MatchScreen(OsuNetworkingHandler osuNetworkingHandler, JoinedMatchPacket joinedPacket)
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
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.5f,
                            Text = "Leave",
                            Action = this.Exit
                        },
                        new SettingsButton
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.5f,
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
                        startButton = new SettingsButton
                        {
                            Alpha = 0,
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            RelativeSizeAxes = Axes.X,
                            Width = 0.5f,
                            Text = "Start Match",
                            Action = () => SendPacket(new LoadPlayerPacket())
                        },
                        readyButton = new SettingsButton
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            RelativeSizeAxes = Axes.X,
                            Width = 1f,
                            Text = "Ready Up",
                            Action = toggleReady
                        },
                    }
                },
                playerList = new MatchPlayerList(OsuNetworkingHandler),
                matchTools = new MatchTools(OsuNetworkingHandler),
                new Chat(OsuNetworkingHandler)
            };

            foreach (OsuUserInfo user in joinedPacket.MatchInfo.Users)
                playerList.Add(user);

            matchTools.OnMapSearching += () =>
            {
                setStatues(PlayerStatues.SearchingForMap);
                unready();
            };
            matchTools.OnMapFound += () => setStatues(PlayerStatues.FoundMap);
            matchTools.OnMapMissing += () => setStatues(PlayerStatues.MissingMap);
            matchTools.OnRulesetMissing += () => setStatues(PlayerStatues.MissingRuleset);
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
            if (matchTools.SelectedBeatmap != null && !Beatmap.Disabled)
                Beatmap.Value = matchTools.Beatmaps.GetWorkingBeatmap(matchTools.SelectedBeatmap, Beatmap);

            if (matchTools.SelectedRuleset != null && !ruleset.Disabled)
                ruleset.Value = matchTools.SelectedRuleset;

            Push(new MultiPlayer(OsuNetworkingHandler, match));
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
                        BeatmapStars = map.BeatmapInfo.StarDifficulty,
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
                        BeatmapStars = map.BeatmapInfo.StarDifficulty,
                        RulesetShortname = ruleset.Value.ShortName,
                    }));
                }
            };
        }

        private void toggleReady()
        {
            if (matchTools.Searching) return;

            if (ready.Value)
                unready();
            else
                readyup();
        }

        private void unready()
        {
            ready.Value = false;
            readyButton.Text = "Ready Up";

            if (!matchTools.Searching) setStatues(PlayerStatues.FoundMap);

            readyButton.ResizeWidthTo(1f, 200, Easing.OutCubic);
            startButton.FadeOutFromOne(100);
        }

        private void readyup()
        {
            ready.Value = true;
            readyButton.Text = "UnReady";

            setStatues(PlayerStatues.Ready);

            readyButton.ResizeWidthTo(0.5f, 200, Easing.OutCubic);
            startButton.Delay(100).FadeInFromZero(100);
        }

        private void setStatues(PlayerStatues statues)
        {
            OsuNetworkingHandler.OsuUserInfo.Statues = statues;
            OsuNetworkingHandler.SendToServer(new StatuesChangePacket());
        }

        protected override void Dispose(bool isDisposing)
        {
            SendPacket(new LeavePacket());
            base.Dispose(isDisposing);
        }
    }
}

