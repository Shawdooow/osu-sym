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
using Symcol.Rulesets.Core;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Framework.Logging;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players;

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

        /*
        private readonly PlayableCharacters playerOne = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerOne);
        private readonly PlayableCharacters playerTwo = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerTwo);
        private readonly PlayableCharacters playerThree = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerThree);
        private readonly PlayableCharacters playerFour = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerFour);
        private readonly PlayableCharacters playerFive = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerFive);
        private readonly PlayableCharacters playerSix = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerSix);
        private readonly PlayableCharacters playerSeven = VitaruSettings.VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerSeven);
        */

        public readonly Container BulletField;
        public readonly Container SpellField;
        public readonly Container CharacterField;

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

        public VitaruPlayfield() : base(BaseSize)
        {
            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            if (currentGameMode != VitaruGamemode.Dodge && currentGameMode != VitaruGamemode.Gravaru && multiplayer && enemyPlayerCount > 0)
            {
                Position = new Vector2(20, 0);
                Anchor = Anchor.Centre;
                Origin = Anchor.CentreLeft;
                Add(Mirrorfield = new MirrorField(this));
            }

            AddRange(new Drawable[]
            {
                CharacterField = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                BulletField = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                SpellField = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
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
                        playerList.Add(Player = new Ryukoy(this));
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
                /*
                if (multiplayer && currentGameMode != VitaruGamemode.Dodge && currentGameMode != VitaruGamemode.Gravaru)
                {
                    switch (friendlyPlayerCount)
                    {
                        case 0:
                            break;
                        case 1:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2), Auto = true, Bot = true });
                            break;
                        case 2:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 700), Auto = true, Bot = true });
                            break;
                        case 3:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 700), Auto = true, Bot = true });
                            break;
                        case 4:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 700), Auto = true, Bot = true });
                            break;
                        case 5:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 700), Auto = true, Bot = true });
                            break;
                        case 6:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(150, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(250, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 250, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 150, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerSix) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 700), Auto = true, Bot = true });
                            break;
                        case 7:
                            playerList.Add(new VitaruPlayer(this, playerOne) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(0, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerTwo) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(125, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerThree) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFour) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512f / 2, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerFive) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 200, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerSix) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512 - 125, 700), Auto = true, Bot = true });
                            playerList.Add(new VitaruPlayer(this, playerSeven) { Anchor = Anchor.Centre, Origin = Anchor.Centre, Position = new Vector2(512, 700), Auto = true, Bot = true });
                            break;
                    }
                }
                */

                foreach (VitaruPlayer player in playerList)
                    CharacterField.Add(player);

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
                    Position = new Vector2(Player.Position.X, Player.Position.Y + 50)
                };

                judgementLayer.Add(explosion);
            }
        }

        protected virtual CursorContainer CreateCursor() => new GameplayCursor();
    }
}
