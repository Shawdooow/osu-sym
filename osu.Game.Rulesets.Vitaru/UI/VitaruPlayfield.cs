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
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players;
using Symcol.Rulesets.Core.Rulesets;
using osu.Framework.Graphics.Effects;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruPlayfield : SymcolPlayfield
    {
        private static readonly Bindable<VitaruGamemode> currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);
        private readonly SelectableCharacters currentCharacter = VitaruSettings.VitaruConfigManager.GetBindable<SelectableCharacters>(VitaruSetting.Characters);
        private readonly bool multiplayer = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ShittyMultiplayer);
        private bool friendlyPlayerOverride = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.FriendlyPlayerOverride);
        private readonly Bindable<int> friendlyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.FriendlyPlayerCount);
        private readonly Bindable<int> enemyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.EnemyPlayerCount);

        public readonly VitaruInputManager VitaruInputManager;

        public readonly AbstractionField GameField;

        public readonly MirrorField Mirrorfield;

        private readonly Container judgementLayer;
        private readonly List<VitaruPlayer> playerList = new List<VitaruPlayer>();

        public static List<VitaruClientInfo> LoadPlayerList = new List<VitaruClientInfo>();

        public static VitaruPlayer Player;

        public virtual bool LoadPlayer => true;

        public static Vector2 BaseSize
        {
            get
            {
                if (currentGameMode == VitaruGamemode.Dodge)
                    return new Vector2(512, 384);
                else if (currentGameMode == VitaruGamemode.Gravaru)
                    return new Vector2(384 * 2, 384);
                else
                    return new Vector2(512, 820);
            }
        }

        public VitaruPlayfield(VitaruInputManager vitaruInput) : base(BaseSize)
        {
            VitaruInputManager = vitaruInput;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            if (currentGameMode != VitaruGamemode.Dodge && currentGameMode != VitaruGamemode.Gravaru && multiplayer && enemyPlayerCount > 0)
            {
                Position = new Vector2(20, 0);
                Anchor = Anchor.Centre;
                Origin = Anchor.CentreLeft;
                Add(Mirrorfield = new MirrorField(this, vitaruInput));
            }

            Bindable<int> abstraction = new Bindable<int>() { Value = 0 };

            AddRange(new Drawable[]
            {
                GameField = new AbstractionField(abstraction),
                judgementLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
            });

            if (LoadPlayer)
            {
                VitaruNetworkingClientHandler vitaruNetworkingClientHandler = RulesetNetworkingClientHandler as VitaruNetworkingClientHandler;

                switch (currentCharacter)
                {
                    case SelectableCharacters.RyukoyHakurei:
                        playerList.Add(Player = new Ryukoy(this, abstraction));
                        break;
                    case SelectableCharacters.TomajiHakurei:
                        playerList.Add(Player = new Tomaji(this));
                        break;
                    case SelectableCharacters.SakuyaIzayoi:
                        playerList.Add(Player = new Sakuya(this));
                        break;
                }

                /*
                if (vitaruNetworkingClientHandler != null)
                    playerList.Add(Player = new Player(this, currentCharacter) { VitaruNetworkingClientHandler = vitaruNetworkingClientHandler, PlayerID = vitaruNetworkingClientHandler.VitaruClientInfo.IP + vitaruNetworkingClientHandler.VitaruClientInfo.UserID });
                else
                    playerList.Add(Player = new Player(this, currentCharacter));

                foreach (VitaruClientInfo client in LoadPlayerList)
                    if (client.PlayerInformation.PlayerID != Player.PlayerID)
                    {
                        Logger.Log("Loading a player recieved from internet!", LoggingTarget.Network, LogLevel.Verbose);
                        playerList.Add(new Player(this, client.PlayerInformation.Character)
                        {
                            Puppet = true,
                            PlayerID = client.PlayerInformation.PlayerID,
                            VitaruNetworkingClientHandler = vitaruNetworkingClientHandler
                        });
                    }*/

                foreach (VitaruPlayer player in playerList)
                    GameField.Add(player);

                Player.Position = new Vector2(256, 700);
                if (currentGameMode == VitaruGamemode.Dodge || currentGameMode == VitaruGamemode.Gravaru)
                    Player.Position = BaseSize / 2;
            }
            else
                Player = null;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            var cursor = CreateCursor();
            if (cursor != null)
                AddInternal(cursor);
        }

        public override void Add(DrawableHitObject h)
        {
            h.Depth = (float)h.HitObject.StartTime;

            h.OnJudgement += onJudgement;

            base.Add(h);
        }

        private void onJudgement(DrawableHitObject judgedObject, Judgement judgement)
        {
            var vitaruJudgement = (VitaruJudgement)judgement;

            if (Player != null)
            {
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

        public class AbstractionField : Container
        {
            public readonly Bindable<int> AbstractionLevel;

            public Container Current = new Container { RelativeSizeAxes = Axes.Both };
            public Container QuarterOut = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.5f };
            public Container HalfOut = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.25f };
            public Container FullAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.125f };

            public AbstractionField(Bindable<int> abstraction)
            {
                AbstractionLevel = abstraction;

                RelativeSizeAxes = Axes.Both;

                Children = new Drawable[]
                {
                    FullAbstraction.WithEffect(new GlowEffect
                    {
                        Strength = 8f,
                        BlurSigma = new Vector2(16)
                    }),
                    HalfOut.WithEffect(new GlowEffect
                    {
                        Strength = 4f,
                        BlurSigma = new Vector2(8)
                    }),
                    QuarterOut.WithEffect(new GlowEffect
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

                    foreach (Drawable draw in QuarterOut)
                        q.Add(draw);

                    foreach (Drawable draw in HalfOut)
                        h.Add(draw);

                    foreach (Drawable draw in FullAbstraction)
                        f.Add(draw);

                    foreach (Drawable draw in q)
                    {
                        QuarterOut.Remove(draw);
                        Current.Add(draw);
                    }

                    foreach (Drawable draw in h)
                    {
                        HalfOut.Remove(draw);
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
                            if (draw is VitaruPlayer player && player.Abstraction < value)
                                quarter.Add(player);
                            if (draw is Enemy enemy && enemy.Abstraction < value)
                                quarter.Add(enemy);
                        }

                        foreach (Drawable draw in quarter)
                        {
                            Current.Remove(draw);
                            QuarterOut.Add(draw);
                        }
                    }
                    if (value >= 2)
                    {
                        List<Drawable> half = new List<Drawable>();

                        foreach (Drawable draw in QuarterOut)
                        {
                            if (draw is DrawableBullet bullet && bullet.Bullet.Abstraction < value - 1)
                                half.Add(bullet);
                            if (draw is VitaruPlayer player && player.Abstraction < value - 1)
                                half.Add(player);
                            if (draw is Enemy enemy && enemy.Abstraction < value - 1)
                                half.Add(enemy);
                        }

                        foreach (Drawable draw in half)
                        {
                            QuarterOut.Remove(draw);
                            HalfOut.Add(draw);
                        }
                    }
                    if (value >= 3)
                    {
                        List<Drawable> full = new List<Drawable>();

                        foreach (Drawable draw in HalfOut)
                        {
                            if (draw is DrawableBullet bullet && bullet.Bullet.Abstraction < value - 2)
                                full.Add(bullet);

                            if (draw is VitaruPlayer player && player.Abstraction < value - 2)
                                full.Add(player);

                            if (draw is Enemy enemy && enemy.Abstraction < value - 2)
                                full.Add(enemy);
                        }

                        foreach (Drawable draw in full)
                        {
                            HalfOut.Remove(draw);
                            FullAbstraction.Add(draw);
                        }
                    }
                };
                AbstractionLevel.TriggerChange();
            }

            public void Add (Drawable drawable)
            {
                Current.Add(drawable);
                AbstractionLevel.TriggerChange();
            }

            public void Remove (Drawable drawable)
            {
                foreach (Drawable draw in Current)
                    if (draw == drawable)
                    {
                        Current.Remove(drawable);
                        return;
                    }
                foreach (Drawable draw in QuarterOut)
                    if (draw == drawable)
                    {
                        QuarterOut.Remove(drawable);
                        return;
                    }
                foreach (Drawable draw in HalfOut)
                    if (draw == drawable)
                    {
                        HalfOut.Remove(drawable);
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
