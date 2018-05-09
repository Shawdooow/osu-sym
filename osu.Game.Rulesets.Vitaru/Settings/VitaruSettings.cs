using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Overlays.Settings;
using System.Collections.Generic;
using System.Linq;
using Symcol.Rulesets.Core.Wiki;
using osu.Game.Rulesets.Vitaru.Wiki;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using eden.Game.GamePieces;
using Symcol.Rulesets.Core.Rulesets;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;

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

        private Bindable<VitaruCharacters> selectedVitaruCharacter;
        private Bindable<TouhosuCharacters> selectedTouhosuCharacter;

        private FillFlowContainer debugUiSettings;
        private Bindable<bool> showDebugUi;

        private const int transition_duration = 400;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            if (api == null)
                Add(api = new VitaruAPIContainer());

            VitaruConfigManager = new VitaruConfigManager(host.Storage);

            showDebugUi = VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugOverlay);

            selectedVitaruCharacter = VitaruConfigManager.GetBindable<VitaruCharacters>(VitaruSetting.VitaruCharacter);
            selectedTouhosuCharacter = VitaruConfigManager.GetBindable<TouhosuCharacters>(VitaruSetting.TouhosuCharacter);

            Children = new Drawable[]
            {
                new SettingsEnumDropdown<Gamemodes>
                {
                    LabelText = "Vitaru's current gamemode",
                    Bindable = VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode)
                },
                new SettingsEnumDropdown<VitaruCharacters>
                {
                    LabelText = "Selected Vitaru Character",
                    Bindable = selectedVitaruCharacter
                },
                new SettingsEnumDropdown<TouhosuCharacters>
                {
                    LabelText = "Selected Touhosu Character",
                    Bindable = selectedTouhosuCharacter
                },
                new SettingsEnumDropdown<GraphicsPresets>
                {
                    LabelText = "Graphics Presets",
                    Bindable = VitaruConfigManager.GetBindable<GraphicsPresets>(VitaruSetting.GraphicsPresets)
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
            };

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
