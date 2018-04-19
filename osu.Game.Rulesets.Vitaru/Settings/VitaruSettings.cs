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

        private Bindable<SelectableCharacters> selectedCharacter;

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
            selectedCharacter = VitaruConfigManager.GetBindable<SelectableCharacters>(VitaruSetting.Characters);
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
                new SettingsEnumDropdown<SelectableCharacters>
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
                /*
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
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerOne override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerOne)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerTwo override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerTwo)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerThree override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerThree)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerFour override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerFour)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerFive override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerFive)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerSix override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerSix)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "PlayerSeven override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.PlayerSeven)
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
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyOne override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemyOne)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyTwo override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemyTwo)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyThree override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemyThree)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyFour override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemyFour)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyFive override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemyFive)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemySix override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemySix)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyEight override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemySeven)
                                },
                                new SettingsEnumDropdown<PlayableCharacters>
                                {
                                    LabelText = "EnemyEight override",
                                    Bindable = VitaruConfigManager.GetBindable<PlayableCharacters>(VitaruSetting.EnemyEight)
                                }
                            }
                        },
                    }
                },
                */
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

            /*
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
            */

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
