using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Objects.Drawables;
using osu.Game.Rulesets.Judgements;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Framework.Graphics.Cursor;
using osu.Game.Rulesets.Vitaru.UI.Cursor;
using osu.Framework.Configuration;
using System.Collections.Generic;
using osu.Game.Rulesets.Vitaru.Multi;
using Symcol.Rulesets.Core.Rulesets;
using osu.Framework.Graphics.Effects;
using osu.Game.Rulesets.Vitaru.Characters;
using osu.Game.Graphics;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.DrawableTouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers;
using osu.Framework.Logging;
using osu.Framework.Graphics.Shapes;
using OpenTK.Graphics;
using osu.Game.Overlays.Notifications;
using osu.Framework.Allocation;
using osu.Game.Overlays;
using osu.Game.Rulesets.Vitaru.Debug;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruPlayfield : SymcolPlayfield
    {
        private static readonly Bindable<Gamemodes> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        private readonly Bindable<bool> goodFps = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.GoodFPS);

        private readonly bool playfieldBorder = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.PlayfieldBorder);

        private readonly bool kiaiBoss = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.KiaiBoss);

        private readonly string character = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

        public readonly VitaruInputManager VitaruInputManager;

        public readonly AbstractionField GameField;

        public readonly MirrorField Mirrorfield;

        public static Action<Judgement> OnJudgement;
        //public Action<Judgement> RemoveJudgement;

        private readonly Container judgementLayer;
        private readonly List<DrawableVitaruPlayer> playerList = new List<DrawableVitaruPlayer>();

        //TODO: Make this not need to be static?
        public static List<VitaruClientInfo> LoadPlayerList = new List<VitaruClientInfo>();

        //TODO: Make this not need to be static
        public DrawableVitaruPlayer Player;

        public Boss Boss;

        public virtual bool Editor => false;

        public static Vector2 BaseSize
        {
            get
            {
                if (gamemode == Gamemodes.Touhosu)
                    return new Vector2(512 * 2, 820);
                else if (gamemode == Gamemodes.Dodge)
                    return new Vector2(512, 384);
                else if (gamemode == Gamemodes.Gravaru)
                    return new Vector2(384 * 2, 384);
                else
                    return new Vector2(512, 820);
            }
        }

        private readonly DebugStat<int> returnedJudgementCount;
        private readonly DebugStat<int> drawableHitobjectCount;
        private readonly DebugStat<int> drawablePatternCount;
        private readonly DebugStat<int> ranked;

        public VitaruPlayfield(VitaruInputManager vitaruInput) : base(BaseSize)
        {
            VitaruInputManager = vitaruInput;

            DrawableBullet.BoundryHacks = false;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Bindable<int> abstraction = new Bindable<int>() { Value = 0 };

            DebugToolkit.GeneralDebugItems.Add(returnedJudgementCount = new DebugStat<int>(new Bindable<int>()) { Text = "Returned Judgement Count" });
            DebugToolkit.GeneralDebugItems.Add(drawableHitobjectCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Hitobject Count" });
            DebugToolkit.GeneralDebugItems.Add(drawablePatternCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Pattern Count" });
            
            if (playfieldBorder)
                Add(new Container
                {
                    Name = "Border",
                    RelativeSizeAxes = Axes.Both,
                    Masking = true,
                    BorderColour = Color4.White,
                    BorderThickness = 3,
                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0,
                        AlwaysPresent = true
                    }
                });

            AddRange(new Drawable[]
            {
                GameField = new AbstractionField(abstraction, drawableHitobjectCount),
                judgementLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                }
            });

            if (!Editor)
            {
                VitaruNetworkingClientHandler vitaruNetworkingClientHandler = RulesetNetworkingClientHandler as VitaruNetworkingClientHandler;

                if (gamemode == Gamemodes.Touhosu)
                    playerList.Add(Player = DrawableTouhosuPlayer.GetDrawableTouhosuPlayer(this, character, vitaruNetworkingClientHandler, abstraction));
                else
                    playerList.Add(Player = DrawableVitaruPlayer.GetDrawableVitaruPlayer(this, character, vitaruNetworkingClientHandler));

                DebugToolkit.GeneralDebugItems.Add(new DebugAction()
                {
                    Text = "Add New Player",
                    Action = () =>
                    {
                        if (gamemode == Gamemodes.Touhosu)
                            GameField.Add(DrawableTouhosuPlayer.GetDrawableTouhosuPlayer(this, character, vitaruNetworkingClientHandler, abstraction));
                        else
                            GameField.Add(DrawableVitaruPlayer.GetDrawableVitaruPlayer(this, character, vitaruNetworkingClientHandler));
                    }
                });

                foreach (VitaruClientInfo client in LoadPlayerList)
                    if (client.PlayerInformation.PlayerID != Player.PlayerID)
                    {
                        Logger.Log("Loading a player recieved from internet!", LoggingTarget.Network, LogLevel.Verbose);

                        if (gamemode == Gamemodes.Touhosu)
                            playerList.Add(Player = DrawableTouhosuPlayer.GetDrawableTouhosuPlayer(this, character, vitaruNetworkingClientHandler, abstraction));
                        else
                            playerList.Add(Player = DrawableVitaruPlayer.GetDrawableVitaruPlayer(this, character, vitaruNetworkingClientHandler));
                    }

                foreach (DrawableVitaruPlayer player in playerList)
                    GameField.Add(player);

                if (gamemode == Gamemodes.Touhosu && kiaiBoss)
                    GameField.Add(Boss = new Boss(this));

                Player.Position = new Vector2(256, 700);
                if (gamemode == Gamemodes.Dodge || gamemode == Gamemodes.Gravaru)
                    Player.Position = BaseSize / 2;
                else if (gamemode == Gamemodes.Touhosu)
                    Player.Position = new Vector2(512, 700);
            }
            else
                Player = null;

            DebugToolkit.GeneralDebugItems.Add(new DebugAction() { Text = "Exclusive Testing Hax", Action = () => { BulletPiece.ExclusiveTestingHax = !BulletPiece.ExclusiveTestingHax; } });
        }

        [BackgroundDependencyLoader(true)]
        private void load(NotificationOverlay notificationOverlay)
        {
            if (VitaruSettings.VitaruConfigManager.Get<bool>(VitaruSetting.AnnoyPlayer))
                notificationOverlay?.Post(new SimpleNotification
                {
                    Text = "Be sure to check out vitaru settings for the ingame wiki for wiki things! (click me to never see me again)",
                    Activated = () =>
                    {
                        VitaruSettings.VitaruConfigManager.Set<bool>(VitaruSetting.AnnoyPlayer, false);
                        return true;
                    }
                });
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var cursor = CreateCursor();
            if (cursor != null)
                AddInternal(cursor);
        }

        private readonly List<Pattern> patterns = new List<Pattern>();

        private void add(Pattern p, DrawablePattern drawable = null)
        {
            if (drawable == null)
                drawable = new DrawablePattern(p, this);

            drawable.Depth = (float)drawable.HitObject.StartTime;

            drawable.Editor = Editor || !goodFps;

            drawableHitobjectCount.Bindable.Value++;
            drawable.OnDispose += isDisposing => { drawableHitobjectCount.Bindable.Value--; };

            drawablePatternCount.Bindable.Value++;
            drawable.OnDispose += isDisposing => { drawablePatternCount.Bindable.Value--; };

            drawable.OnJudgement += onJudgement;

            base.Add(drawable);
        }

        public override void Add(DrawableHitObject h)
        {
            DrawablePattern drawable = h as DrawablePattern;

            if (Editor || !goodFps)
                add(null, drawable);
            else
                patterns.Add((Pattern)drawable.HitObject);
        }

        protected override void Update()
        {
            base.Update();

            foreach (Pattern p in patterns)
            {
                if (Time.Current >= p.StartTime - p.TimePreempt * 2)
                {
                    add(p);
                    patterns.Remove(p);
                    break;
                }
            }
        }

        private void onJudgement(DrawableHitObject judgedObject, Judgement judgement)
        {
            var vitaruJudgement = (VitaruJudgement)judgement;

            OnJudgement?.Invoke(vitaruJudgement);

            if (Player != null)
            {
                returnedJudgementCount.Bindable.Value++;

                DrawableVitaruJudgement explosion = new DrawableVitaruJudgement(judgement, judgedObject)
                {
                    Alpha = 0.5f,
                    Origin = Anchor.Centre,
                    Position = judgedObject.Position
                };

                judgementLayer.Add(explosion);
            }
        }

        protected virtual CursorContainer CreateCursor() => new GameplayCursor();

        protected override void Dispose(bool isDisposing)
        {
            BulletPiece.ExclusiveTestingHax = false;
            OnJudgement = null;
            //RemoveJudgement = null;
            base.Dispose(isDisposing);
        }

        public class AbstractionField : Container
        {
            public readonly Bindable<int> AbstractionLevel;

            private readonly DebugStat<int> drawableHitobjectCount;
            private readonly DebugStat<int> drawableBulletCount;
            private readonly DebugStat<int> drawableLaserCount;
            private readonly DebugStat<int> enemyCount;
            private readonly DebugStat<int> bossCount;
            private readonly DebugStat<int> playerCount;

            public Container Current = new Container { RelativeSizeAxes = Axes.Both, Name = "Current" };
            public Container QuarterAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.5f, Colour = OsuColour.FromHex("#ffe6d1"), Name = "QuarterAbstraction" };
            public Container HalfAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.25f, Colour = OsuColour.FromHex("#bff5ff"), Name = "HalfAbstraction" };
            public Container FullAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.125f, Colour = OsuColour.FromHex("#d191ff"), Name = "FullAbstraction" };

            public AbstractionField(Bindable<int> abstraction, DebugStat<int> drawableHitobjectCount)
            {
                AbstractionLevel = abstraction;

                this.drawableHitobjectCount = drawableHitobjectCount;

                DebugToolkit.GeneralDebugItems.Add(drawableBulletCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Bullet Count" });
                DebugToolkit.GeneralDebugItems.Add(drawableLaserCount = new DebugStat<int>(new Bindable<int>()) { Text = "Drawable Laser Count" });
                DebugToolkit.GeneralDebugItems.Add(enemyCount = new DebugStat<int>(new Bindable<int>()) { Text = "Enemy Count" });
                DebugToolkit.GeneralDebugItems.Add(bossCount = new DebugStat<int>(new Bindable<int>()) { Text = "Boss Count" });
                DebugToolkit.GeneralDebugItems.Add(playerCount = new DebugStat<int>(new Bindable<int>()) { Text = "Player Count" });

                RelativeSizeAxes = Axes.Both;

                Name = "AbstractionField";

                Children = new Drawable[]
                {
                    FullAbstraction.WithEffect(new GlowEffect
                    {
                        Strength = 8f,
                        BlurSigma = new Vector2(16)
                    }),
                    HalfAbstraction.WithEffect(new GlowEffect
                    {
                        Strength = 4f,
                        BlurSigma = new Vector2(8)
                    }),
                    QuarterAbstraction.WithEffect(new GlowEffect
                    {
                        Strength = 2f,
                        BlurSigma = new Vector2(4)
                    }),
                    Current
                };

                AbstractionLevel.ValueChanged += (value) =>
                {
                    List<Drawable> q = new List<Drawable>();
                    List<Drawable> h = new List<Drawable>();
                    List<Drawable> f = new List<Drawable>();

                    foreach (Drawable draw in QuarterAbstraction)
                        q.Add(draw);

                    foreach (Drawable draw in HalfAbstraction)
                        h.Add(draw);

                    foreach (Drawable draw in FullAbstraction)
                        f.Add(draw);

                    foreach (Drawable draw in q)
                    {
                        QuarterAbstraction.Remove(draw);
                        Current.Add(draw);
                    }

                    foreach (Drawable draw in h)
                    {
                        HalfAbstraction.Remove(draw);
                        Current.Add(draw);
                    }

                    foreach (Drawable draw in f)
                    {
                        FullAbstraction.Remove(draw);
                        Current.Add(draw);
                    }

                    if (value >= 1)
                    {
                        List<Drawable> quarter = new List<Drawable>();

                        foreach (Drawable draw in Current)
                        {
                            if (draw is DrawableBullet bullet && bullet.Bullet.Abstraction < value)
                                quarter.Add(bullet);
                            if (draw is DrawableVitaruPlayer player && player.Abstraction < value)
                                quarter.Add(player);
                            if (draw is Enemy enemy && enemy.Abstraction < value)
                                quarter.Add(enemy);
                        }

                        foreach (Drawable draw in quarter)
                        {
                            Current.Remove(draw);
                            QuarterAbstraction.Add(draw);
                        }
                    }
                    if (value >= 2)
                    {
                        List<Drawable> half = new List<Drawable>();

                        foreach (Drawable draw in QuarterAbstraction)
                        {
                            if (draw is DrawableBullet bullet && bullet.Bullet.Abstraction < value - 1)
                                half.Add(bullet);
                            if (draw is DrawableVitaruPlayer player && player.Abstraction < value - 1)
                                half.Add(player);
                            if (draw is Enemy enemy && enemy.Abstraction < value - 1)
                                half.Add(enemy);
                        }

                        foreach (Drawable draw in half)
                        {
                            QuarterAbstraction.Remove(draw);
                            HalfAbstraction.Add(draw);
                        }
                    }
                    if (value >= 3)
                    {
                        List<Drawable> full = new List<Drawable>();

                        foreach (Drawable draw in HalfAbstraction)
                        {
                            if (draw is DrawableBullet bullet && bullet.Bullet.Abstraction < value - 2)
                                full.Add(bullet);

                            if (draw is DrawableVitaruPlayer player && player.Abstraction < value - 2)
                                full.Add(player);

                            if (draw is Enemy enemy && enemy.Abstraction < value - 2)
                                full.Add(enemy);
                        }

                        foreach (Drawable draw in full)
                        {
                            HalfAbstraction.Remove(draw);
                            FullAbstraction.Add(draw);
                        }
                    }
                };
                AbstractionLevel.TriggerChange();
            }

            public new void Add (Drawable drawable)
            {
                if (drawable is DrawableBullet bt)
                {
                    drawableHitobjectCount.Bindable.Value++;
                    bt.OnDispose += (isDisposing) => { drawableHitobjectCount.Bindable.Value--; };

                    drawableBulletCount.Bindable.Value++;
                    bt.OnDispose += (isDisposing) => { drawableBulletCount.Bindable.Value--; };
                }
                if (drawable is DrawableLaser l)
                {
                    drawableHitobjectCount.Bindable.Value++;
                    l.OnDispose += (isDisposing) => { drawableHitobjectCount.Bindable.Value--; };

                    drawableLaserCount.Bindable.Value++;
                    l.OnDispose += (isDisposing) => { drawableLaserCount.Bindable.Value--; };
                }
                else if (drawable is Enemy e)
                {
                    enemyCount.Bindable.Value++;
                    e.OnDispose += (isDisposing) => { enemyCount.Bindable.Value--; };
                }
                else if (drawable is Boss bs)
                {
                    bossCount.Bindable.Value++;
                    bs.OnDispose += (isDisposing) => { bossCount.Bindable.Value--; };
                }
                else if (drawable is DrawableVitaruPlayer p)
                {
                    playerCount.Bindable.Value++;
                    p.OnDispose += (isDisposing) => { playerCount.Bindable.Value--; };
                }

                Current.Add(drawable);
                AbstractionLevel.TriggerChange();
            }

            public new void Remove (Drawable drawable)
            {
                foreach (Drawable draw in Current)
                    if (draw == drawable)
                    {
                        Current.Remove(drawable);
                        return;
                    }
                foreach (Drawable draw in QuarterAbstraction)
                    if (draw == drawable)
                    {
                        QuarterAbstraction.Remove(drawable);
                        return;
                    }
                foreach (Drawable draw in HalfAbstraction)
                    if (draw == drawable)
                    {
                        HalfAbstraction.Remove(drawable);
                        return;
                    }
                foreach (Drawable draw in FullAbstraction)
                    if (draw == drawable)
                    {
                        FullAbstraction.Remove(drawable);
                        return;
                    }
            }
        }
    }
}
