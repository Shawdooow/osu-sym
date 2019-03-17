using System;
using System.Linq;
using System.Threading.Tasks;
using osu.Core;
using osu.Core.Config;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Cursor;
using osu.Framework.Input.Events;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Framework.Threading;
using osu.Framework.Timing;
using osu.Game;
using osu.Game.Beatmaps;
using osu.Game.Configuration;
using osu.Game.Graphics;
using osu.Game.Graphics.Cursor;
using osu.Game.Online.API;
using osu.Game.Overlays;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Play;
using osu.Game.Skinning;
using osu.Game.Storyboards.Drawables;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Player.Packets;
using osu.Mods.Online.Multi.Player.Pieces;
using osu.Mods.Online.Multi.Rulesets;
using osu.Mods.Online.Multi.Settings;
using osuTK;
using osuTK.Input;
using Sym.Base.Graphics.Containers;
using Sym.Networking.Packets;

namespace osu.Mods.Online.Multi.Player
{
    public class MultiPlayer : ScreenWithBeatmapBackground, IProvideCursor
    {
        public override float BackgroundParallaxAmount => 0.1f;

        public override bool HideOverlaysOnEnter => true;

        public override OverlayActivation InitialOverlayActivationMode => OverlayActivation.UserTriggered;

        public bool AllowPause { get; set; } = true;
        public bool AllowLeadIn { get; set; } = true;
        public bool AllowResults { get; set; } = true;

        private Bindable<bool> mouseWheelDisabled;
        private Bindable<double> userAudioOffset;

        public CursorContainer Cursor => RulesetContainer.Cursor;
        public bool ProvidingUserCursor => RulesetContainer?.Cursor != null && !RulesetContainer.HasReplayLoaded.Value;

        private IAdjustableClock sourceClock;

        /// <summary>
        /// The decoupled clock used for gameplay. Should be used for seeks and clock control.
        /// </summary>
        private DecoupleableInterpolatingFramedClock adjustableClock;

        private RulesetInfo ruleset;

        private APIAccess api;

        private OsuGame osu;

        private SampleChannel sampleRestart;

        [Resolved]
        private ScoreManager scoreManager { get; set; }

        protected ScoreProcessor ScoreProcessor;
        protected RulesetContainer RulesetContainer;

        private HUDOverlay hudOverlay;

        private DrawableStoryboard storyboard;
        private Container storyboardContainer;

        public bool LoadedBeatmapSuccessfully => RulesetContainer?.Objects.Any() == true;

        public readonly OsuNetworkingHandler OsuNetworkingHandler;

        protected readonly MatchInfo Match;

        protected DeadContainer<MultiCursorContainer> CursorContainer;

        protected bool LiveSpectator;

        public MultiPlayer(OsuNetworkingHandler osuNetworkingHandler, MatchInfo match)
        {
            Name = "MultiPlayer";
            OsuNetworkingHandler = osuNetworkingHandler;
            Match = match;
            OsuNetworkingHandler.OnPacketReceive += handlePackets;

            foreach (Setting setting in Match.Settings)
                if (setting.Name == "Live Spectator" && setting is Setting<bool> value)
                    LiveSpectator = value.Value;
        }

