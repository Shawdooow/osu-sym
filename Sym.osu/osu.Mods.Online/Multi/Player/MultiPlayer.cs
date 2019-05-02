#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using osu.Core;
using osu.Core.Config;
using osu.Framework;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Framework.Bindables;
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
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Cursor;
using osu.Game.Online.API;
using osu.Game.Overlays;
using osu.Game.Rulesets;
using osu.Game.Rulesets.Mods;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.UI;
using osu.Game.Scoring;
using osu.Game.Screens.Play;
using osu.Game.Screens.Ranking;
using osu.Game.Skinning;
using osu.Game.Storyboards.Drawables;
using osu.Mods.Online.Base;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi.Player.Packets;
using osu.Mods.Online.Multi.Player.Pieces;
using osu.Mods.Online.Multi.Rulesets;
using osu.Mods.Online.Multi.Settings;
using osuTK;
using osuTK.Input;
using Sym.Base.Graphics.Containers;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Multi.Player
{
    public class MultiPlayer : ScreenWithBeatmapBackground
    {
        protected override bool AllowBackButton => false; // handled by HoldForMenuButton

        public override float BackgroundParallaxAmount => 0.1f;

        public override bool HideOverlaysOnEnter => true;

        public override OverlayActivation InitialOverlayActivationMode => OverlayActivation.UserTriggered;

        private Bindable<bool> mouseWheelDisabled;

        private readonly Bindable<bool> storyboardReplacesBackground = new Bindable<bool>();

        [Resolved]
        private ScoreManager scoreManager { get; set; }

        private RulesetInfo ruleset;

        protected ScoreProcessor ScoreProcessor { get; private set; }
        protected DrawableRuleset DrawableRuleset { get; private set; }

        protected HUDOverlay HUDOverlay { get; private set; }

        public bool LoadedBeatmapSuccessfully => DrawableRuleset?.Objects.Any() == true;

        protected GameplayClockContainer GameplayClockContainer { get; private set; }

        [Cached]
        [Cached(Type = typeof(IBindable<IReadOnlyList<Mod>>))]
        protected new readonly Bindable<IReadOnlyList<Mod>> Mods = new Bindable<IReadOnlyList<Mod>>(Array.Empty<Mod>());

        private OsuGame osu;

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

        private void handlePackets(PacketInfo<Sym.Networking.NetworkingHandlers.Peer.Host> info)
        {
            switch (info.Packet)
            {
                case MatchStartingPacket start:
                    //GameplayClockContainer.Start();
                    break;
                case MatchExitPacket exit:
                    this.Exit();
                    break;
                case Vector2Packet position:
                    if (position.Name == "cursor")
                        foreach (MultiCursorContainer c in CursorContainer)
                            if (position.ID.ToString() == c.Name)
                                c.ActiveCursor.MoveTo(new Vector2(position.X + osu.DrawSize.X / 2, position.Y + osu.DrawSize.Y / 2), 1000f / 30f);
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio, OsuConfigManager config, OsuGame osu)
        {
            this.osu = osu;

            Mods.Value = base.Mods.Value.Select(m => m.CreateCopy()).ToArray();

            WorkingBeatmap working = loadBeatmap();

            if (working == null)
                return;

            mouseWheelDisabled = config.GetBindable<bool>(OsuSetting.MouseDisableWheel);
            showStoryboard = config.GetBindable<bool>(OsuSetting.ShowStoryboard);

            ScoreProcessor = DrawableRuleset.CreateScoreProcessor();
            ScoreProcessor.Mods.BindTo(Mods);

            if (!ScoreProcessor.Mode.Disabled)
                config.BindWith(OsuSetting.ScoreDisplayMode, ScoreProcessor.Mode);

            InternalChild = GameplayClockContainer = new GameplayClockContainer(working, Mods.Value, DrawableRuleset.GameplayStartTime);

            GameplayClockContainer.Children = new[]
            {
                StoryboardContainer = CreateStoryboardContainer(),
                new ScalingContainer(ScalingMode.Gameplay)
                {
                    Child = new LocalSkinOverrideContainer(working.Skin)
                    {
                        RelativeSizeAxes = Axes.Both,
                        Child = DrawableRuleset
                    }
                },
                new BreakOverlay(working.Beatmap.BeatmapInfo.LetterboxInBreaks, ScoreProcessor)
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Breaks = working.Beatmap.Breaks
                },
                // display the cursor above some HUD elements.
                DrawableRuleset.Cursor?.CreateProxy() ?? new Container(),
                CursorContainer = new DeadContainer<MultiCursorContainer>
                {
                    RelativeSizeAxes = Axes.Both
                },
                HUDOverlay = new HUDOverlay(ScoreProcessor, DrawableRuleset, Mods.Value)
                {
                    HoldToQuit = { Action = () => OsuNetworkingHandler.SendToServer(new MatchExitPacket()) },
                    PlayerSettingsOverlay = { PlaybackSettings = { UserPlaybackRate = { BindTarget = GameplayClockContainer.UserPlaybackRate } } },
                    KeyCounter = { Visible = { BindTarget = DrawableRuleset.HasReplayLoaded } },
                    RequestSeek = GameplayClockContainer.Seek,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre
                },
                new SkipOverlay(DrawableRuleset.GameplayStartTime)
                {
                    RequestSeek = GameplayClockContainer.Seek
                },
            };

            if (LiveSpectator)
                foreach (OsuUser user in Match.Users)
                    if (user.ID != OsuNetworkingHandler.OsuUser.ID)
                    {
                        try
                        {
                            MultiCursorContainer c = new MultiCursorContainer();

                            if (DrawableRuleset.Cursor != null && DrawableRuleset.Cursor is MultiCursorContainer m && m.CreateMultiCursor() != null)
                                c = m.CreateMultiCursor();

                            c.Colour = OsuColour.FromHex(user.Colour);
                            c.Name = user.ID.ToString();
                            c.Slave = true;
                            c.Alpha = 0.5f;
                            CursorContainer.Add(c);
                        }
                        catch { }
                    }

            // bind clock into components that require it
            DrawableRuleset.IsPaused.BindTo(GameplayClockContainer.IsPaused);

            // load storyboard as part of player's load if we can
            initializeStoryboard(false);

            // Bind ScoreProcessor to ourselves
            ScoreProcessor.AllJudged += this.Exit;
            //ScoreProcessor.Failed += onFail;

            foreach (var mod in Mods.Value.OfType<IApplicableToScoreProcessor>())
                mod.ApplyToScoreProcessor(ScoreProcessor);
        }

        private double boo = double.MinValue;
        protected override void Update()
        {
            base.Update();

            if (boo <= Time.Current)
            {
                //30 packets per second test
                boo = Time.Current + 1000f / 30f;

                if (LiveSpectator && DrawableRuleset.Cursor != null)
                    OsuNetworkingHandler.SendToServer(new Vector2Packet
                    {
                        Name = "cursor",
                        ID = OsuNetworkingHandler.OsuUser.ID,
                        Vector2 = DrawableRuleset.Cursor.ActiveCursor.Position - osu.DrawSize / 2,
                    });
            }
        }

        private WorkingBeatmap loadBeatmap()
        {
            WorkingBeatmap working = Beatmap.Value;
            if (working is DummyWorkingBeatmap)
                return null;

            try
            {
                var beatmap = working.Beatmap;

                if (beatmap == null)
                    throw new InvalidOperationException("Beatmap was not loaded");

                ruleset = Ruleset.Value ?? beatmap.BeatmapInfo.Ruleset;
                var rulesetInstance = ruleset.CreateInstance();

                try
                {
                    DrawableRuleset = rulesetInstance.CreateDrawableRulesetWith(working, Mods.Value);
                }
                catch (BeatmapInvalidForRulesetException)
                {
                    // we may fail to create a DrawableRuleset if the beatmap cannot be loaded with the user's preferred ruleset
                    // let's try again forcing the beatmap's ruleset.
                    ruleset = beatmap.BeatmapInfo.Ruleset;
                    rulesetInstance = ruleset.CreateInstance();
                    DrawableRuleset = rulesetInstance.CreateDrawableRulesetWith(Beatmap.Value, Mods.Value);
                }

                if (!DrawableRuleset.Objects.Any())
                {
                    Logger.Log("Beatmap contains no hit objects!", level: LogLevel.Error);
                    return null;
                }
            }
            catch (Exception e)
            {
                Logger.Error(e, "Could not load beatmap sucessfully!");
                //couldn't load, hard abort!
                return null;
            }

            return working;
        }

        protected override bool OnScroll(ScrollEvent e) => mouseWheelDisabled.Value && !GameplayClockContainer.IsPaused.Value;

        #region Storyboard

        private DrawableStoryboard storyboard;
        protected UserDimContainer StoryboardContainer { get; private set; }

        protected virtual UserDimContainer CreateStoryboardContainer() => new UserDimContainer(true)
        {
            RelativeSizeAxes = Axes.Both,
            Alpha = 1,
            EnableUserDim = { Value = true }
        };

        private Bindable<bool> showStoryboard;

        private void initializeStoryboard(bool asyncLoad)
        {
            if (StoryboardContainer == null || storyboard != null)
                return;

            if (!showStoryboard.Value)
                return;

            var beatmap = Beatmap.Value;

            storyboard = beatmap.Storyboard.CreateDrawable();
            storyboard.Masking = true;

            if (asyncLoad)
                LoadComponentAsync(storyboard, StoryboardContainer.Add);
            else
                StoryboardContainer.Add(storyboard);
        }

        #endregion

        #region Screen Logic

        public override void OnEntering(IScreen last)
        {
            base.OnEntering(last);

            if (!LoadedBeatmapSuccessfully)
                return;

            Alpha = 0;
            this
                .ScaleTo(0.7f)
                .ScaleTo(1, 750, Easing.OutQuint)
                .Delay(250)
                .FadeIn(250);

            showStoryboard.ValueChanged += _ => initializeStoryboard(true);

            Background.EnableUserDim.Value = true;
            Background.BlurAmount.Value = 0;

            Background.StoryboardReplacesBackground.BindTo(storyboardReplacesBackground);
            StoryboardContainer.StoryboardReplacesBackground.BindTo(storyboardReplacesBackground);

            storyboardReplacesBackground.Value = Beatmap.Value.Storyboard.ReplacesBackground && Beatmap.Value.Storyboard.HasDrawable;

            GameplayClockContainer.Restart();
            GameplayClockContainer.FadeInFromZero(750, Easing.OutQuint);
        }

        public override void OnSuspending(IScreen next)
        {
            fadeOut();
            base.OnSuspending(next);
        }

        public override bool OnExiting(IScreen next)
        {
            if (!GameplayClockContainer.IsPaused.Value)
                // still want to block if we are within the cooldown period and not already paused.
                return true;

            GameplayClockContainer.ResetLocalAdjustments();

            fadeOut();
            return base.OnExiting(next);
        }

        private void fadeOut(bool instant = false)
        {
            float fadeOutDuration = instant ? 0 : 250;
            this.FadeOut(fadeOutDuration);

            Background.EnableUserDim.Value = false;
            storyboardReplacesBackground.Value = false;
        }

        #endregion
    }
}
