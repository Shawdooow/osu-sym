using OpenTK;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Overlays.Settings;
using System.Collections.Generic;
using System.Linq;
using Symcol.Rulesets.Core;
using Symcol.Rulesets.Core.Wiki;
using osu.Game.Rulesets.Vitaru.Wiki;
using osu.Game.Rulesets.Vitaru.Scoring;
using osu.Game.Rulesets.Vitaru.Edit;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using eden.Game.GamePieces;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;

namespace osu.Game.Rulesets.Vitaru.Settings
{
    public class VitaruSettings : SymcolSettingsSubsection
    {
        protected override string Header => "vitaru!";

        public override WikiOverlay Wiki => vitaruWiki;

        public override RulesetLobbyItem RulesetLobbyItem => vitaruLobby;

        private readonly VitaruWikiOverlay vitaruWiki = new VitaruWikiOverlay();

        private readonly VitaruLobbyItem vitaruLobby = new VitaruLobbyItem();

        public static VitaruConfigManager VitaruConfigManager;

        private static VitaruAPIContainer api;

        private Bindable<PlayableCharacters> selectedCharacter;

        private FillFlowContainer multiplayerSettings;
        private Bindable<bool> multiplayer;
        private Bindable<int> friendlyPlayerCount;
        private Bindable<bool> friendlyPlayerOverride;
        private FillFlowContainer friendlyPlayerSettings;
        private Bindable<int> enemyPlayerCount;
        private Bindable<bool> enemyPlayerOverride;
        private FillFlowContainer enemyPlayerSettings;

        private FillFlowContainer debugUiSettings;
        private Bindable<bool> showDebugUi;

        private SettingsDropdown<string> skin;
        private Bindable<string> currentSkin;

        private const int transition_duration = 400;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            if (api == null)
                Add(api = new VitaruAPIContainer());

            VitaruConfigManager = new VitaruConfigManager(host.Storage);

            Storage skinsStorage = storage.GetStorageForDirectory("Skins");