        private void handlePackets(PacketInfo info)
        {
            switch (info.Packet)
            {
                case MatchStartingPacket start:
                    adjustableClock.Start();
                    break;
                case MatchExitPacket exit:
                    this.Exit();
                    break;
                case CursorPositionPacket position:
                    foreach (MultiCursorContainer c in CursorContainer)
                        if (position.ID.ToString() == c.Name)
                            c.ActiveCursor.MoveTo(new Vector2(position.X + osu.DrawSize.X / 2, position.Y + osu.DrawSize.Y / 2), 1000f / 30f);
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu, AudioManager audio, APIAccess api, OsuConfigManager config)
        {
            this.api = api;
            this.osu = osu;

            WorkingBeatmap working = Beatmap.Value;
            if (working is DummyWorkingBeatmap)
                return;

            sampleRestart = audio.Sample.Get(@"Gameplay/restart");

            mouseWheelDisabled = config.GetBindable<bool>(OsuSetting.MouseDisableWheel);
            userAudioOffset = config.GetBindable<double>(OsuSetting.AudioOffset);

            IBeatmap beatmap;

            try
            {
                beatmap = working.Beatmap;

                if (beatmap == null)
                    throw new InvalidOperationException("Beatmap was not loaded");

                ruleset = Ruleset.Value ?? beatmap.BeatmapInfo.Ruleset;
                Ruleset rulesetInstance = ruleset.CreateInstance();

                try
                {
                    if (rulesetInstance is IRulesetMulti multiInstance)
                        RulesetContainer = multiInstance.CreateRulesetContainerMulti(working, OsuNetworkingHandler, Match);
                }
                catch (Exception e)
                {
                    Logger.Error(e, "Failed to create multi RulesetContainer!");
                }

                if (RulesetContainer == null)
                    RulesetContainer = rulesetInstance.CreateRulesetContainerWith(working);

                if (!RulesetContainer.Objects.Any())
                {
                    Logger.Error(new InvalidOperationException("Beatmap contains no hit objects!"), "Beatmap contains no hit objects!");
                    return;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Could not load beatmap sucessfully!");
                this.Exit();
                return;
            }

            sourceClock = (IAdjustableClock)working.Track ?? new StopwatchClock();
            adjustableClock = new DecoupleableInterpolatingFramedClock { IsCoupled = false };

            adjustableClock.Seek(AllowLeadIn
                ? Math.Min(0, RulesetContainer.GameplayStartTime - beatmap.BeatmapInfo.AudioLeadIn)
                : RulesetContainer.GameplayStartTime);

            adjustableClock.ProcessFrame();

            // Lazer's audio timings in general doesn't match stable. This is the result of user testing, albeit limited.
            // This only seems to be required on windows. We need to eventually figure out why, with a bit of luck.
            var platformOffsetClock = new FramedOffsetClock(adjustableClock) { Offset = RuntimeInfo.OS == RuntimeInfo.Platform.Windows ? 22 : 0 };

            // the final usable gameplay clock with user-set offsets applied.
            var offsetClock = new FramedOffsetClock(platformOffsetClock);

            userAudioOffset.ValueChanged += v => offsetClock.Offset = v;
            userAudioOffset.TriggerChange();

            ScoreProcessor = RulesetContainer.CreateScoreProcessor();

            try
            {
                RulesetContainer.Cursor.ActiveCursor.Colour = OsuColour.FromHex(SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.PlayerColor));
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to set Cursor color!");
            }

            InternalChildren = new[]
            {
                new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Clock = offsetClock,
                    Children = new[]
                    {
                        storyboardContainer = new Container
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0,
                        },
                        new LocalSkinOverrideContainer(working.Skin)
                        {
                            RelativeSizeAxes = Axes.Both,
                            Child = RulesetContainer
                        },
                        new Scoreboard(OsuNetworkingHandler, Match.Users, ScoreProcessor),
                        new BreakOverlay(beatmap.BeatmapInfo.LetterboxInBreaks, ScoreProcessor)
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            ProcessCustomClock = false,
                            Breaks = beatmap.Breaks
                        },
                        RulesetContainer.Cursor?.CreateProxy() ?? new Container(),
                        CursorContainer = new DeadContainer<MultiCursorContainer>
                        {
                            RelativeSizeAxes = Axes.Both
                        },
                        hudOverlay = new HUDOverlay(ScoreProcessor, RulesetContainer, working, offsetClock, adjustableClock)
                        {
                            Clock = Clock, // hud overlay doesn't want to use the audio clock directly
                            ProcessCustomClock = false,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre
                        },
                        //TODO: voting on this
                        /*
                        new SkipOverlay(RulesetContainer.GameplayStartTime)
                        {
                            Clock = Clock, // skip button doesn't want to use the audio clock directly
                            ProcessCustomClock = false,
                            AdjustableClock = adjustableClock,
                            FramedClock = offsetClock,
                        },
                        */
                    }
                }
            };

            if (LiveSpectator)
                foreach (OsuUserInfo user in Match.Users)
                    if (user.ID != OsuNetworkingHandler.OsuUserInfo.ID)
                    {
                        try
                        {
                            MultiCursorContainer c = new MultiCursorContainer();

                            if (RulesetContainer.Cursor != null && RulesetContainer.Cursor is MultiCursorContainer m && m.CreateMultiCursor() != null)
                                c = m.CreateMultiCursor();

                            c.Colour = OsuColour.FromHex(user.Colour);
                            c.Name = user.ID.ToString();
                            c.Slave = true;
                            c.Alpha = 0.5f;
                            CursorContainer.Add(c);
                        }
                        catch { }
                    }

            if (!ScoreProcessor.Mode.Disabled)
                config.BindWith(OsuSetting.ScoreDisplayMode, ScoreProcessor.Mode);

            hudOverlay.HoldToQuit.Action = this.Exit;
            hudOverlay.KeyCounter.Visible.BindTo(RulesetContainer.HasReplayLoaded);

            if (ShowStoryboard)
                initializeStoryboard(false);

            // Bind ScoreProcessor to ourselves
            ScoreProcessor.AllJudged += onCompletion;
            ScoreProcessor.Failed += onFail;

