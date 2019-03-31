#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Input;
using osu.Framework.Input.Events;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.Bosses;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.Bosses.DrawableBosses;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Game.Rulesets.Vitaru.Sym.Multi.Packets;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi;
using osu.Mods.Online.Multi.Settings;
using osu.Mods.Rulesets.Core.Rulesets;
using osuTK;
using osuTK.Graphics;
using Sym.Networking.Packets;

#endregion

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields
{
    public class VitaruPlayfield : SymcolPlayfield, IRequireHighFrequencyMousePosition
    {
        public override bool ReceivePositionalInputAt(Vector2 screenSpacePos) => true;

        public static bool CHARGED;
        public static bool ACCEL;
        //For Sakuya only pretty much
        public static double ACCELMULTIPLIER = 1;
        public static bool HIDDEN;
        public static bool TRUEHIDDEN;
        public static bool FLASHLIGHT;

        public static Action<JudgementResult> OnResult;

        public static Bindable<int> HITOBJECT_COUNT = new Bindable<int>();

        public bool Cheated { get; internal set; }

        private readonly ChapterSet chapterSet = ChapterStore.GetChapterSet(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        private readonly bool playfieldBorder = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.PlayfieldBorder);

        protected virtual bool KiaiBoss => VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.KiaiBoss);

        private readonly string character = VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Character);

        public VitaruInputManager VitaruInputManager { get; private set; }

        public AspectLockedPlayfield Gamefield { get; private set; }

        private readonly Container judgementLayer;
        private readonly List<DrawableVitaruPlayer> playerList = new List<DrawableVitaruPlayer>();

        public Vector2 PlayerPosition => Player?.Position ?? chapterSet.PlayerStartingPosition;

        public DrawableVitaruPlayer Player { get; internal set; }

        public DrawableBoss DrawableBoss { get; private set; }

        public virtual bool Editor => false;

        public override float Margin => chapterSet.PlayfieldMargin;

        protected override Vector2 AspectRatio => chapterSet.PlayfieldAspectRatio;

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();

        private double startTime = double.MinValue;
        private double endTime = double.MaxValue;

        private readonly DebugStat<int> returnedJudgementCount;

        private const int enemyTeam = 0;
        private const int playerTeam = 1;

        protected OsuNetworkingHandler OsuNetworkingHandler { get; private set; }

        /// <summary>
        /// VitaruPlayfield.Current
        /// </summary>
        public float Current { get; private set; }

        public VitaruPlayfield(VitaruInputManager vitaruInput, OsuNetworkingHandler osuNetworkingHandler = null, MatchInfo match = null)
        {
            VitaruInputManager = vitaruInput;
            OsuNetworkingHandler = osuNetworkingHandler;

            //Reset crap
            CHARGED = false;
            ACCEL = false;
            ACCELMULTIPLIER = 1;
            HIDDEN = false;
            TRUEHIDDEN = false;
            FLASHLIGHT = false;

            BulletPiece.ExclusiveTestingHax = false;
            DrawableBullet.BoundryHacks = false;

            Size = chapterSet.PlayfieldSize;

            AddRange(new Drawable[]
            {
                judgementLayer = new Container
                {
                    Name = "Judgements",
                    RelativeSizeAxes = Axes.Both
                },
                Gamefield = new AspectLockedPlayfield
                {
                    Name = "Gamefield",
                    Margin = 1
                },
            });

            DebugToolkit.GeneralDebugItems.Add(new MemoryDebugStat());
            DebugToolkit.GeneralDebugItems.Add(new DebugStat<int>(HITOBJECT_COUNT) { Text = "Hitobject Count" });
            DebugToolkit.GeneralDebugItems.Add(new DebugStat<int>(DrawableCluster.CLUSTER_COUNT) { Text = "Cluster Count" });
            DebugToolkit.GeneralDebugItems.Add(new DebugStat<int>(DrawableBullet.BULLET_COUNT) { Text = "Bullet Count" });
            DebugToolkit.GeneralDebugItems.Add(returnedJudgementCount = new DebugStat<int>(new Bindable<int>()) { Text = "Returned Judge Count" });

            if (playfieldBorder)
                AddDrawable(new Container
                {
                    Name = "Border",
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = Color4.White,
                    BorderThickness = Editor ? 8 : 4,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                });

            if (!Editor)
            {
                VitaruPlayer vitaruPlayer = ChapterStore.GetPlayer(character);
                playerList.Add(Player = ChapterStore.GetDrawablePlayer(this, vitaruPlayer));
                
                //Multiplayer stuff
                if (match != null && osuNetworkingHandler != null)
                {
                    Player.SetNetworking(osuNetworkingHandler, osuNetworkingHandler.OsuUserInfo);

                    osuNetworkingHandler.OnPacketReceive += OnPacketReceive;

                    foreach (OsuUserInfo user in match.Users)
                        if (user.ID != osuNetworkingHandler.OsuUserInfo.ID)
                            foreach (Setting set in user.UserSettings)
                                switch (set.Name)
                                {
                                    //TODO: check if we LiveSpectator
                                    case "Character" when set is Setting<string> ch:
                                        VitaruPlayer v = ChapterStore.GetPlayer(ch.Value);

                                        DrawableVitaruPlayer dp;
                                        playerList.Add(dp = ChapterStore.GetDrawablePlayer(this, v));

                                        dp.SetNetworking(osuNetworkingHandler, user);
                                        dp.ControlType = ControlType.Net;
                                        break;
                                }
                }

                foreach (DrawableVitaruPlayer player in playerList)
                    Gamefield.Add(player);

                Player.Position = chapterSet.PlayerStartingPosition;
                Player.Team = playerTeam;

                VitaruInputManager.TouchControls.VitaruInputContainer = Player.VitaruInputContainer;
            }
            else
                Player = null;

            if (chapterSet is TouhosuChapterSet && KiaiBoss)
            {
                Gamefield.Add(DrawableBoss = new DrawableBoss(this, new Kokoro()));
                DrawableBoss.Team = enemyTeam;
            }

            DebugToolkit.GeneralDebugItems.Add(new DebugAction { Text = "Exclusive Testing Hax", Action = () =>
            {
                BulletPiece.ExclusiveTestingHax = !BulletPiece.ExclusiveTestingHax;
                osuNetworkingHandler?.SendToServer(new HaxPacket
                {
                    Hax = BulletPiece.ExclusiveTestingHax,
                    ID = OsuNetworkingHandler.OsuUserInfo.ID,
                });
            } });
        }

        protected virtual void OnPacketReceive(PacketInfo info)
        {
            switch (info.Packet)
            {
                case HaxPacket hax:
                    BulletPiece.ExclusiveTestingHax = hax.Hax;
                    break;
            }
        }

        [BackgroundDependencyLoader]
        private void load(Bindable<WorkingBeatmap> beatmap) => workingBeatmap.BindTo(beatmap);

        private readonly List<Cluster> unloadedClusters = new List<Cluster>();
        private readonly List<Cluster> loadedClusters = new List<Cluster>();

        private DrawableCluster add(Cluster p)
        {
            p.Hidden = HIDDEN;
            p.TrueHidden = TRUEHIDDEN;
            p.Flashlight = FLASHLIGHT;

            DrawableCluster drawable = chapterSet.GetDrawableCluster(p, this);

            drawable.Depth = (float)drawable.HitObject.StartTime;

            drawable.Editor = Editor;

            HITOBJECT_COUNT.Value++;
            drawable.OnDispose += () => { HITOBJECT_COUNT.Value--; };

            DrawableCluster.CLUSTER_COUNT.Value++;
            drawable.OnDispose += () => DrawableCluster.CLUSTER_COUNT.Value--;
            
            drawable.OnNewResult += onResult;

            drawable.OnDispose += () => drawable.OnNewResult -= onResult;

            base.Add(drawable);

            return drawable;
        }

        public override void Add(DrawableHitObject h)
        {
            if (!(h is DrawableCluster))
                throw new InvalidOperationException("Only DrawableClusters can be added to playfield!");

            Cluster c = (Cluster)h.HitObject;

            c.Team = enemyTeam;
            unloadedClusters.Add(c);

            foreach (Projectile projectile in c.GetProjectiles())
                c.AddNested(projectile);

            if (startTime == double.MinValue)
                startTime = h.HitObject.StartTime;

            endTime = c.EndTime;
            h.Dispose();
        }

        protected override void Update()
        {
            base.Update();

            Current = (float)Time.Current;

            if (CHARGED && Player is DrawableTouhosuPlayer t)
                t.Charge(999);

            if (ACCEL)
                applyToClock(workingBeatmap.Value.Track, (getAccelSpeed(Current) < 0.75f ? 0.75f : getAccelSpeed(Current)) * ACCELMULTIPLIER);

            for (int i = 0; i < unloadedClusters.Count; i++)
            {
                Cluster p = unloadedClusters[i];
                if (Current >= p.StartTime - p.TimePreempt && Current < p.EndTime + p.TimeUnPreempt)
                {
                    DrawableCluster d = add(p);

                    unloadedClusters.Remove(p);
                    loadedClusters.Add(p);

                    DrawableBoss?.DrawableClusters.Add(d);

                    d.OnDispose += () =>
                    {
                        loadedClusters.Remove(p);
                        unloadedClusters.Add(p);
                    };
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            VitaruInputManager = null;
            if (OsuNetworkingHandler != null) OsuNetworkingHandler.OnPacketReceive -= OnPacketReceive;
            OsuNetworkingHandler = null;
            base.Dispose(isDisposing);
        }

        private double getAccelSpeed(double value)
        {
            double scale = (1.5 - 0.75) / (endTime - startTime);
            return 0.75 + (value - startTime) * scale;
        }

        private void applyToClock(IAdjustableClock clock, double speed)
        {
            if (clock is IHasPitchAdjust pitchAdjust)
                pitchAdjust.PitchAdjust = speed;
        }

        private void onResult(DrawableHitObject judgedObject, JudgementResult result)
        {
            VitaruJudgementResult vitaruResult = (VitaruJudgementResult)result;

            if (!Editor)
            {
                OnResult?.Invoke(vitaruResult);

                if (!vitaruResult.VitaruJudgement.BonusScore)
                    returnedJudgementCount.Bindable.Value++;

                DrawableVitaruJudgement explosion = new DrawableVitaruJudgement(result, judgedObject)
                {
                    Alpha = 0.5f,
                    Origin = Anchor.Centre,
                    Position = judgedObject.Position
                };

                judgementLayer.Add(explosion);
            }
        }

        public Vector2 MousePos = Vector2.Zero;
        protected override bool OnMouseMove(MouseMoveEvent e)
        {
            MousePos = e.MousePosition * new Vector2(1.33f) - new Vector2(402, 106);
            return base.OnMouseMove(e);
        }

        private class MemoryDebugStat : DebugStat<double>
        {
            public MemoryDebugStat()
                : base(VitaruRuleset.MEMORY_LEAKED)
            {
                Text = "Memory lost";
            }

            protected override string GetText(double value)
            {
                if (value < 1000)
                    return $"{Text} = {Math.Round(value, 1).ToString()}B";
                if (value < 1000000)
                    return $"{Text} = {Math.Round(value / 1000, 1).ToString()}KB";
                if (value < 1000000000)
                    return $"{Text} = {Math.Round(value / 1000000, 1).ToString()}MB";
                else
                    return $"{Text} = {Math.Round(value / 1000000000, 1).ToString()}GB";
            }
        }
    }
}