            showDebugUi = VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugOverlay);
            selectedCharacter = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.Characters);
            multiplayer = VitaruConfigManager.GetBindable<bool>(VitaruSetting.ShittyMultiplayer);
            friendlyPlayerCount = VitaruConfigManager.GetBindable<int>(VitaruSetting.FriendlyPlayerCount);
            friendlyPlayerOverride = VitaruConfigManager.GetBindable<bool>(VitaruSetting.FriendlyPlayerOverride);
            enemyPlayerCount = VitaruConfigManager.GetBindable<int>(VitaruSetting.EnemyPlayerCount);
            enemyPlayerOverride = VitaruConfigManager.GetBindable<bool>(VitaruSetting.EnemyPlayerOverride);

            Children = new Drawable[]
            {
                new SettingsEnumDropdown<VitaruGamemode>
                {
                    LabelText = "Vitaru's current gamemode",
                    Bindable = VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode)
                },
                new SettingsEnumDropdown<PlayableCharacters>
                {
                    LabelText = "Selected Character",
                    Bindable = selectedCharacter
                },
                new SettingsEnumDropdown<GraphicsPresets>
                {
                    LabelText = "Graphics Presets",
                    Bindable = VitaruConfigManager.GetBindable<GraphicsPresets>(VitaruSetting.GraphicsPresets)
                },
                new SettingsEnumDropdown<ScoringMetric>
                {
                    LabelText = "Current Scoring Metric used (Difficulty, Score and PP)",
                    Bindable = VitaruConfigManager.GetBindable<ScoringMetric>(VitaruSetting.ScoringMetric)
                },
                new SettingsCheckbox
                {
                    LabelText = "Enable ComboFire",
                    Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.ComboFire)
                },
                new SettingsCheckbox
                {
                    LabelText = "Offline Multiplayer",
                    Bindable = multiplayer
                },
                multiplayerSettings = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = transition_duration,
                    AutoSizeEasing = Easing.OutQuint,
                    Masking = true,

                    Children = new Drawable[]
                    {
                        new SettingsSlider<int>
                        {
                            LabelText = "How many Friends?",
                            Bindable = friendlyPlayerCount,
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Override Friendly Characters",
                            Bindable = friendlyPlayerOverride
                        },
                        friendlyPlayerSettings = new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            AutoSizeDuration = transition_duration,
                            AutoSizeEasing = Easing.OutQuint,
                            Masking = true,

                            Children = new Drawable[]
                            {
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerOne override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerOne)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerTwo override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerTwo)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerThree override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerThree)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerFour override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerFour)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerFive override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerFive)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerSix override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerSix)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "PlayerSeven override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.PlayerSeven)
                                }
                            }
                        },
                        new SettingsSlider<int>
                        {
                            LabelText = "How many Enemies?",
                            Bindable = enemyPlayerCount,
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Override Enemy Characters",
                            Bindable = enemyPlayerOverride
                        },
                        enemyPlayerSettings = new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            AutoSizeDuration = transition_duration,
                            AutoSizeEasing = Easing.OutQuint,
                            Masking = true,

                            Children = new Drawable[]
                            {
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyOne override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyOne)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyTwo override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyTwo)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyThree override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyThree)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyFour override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyFour)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyFive override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyFive)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemySix override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemySix)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyEight override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemySeven)
                                },
                                new SettingsEnumDropdown<Player>
                                {
                                    LabelText = "EnemyEight override",
                                    Bindable = VitaruConfigManager.GetBindable<Player>(VitaruSetting.EnemyEight)
                                }
                            }
                        },
                    }
                },
                new SettingsButton
                {
                    Text = "Open In-game Wiki",
                    Action = vitaruWiki.Show
                },
                new SettingsCheckbox
                {
                    LabelText = "Show Debug UI In-Game",
                    Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugOverlay)
                },
                debugUiSettings = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = transition_duration,
                    AutoSizeEasing = Easing.OutQuint,
                    Masking = true,

                    Child = new SettingsEnumDropdown<DebugUiConfiguration>
                    {
                        LabelText = "What will be displayed on the DebugUI In-Game",
                        Bindable = VitaruConfigManager.GetBindable<DebugUiConfiguration>(VitaruSetting.DebugUIConfiguration)
                    }
                },
                skin = new SettingsDropdown<string>
                {
                    LabelText = "Current Skin"
                },
                new SettingsButton
                {
                    Text = "Open skins folder",
                    Action = skinsStorage.OpenInNativeExplorer,
                }
            };

            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Default", "default") };

            try
            {
                foreach (string skinName in storage.GetDirectories("Skins"))
                {
                    string[] args = skinName.Split('\\');
                    items.Add(new KeyValuePair<string, string>(args.Last(), args.Last()));
                }

                skin.Items = items.Distinct().ToList();
                currentSkin = VitaruConfigManager.GetBindable<string>(VitaruSetting.Skin);
                skin.Bindable = currentSkin;

                currentSkin.ValueChanged += skin => { VitaruConfigManager.Set(VitaruSetting.Skin, skin); };
            }
            catch { }

            //basically just an ingame wiki for the characters
            selectedCharacter.ValueChanged += character =>
            {
                /*
                if (character == Player.AliceMuyart | character == Player.ArysaMuyart && !VitaruAPIContainer.Shawdooow)
                {
                    selectedCharacter.Value = Player.ReimuHakurei;
                    character = Player.ReimuHakurei;
                }
                */
            };
            selectedCharacter.TriggerChange();

            multiplayer.ValueChanged += isVisible =>
            {
                multiplayerSettings.ClearTransforms();
                multiplayerSettings.AutoSizeAxes = isVisible ? Axes.Y : Axes.None;

                if (!isVisible)
                    multiplayerSettings.ResizeHeightTo(0, transition_duration, Easing.OutQuint);
            };
            multiplayer.TriggerChange();

            friendlyPlayerOverride.ValueChanged += isVisible =>
            {
                friendlyPlayerSettings.ClearTransforms();
                friendlyPlayerSettings.AutoSizeAxes = isVisible ? Axes.Y : Axes.None;

                if (!isVisible)
                    friendlyPlayerSettings.ResizeHeightTo(0, transition_duration, Easing.OutQuint);
            };
            friendlyPlayerOverride.TriggerChange();

            enemyPlayerOverride.ValueChanged += isVisible =>
            {
                enemyPlayerSettings.ClearTransforms();
                enemyPlayerSettings.AutoSizeAxes = isVisible ? Axes.Y : Axes.None;

                if (!isVisible)
                    enemyPlayerSettings.ResizeHeightTo(0, transition_duration, Easing.OutQuint);
            };
            enemyPlayerOverride.TriggerChange();

            showDebugUi.ValueChanged += isVisible =>
            {
                debugUiSettings.ClearTransforms();
                debugUiSettings.AutoSizeAxes = isVisible ? Axes.Y : Axes.None;

                if (!isVisible)
                    debugUiSettings.ResizeHeightTo(0, transition_duration, Easing.OutQuint);
            };
            showDebugUi.TriggerChange();
        }
    }
}