            foreach (var mod in Beatmap.Value.Mods.Value.OfType<IApplicableToScoreProcessor>())
                mod.ApplyToScoreProcessor(ScoreProcessor);
        }

        private void applyRateFromMods()
        {
            if (sourceClock == null) return;

            sourceClock.Rate = 1;
            foreach (var mod in Beatmap.Value.Mods.Value.OfType<IApplicableToClock>())
                mod.ApplyToClock(sourceClock);
        }

        protected virtual ScoreInfo CreateScore()
        {
            var score = new ScoreInfo
            {
                Beatmap = Beatmap.Value.BeatmapInfo,
                Ruleset = ruleset,
                User = api.LocalUser.Value
            };

            ScoreProcessor.PopulateScore(score);

            return score;
        }

        private ScheduledDelegate onCompletionEvent;

        private void onCompletion()
        {
            // Only show the completion screen if the player hasn't failed
            if (ScoreProcessor.HasFailed || onCompletionEvent != null)
                return;

            if (!AllowResults) return;

            ValidForResume = false;

            using (BeginDelayedSequence(1000))
            {
                onCompletionEvent = Schedule(delegate
                {
                    var score = CreateScore();
                    //if (RulesetContainer.Replay == null)
                        //scoreManager.Import(score, true);
                    //Push(new Results(score));
                    this.Exit();
                });
            }
        }


        private bool onFail()
        {
            if (Beatmap.Value.Mods.Value.OfType<IApplicableFailOverride>().Any(m => !m.AllowFail))
                return false;
            return true;
        }

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            if (!LoadedBeatmapSuccessfully)
                return;

            this.Alpha = 0;
            this
                .ScaleTo(0.7f)
                .ScaleTo(1, 750, Easing.OutQuint)
                .Delay(250)
                .FadeIn(250);

            Task.Run(() =>
            {
                sourceClock.Reset();

                Schedule(() =>
                {
                    adjustableClock.ChangeSource(sourceClock);
                    applyRateFromMods();

                    this.Delay(750).Schedule(() =>
                    {
                        Logger.Log("Client finnished loading", LoggingTarget.Network);
                        OsuNetworkingHandler.SendToServer(new PlayerLoadedPacket());
                    });
                });
            });
        }

        public override void OnSuspending(IScreen next)
        {
            fadeOut();
            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            OsuNetworkingHandler.OnPacketReceive -= handlePackets;
            applyRateFromMods();

            fadeOut();
            return base.OnExiting(next);
        }

        private void fadeOut()
        {
            const float fade_out_duration = 250;

            RulesetContainer?.FadeOut(fade_out_duration);
            this.FadeOut(fade_out_duration);

            hudOverlay?.ScaleTo(0.7f, fade_out_duration * 3, Easing.In);

            Background?.FadeTo(1f, fade_out_duration);
        }

        protected override bool OnScroll(ScrollEvent e) => mouseWheelDisabled.Value;

        protected override bool OnKeyDown(KeyDownEvent e)
        {
            if (e.Key == Key.Escape && !e.Repeat)
            {
                OsuNetworkingHandler.SendToServer(new MatchExitPacket());
                this.Exit();
                return true;
            }

            return base.OnKeyDown(e);
        }

        private void initializeStoryboard(bool asyncLoad)
        {
            if (storyboardContainer == null)
                return;

            var beatmap = Beatmap.Value;

            storyboard = beatmap.Storyboard.CreateDrawable();
            storyboard.Masking = true;

            if (asyncLoad)
                LoadComponentAsync(storyboard, storyboardContainer.Add);
            else
                storyboardContainer.Add(storyboard);
        }

        protected override void UpdateBackgroundElements()
        {
            base.UpdateBackgroundElements();

            if (ShowStoryboard && storyboard == null)
                initializeStoryboard(true);

            var beatmap = Beatmap.Value;
            var storyboardVisible = ShowStoryboard && beatmap.Storyboard.HasDrawable;

            storyboardContainer?
                .FadeColour(OsuColour.Gray(BackgroundOpacity), BACKGROUND_FADE_DURATION, Easing.OutQuint)
                .FadeTo(storyboardVisible && BackgroundOpacity > 0 ? 1 : 0, BACKGROUND_FADE_DURATION, Easing.OutQuint);

            if (storyboardVisible && beatmap.Storyboard.ReplacesBackground)
                Background?.FadeTo(0, BACKGROUND_FADE_DURATION, Easing.OutQuint);
        }

        private double boo = double.MinValue;
        protected override void Update()
        {
            base.Update();

            if (boo <= Time.Current)
            {
                //30 packets per second test
                boo = Time.Current + 1000f / 30f;

                if (LiveSpectator && RulesetContainer.Cursor != null)
                    OsuNetworkingHandler.SendToServer(new CursorPositionPacket
                    {
                        ID = OsuNetworkingHandler.OsuUserInfo.ID,
                        X = RulesetContainer.Cursor.ActiveCursor.Position.X - osu.DrawSize.X / 2,
                        Y = RulesetContainer.Cursor.ActiveCursor.Position.Y - osu.DrawSize.Y / 2,
                    });
            }
        }
    }
}
