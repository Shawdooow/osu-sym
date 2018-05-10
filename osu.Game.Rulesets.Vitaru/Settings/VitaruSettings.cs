using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Overlays.Settings;
using Symcol.Rulesets.Core.Wiki;
using osu.Game.Rulesets.Vitaru.Wiki;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using eden.Game.GamePieces;
using Symcol.Rulesets.Core.Rulesets;
using System.ComponentModel;
using System.Collections.Generic;
using Symcol.Core.Extentions;
using System.Linq;

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

        private Bindable<Gamemodes> selectedGamemode;

        private Bindable<string> selectedCharacter;

        private FillFlowContainer vitaruCharacter;
        private SettingsDropdown<string> vitaruCharacterDropdown;
        private Bindable<string> selectedVitaruCharacter;
        private FillFlowContainer touhosuCharacter;
        private SettingsDropdown<string> touhosuCharacterDropdown;
        private Bindable<string> selectedTouhosuCharacter;

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

            selectedGamemode = VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

            selectedCharacter = VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);
            selectedVitaruCharacter = VitaruConfigManager.GetBindable<string>(VitaruSetting.VitaruCharacter);
            selectedTouhosuCharacter = VitaruConfigManager.GetBindable<string>(VitaruSetting.TouhosuCharacter);

            List<KeyValuePair<string, string>> vitaruItems = new List<KeyValuePair<string, string>>();
            foreach (VitaruCharacters character in System.Enum.GetValues(typeof(VitaruCharacters)))
                vitaruItems.Add(new KeyValuePair<string, string>(character.GetDescription(), character.ToString()));
            selectedVitaruCharacter.ValueChanged += character => { VitaruConfigManager.Set(VitaruSetting.VitaruCharacter, character); VitaruConfigManager.Set(VitaruSetting.Character, character); };

            List<KeyValuePair<string, string>> touhosuItems = new List<KeyValuePair<string, string>>();
            foreach (TouhosuCharacters character in System.Enum.GetValues(typeof(TouhosuCharacters)))
                touhosuItems.Add(new KeyValuePair<string, string>(character.GetDescription(), character.ToString()));
            selectedTouhosuCharacter.ValueChanged += character => { VitaruConfigManager.Set(VitaruSetting.TouhosuCharacter, character); VitaruConfigManager.Set(VitaruSetting.Character, character); };


            Children = new Drawable[]
            {
                new SettingsEnumDropdown<Gamemodes>
                {
                    LabelText = "Vitaru's current gamemode",
                    Bindable = selectedGamemode
                },
                vitaruCharacter = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 0,
                    AutoSizeEasing = Easing.OutQuint,
                    Masking = true,

                    Child = vitaruCharacterDropdown = new SettingsDropdown<string>
                    {
                        LabelText = "Selected Vitaru Character",
                        Items = vitaruItems.Distinct().ToList()
                    },
                },
                touhosuCharacter = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 0,
                    AutoSizeEasing = Easing.OutQuint,
                    Masking = true,

                    Child = touhosuCharacterDropdown = new SettingsDropdown<string>
                    {
                        LabelText = "Selected Touhosu Character",
                        Items = touhosuItems.Distinct().ToList()
                    },
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

            vitaruCharacterDropdown.Bindable = selectedVitaruCharacter;
            touhosuCharacterDropdown.Bindable = selectedTouhosuCharacter;

            selectedGamemode.ValueChanged += value =>
            {
                if (value == Gamemodes.Touhosu)
                {
                    touhosuCharacter.ClearTransforms();
                    touhosuCharacter.AutoSizeAxes = Axes.Y;

                    vitaruCharacter.ClearTransforms();
                    vitaruCharacter.AutoSizeAxes = Axes.None;
                    vitaruCharacter.ResizeHeightTo(0, 0, Easing.OutQuint);

                    selectedCharacter.Value = selectedTouhosuCharacter.Value;
                }
                else
                {
                    vitaruCharacter.ClearTransforms();
                    vitaruCharacter.AutoSizeAxes = Axes.Y;

                    touhosuCharacter.ClearTransforms();
                    touhosuCharacter.AutoSizeAxes = Axes.None;
                    touhosuCharacter.ResizeHeightTo(0, 0, Easing.OutQuint);

                    selectedCharacter.Value = selectedVitaruCharacter.Value;
                }
            };
            selectedGamemode.TriggerChange();

            selectedTouhosuCharacter.ValueChanged += character =>
            {
                switch (character)
                {

                }
            };
            selectedTouhosuCharacter.TriggerChange();

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

    internal enum VitaruCharacters
    {
        [Description("Alex")]
        Alex
    }

    internal enum TouhosuCharacters
    {
        //The Hakurei Family, or whats left of them
        [Description("Reimu Hakurei")]
        ReimuHakurei,
        [Description("Ryukoy Hakurei")]
        RyukoyHakurei,
        [Description("Tomaji Hakurei")]
        TomajiHakurei,

        //Hakurei Family Friends, 
        [Description("Sakuya Izayoi")]
        SakuyaIzayoi,
        //[System.ComponentModel.Description("Flandre Scarlet")]
        //FlandreScarlet,
        //[System.ComponentModel.Description("Remilia Scarlet")]
        //RemiliaScarlet,

        //Uncle Vaster and Aunty Alice
        //[System.ComponentModel.Description("Alice Letrunce")]
        //AliceLetrunce,
        //[System.ComponentModel.Description("Vaster Letrunce")]
        //VasterLetrunce,

        //[System.ComponentModel.Description("Marisa Kirisame")]
        //MarisaKirisame,
    }
}
