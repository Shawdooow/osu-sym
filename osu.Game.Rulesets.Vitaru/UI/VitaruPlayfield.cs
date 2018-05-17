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
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers;
using osu.Framework.Logging;
using osu.Framework.Graphics.Shapes;
using osu.Game.Screens.Edit.Screens.Compose.Layers;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruPlayfield : SymcolPlayfield
    {
        private static readonly Gamemodes gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        private readonly GraphicsPresets graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsPresets>(VitaruSetting.GraphicsPresets);

        private readonly string selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

        public readonly VitaruInputManager VitaruInputManager;

        public readonly AbstractionField GameField;

        public readonly MirrorField Mirrorfield;

        private readonly Container judgementLayer;
        private readonly List<DrawableVitaruPlayer> playerList = new List<DrawableVitaruPlayer>();

        //TODO: Make this not need to be static?
        public static List<VitaruClientInfo> LoadPlayerList = new List<VitaruClientInfo>();

        //TODO: Make this not need to be static
        public static DrawableVitaruPlayer Player;

        public Boss Boss;

        public virtual bool LoadPlayer => true;

        public static Vector2 BaseSize
        {
            get
            {
                if (gamemode == Gamemodes.Dodge)
                    return new Vector2(512, 384);
                else if (gamemode == Gamemodes.Gravaru)
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

            Bindable<int> abstraction = new Bindable<int>() { Value = 0 };

            if (graphics == GraphicsPresets.StandardV2)
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
                GameField = new AbstractionField(abstraction),
                judgementLayer = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
            });

            if (LoadPlayer)
            {
                VitaruNetworkingClientHandler vitaruNetworkingClientHandler = RulesetNetworkingClientHandler as VitaruNetworkingClientHandler;

                switch (selectedCharacter)
                {
                    case "Alex":
                        playerList.Add(Player = new DrawableVitaruPlayer(this, new Alex(), vitaruNetworkingClientHandler));
                        break;
                    case "ReimuHakurei":
                        playerList.Add(Player = new DrawableTouhosuPlayer(this, new Reimu(), vitaruNetworkingClientHandler));
                        break;
                    case "RyukoyHakurei":
                        playerList.Add(Player = new DrawableRyukoy(this, vitaruNetworkingClientHandler, abstraction));
                        break;
                    case "TomajiHakurei":
                        playerList.Add(Player = new DrawableTomaji(this, vitaruNetworkingClientHandler));
                        break;
                    case "SakuyaIzayoi":
                        playerList.Add(Player = new DrawableSakuya(this, vitaruNetworkingClientHandler));
                        break;
                }

                foreach (VitaruClientInfo client in LoadPlayerList)
                    if (client.PlayerInformation.PlayerID != Player.PlayerID)
                    {
                        Logger.Log("Loading a player recieved from internet!", LoggingTarget.Network, LogLevel.Verbose);

                        switch (client.PlayerInformation.Character)
                        {
                            case "Alex":
                                playerList.Add(Player = new DrawableVitaruPlayer(this, new Alex(), vitaruNetworkingClientHandler) { Puppet = true, PlayerID = client.PlayerInformation.PlayerID });
                                break;
                            case "ReimuHakurei":
                                playerList.Add(Player = new DrawableTouhosuPlayer(this, new Reimu(), vitaruNetworkingClientHandler) { Puppet = true, PlayerID = client.PlayerInformation.PlayerID });
                                break;
                            case "RyukoyHakurei":
                                playerList.Add(Player = new DrawableRyukoy(this, vitaruNetworkingClientHandler, abstraction) { Puppet = true, PlayerID = client.PlayerInformation.PlayerID });
                                break;
                            case "TomajiHakurei":
                                playerList.Add(Player = new DrawableTomaji(this, vitaruNetworkingClientHandler) { Puppet = true, PlayerID = client.PlayerInformation.PlayerID });
                                break;
                            case "SakuyaIzayoi":
                                playerList.Add(Player = new DrawableSakuya(this, vitaruNetworkingClientHandler) { Puppet = true, PlayerID = client.PlayerInformation.PlayerID });
                                break;
                        }
                    }

                foreach (DrawableVitaruPlayer player in playerList)
                    GameField.Add(player);

                if (gamemode == Gamemodes.Touhosu && graphics == GraphicsPresets.StandardV2 && VitaruAPIContainer.Shawdooow)
                    GameField.Add(Boss = new Boss(this));

                Player.Position = new Vector2(256, 700);
                if (gamemode == Gamemodes.Dodge || gamemode == Gamemodes.Gravaru)
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

            public Container Current = new Container { RelativeSizeAxes = Axes.Both, Name = "Current" };
            public Container QuarterAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.5f, Colour = OsuColour.FromHex("#ffe6d1"), Name = "QuarterAbstraction" };
            public Container HalfAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.25f, Colour = OsuColour.FromHex("#bff5ff"), Name = "HalfAbstraction" };
            public Container FullAbstraction = new Container { RelativeSizeAxes = Axes.Both, Alpha = 0.125f, Colour = OsuColour.FromHex("#d191ff"), Name = "FullAbstraction" };

            public AbstractionField(Bindable<int> abstraction)
            {
                AbstractionLevel = abstraction;

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
