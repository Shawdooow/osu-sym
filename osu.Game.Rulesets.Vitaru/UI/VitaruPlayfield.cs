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

namespace osu.Game.Rulesets.Vitaru.UI
{
    public class VitaruPlayfield : SymcolPlayfield
    {
        private static readonly Bindable<VitaruGamemode> currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);
        private readonly Characters currentCharacter = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.Characters);
        private readonly bool multiplayer = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.ShittyMultiplayer);
        private bool friendlyPlayerOverride = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.FriendlyPlayerOverride);
        private readonly Bindable<int> friendlyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.FriendlyPlayerCount);
        private readonly Bindable<int> enemyPlayerCount = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.EnemyPlayerCount);

        private readonly Characters playerOne = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerOne);
        private readonly Characters playerTwo = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerTwo);
        private readonly Characters playerThree = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerThree);
        private readonly Characters playerFour = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerFour);
        private readonly Characters playerFive = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerFive);
        private readonly Characters playerSix = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerSix);
        private readonly Characters playerSeven = VitaruSettings.VitaruConfigManager.GetBindable<Characters>(VitaruSetting.PlayerSeven);

        public readonly Container BulletField;
        public readonly Container SpellField;
        public readonly Container CharacterField;

        public readonly MirrorField Mirrorfield;

        private readonly Container judgementLayer;
        private readonly List<VitaruPlayer> playerList = new List<VitaruPlayer>();

        public static List<VitaruClientInfo> LoadPlayerList = new List<VitaruClientInfo>();

        public static VitaruPlayer VitaruPlayer;

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

                if (vitaruNetworkingClientHandler != null)
                    playerList.Add(VitaruPlayer = new VitaruPlayer(this, currentCharacter) { VitaruNetworkingClientHandler = vitaruNetworkingClientHandler, PlayerID = vitaruNetworkingClientHandler.VitaruClientInfo.IP + vitaruNetworkingClientHandler.VitaruClientInfo.UserID });
                else
                    playerList.Add(VitaruPlayer = new VitaruPlayer(this, currentCharacter));

                foreach (VitaruClientInfo client in LoadPlayerList)
                    if (client.PlayerInformation.PlayerID != VitaruPlayer.PlayerID)
                    {
                        Logger.Log("Loading a player recieved from internet!", LoggingTarget.Network, LogLevel.Verbose);
                        playerList.Add(new VitaruPlayer(this, client.PlayerInformation.Character)
                        {
                            Puppet = true,
                            PlayerID = client.PlayerInformation.PlayerID,
                            VitaruNetworkingClientHandler = vitaruNetworkingClientHandler
                        });
                    }

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

                foreach (VitaruPlayer player in playerList)
                    CharacterField.Add(player);

                VitaruPlayer.Position = new Vector2(256, 700);
                if (currentGameMode == VitaruGamemode.Dodge || currentGameMode == VitaruGamemode.Gravaru)
                    VitaruPlayer.Position = BaseSize / 2;
            }
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

            if (VitaruPlayer != null)
            {
                DrawableVitaruJudgement explosion = new DrawableVitaruJudgement(judgement, judgedObject)
                {
                    Alpha = 0.5f,
                    Origin = Anchor.Centre,
                    Position = new Vector2(VitaruPlayer.Position.X, VitaruPlayer.Position.Y + 50)
                };

                judgementLayer.Add(explosion);
            }
        }

        protected virtual CursorContainer CreateCursor() => new GameplayCursor();
    }
}
