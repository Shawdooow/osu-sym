using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Timing;
using osu.Game.Beatmaps;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.Bosses;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.Bosses.DrawableBosses;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Rulesets.Core.Rulesets;
using osuTK;
using osuTK.Graphics;

// ReSharper disable UnusedMember.Global
// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields
{
    public class VitaruPlayfield : SymcolPlayfield
    {
        public static bool ACCEL;
        //For Sakuya only pretty much
        public static double ACCELMULTIPLIER = 1;
        public static bool HIDDEN;
        public static bool TRUEHIDDEN;
        public static bool FLASHLIGHT;

        public static Action<JudgementResult> OnResult;

        public bool Cheated { get; internal set; }

        private readonly VitaruGamemode gamemode = ChapterStore.GetGamemode(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        private readonly bool playfieldBorder = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.PlayfieldBorder);

        protected virtual bool KiaiBoss => VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.KiaiBoss);

        private readonly string character = VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Character);

        public readonly VitaruInputManager VitaruInputManager;

        public readonly AspectLockedPlayfield Gamefield;

        private readonly Container judgementLayer;
        private readonly List<DrawableVitaruPlayer> playerList = new List<DrawableVitaruPlayer>();

        public Vector2 PlayerPosition => Player?.Position ?? gamemode.PlayerStartingPosition;

        public DrawableVitaruPlayer Player;

        public DrawableBoss DrawableBoss;

        public virtual bool Editor => false;

        public override float Margin => gamemode.PlayfieldMargin;

        protected override Vector2 AspectRatio => gamemode.PlayfieldAspectRatio;

        private readonly Bindable<WorkingBeatmap> workingBeatmap = new Bindable<WorkingBeatmap>();

        private double startTime = double.MinValue;
        private double endTime = double.MaxValue;

        private readonly DebugStat<int> returnedJudgementCount;
        private readonly DebugStat<int> drawableHitobjectCount;
        private readonly DebugStat<int> drawablePatternCount;

        private const int enemyTeam = 0;
        private const int playerTeam = 1;

        /// <summary>
        /// VitaruPlayfield.Current
        /// </summary>
        public float Current { get; private set; }

        public VitaruPlayfield(VitaruInputManager vitaruInput, OsuNetworkingHandler osuNetworkingHandler = null)
        {
            //VitaruRuleset.InitThread();
            VitaruInputManager = vitaruInput;

            ACCEL = false;
            ACCELMULTIPLIER = 1;
            HIDDEN = false;
            TRUEHIDDEN = false;
            FLASHLIGHT = false;

            BulletPiece.ExclusiveTestingHax = false;
            DrawableBullet.BoundryHacks = false;

            Size = gamemode.PlayfieldSize;

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
                }
            });

            DebugToolkit.GeneralDebugItems.Add(returnedJudgementCount = new DebugStat<int>(new Bindable<int>()) { Text = "Returned Judgement Count" });
            DebugToolkit.GeneralDebugItems.Add(drawableHitobjectCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Hitobject Count" });
            DebugToolkit.GeneralDebugItems.Add(drawablePatternCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Pattern Count" });
            DebugToolkit.GeneralDebugItems.Add(new DebugStat<int>(DrawableBullet.BULLET_COUNT) { Text = "Drawable Bullet Count" });
            DrawableBullet.BULLET_COUNT.Value = 0;

            if (playfieldBorder)
                AddDrawable(new Container
                {
                    Name = "Border",
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = Color4.White,
                    BorderThickness = Editor ? 6 : 3,
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
                
                if (gamemode is TouhosuGamemode)
                    playerList.Add(Player = ChapterStore.GetDrawablePlayer(this, (TouhosuPlayer)vitaruPlayer));
                else
                    playerList.Add(Player = ChapterStore.GetDrawablePlayer(this, vitaruPlayer));

                DebugToolkit.GeneralDebugItems.Add(new DebugAction
                {
                    Text = "Add New Player",
                    Action = () =>
                    {
                        if (gamemode is TouhosuGamemode)
                            AddDrawable(ChapterStore.GetDrawablePlayer(this, (TouhosuPlayer)vitaruPlayer));
                        else
                            AddDrawable(ChapterStore.GetDrawablePlayer(this, vitaruPlayer));
                    }
                });

                foreach (DrawableVitaruPlayer player in playerList)
                    Gamefield.Add(player);

                Player.Position = gamemode.PlayerStartingPosition;
                Player.Team = playerTeam;

                VitaruInputManager.TouchControls.VitaruInputContainer = Player.VitaruInputContainer;
            }
            else
                Player = null;

            if (gamemode is TouhosuGamemode && KiaiBoss)
            {
                Gamefield.Add(DrawableBoss = new DrawableBoss(this, new Kokoro()));
                DrawableBoss.Team = enemyTeam;
            }

            DebugToolkit.GeneralDebugItems.Add(new DebugAction { Text = "Exclusive Testing Hax", Action = () => { BulletPiece.ExclusiveTestingHax = !BulletPiece.ExclusiveTestingHax; } });
        }

        [BackgroundDependencyLoader]
        private void load(Bindable<WorkingBeatmap> beatmap)
        {
            workingBeatmap.BindTo(beatmap);
        }

        private readonly List<Cluster> unloadedClusters = new List<Cluster>();
        private readonly List<Cluster> loadedClusters = new List<Cluster>();

        private DrawableCluster add(Cluster p)
        {
            p.Hidden = HIDDEN;
            p.TrueHidden = TRUEHIDDEN;
            p.Flashlight = FLASHLIGHT;

            DrawableCluster drawable = gamemode.GetDrawableCluster(p, this);

            drawable.Depth = (float)drawable.HitObject.StartTime;

            drawable.Editor = Editor;

            drawableHitobjectCount.Bindable.Value++;
            drawable.OnDispose += isDisposing => { drawableHitobjectCount.Bindable.Value--; };

            drawablePatternCount.Bindable.Value++;
            drawable.OnDispose += isDisposing => { drawablePatternCount.Bindable.Value--; };
            
            drawable.OnNewResult += onResult;

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
        }

        protected override void Update()
        {
            base.Update();

            Current = (float)Time.Current;

            if (ACCEL)
                applyToClock(workingBeatmap.Value.Track, (getSpeed(Current) < 0.75f ? 0.75f : getSpeed(Current)) * ACCELMULTIPLIER);

            restart:
            foreach (Cluster p in unloadedClusters)
                if (Current >= p.StartTime - p.TimePreempt && Current < p.EndTime + p.TimeUnPreempt)
                {
                    DrawableCluster d = add(p);
                    unloadedClusters.Remove(p);
                    loadedClusters.Add(p);
                    DrawableBoss?.DrawableClusters.Add(d);
                    d.OnDelete += () => clean(d);

                    goto restart;

                    void clean(DrawableCluster c)
                    {
                        loadedClusters.Remove(p);
                        unloadedClusters.Add(p);
                        c.OnDelete -= () => clean(d);
                    }
                }
        }

        private double getSpeed(double value)
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
            var vitaruResult = (VitaruJudgementResult)result;

            if (!Editor)
            {
                OnResult.Invoke(vitaruResult);
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
    }
}
