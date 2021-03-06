﻿#region usings

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Platform;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Mods.Rulesets.Core.Rulesets;

#endregion

// ReSharper disable UnusedMember.Global

namespace osu.Game.Rulesets.Vitaru.Ruleset.Settings
{
    public class VitaruSettings : SymcolSettingsSubsection
    {
        protected override string Header => "vitaru!";

        public static VitaruConfigManager VitaruConfigManager;

        private Bindable<ThemesPresets> themes;
        private FillFlowContainer graphicsOptions;

        private FillFlowContainer debugUiSettings;
        private Bindable<bool> showDebugUi;

        private const int transition_duration = 400;

        public static bool Editor => editor.Value;
        public static bool Patterns => patterns.Value;
        public static GraphicsOptions BulletGraphics => bulletGraphics.Value;

        private static Bindable<bool> editor;
        private static Bindable<bool> patterns;
        private static Bindable<GraphicsOptions> bulletGraphics;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            Add(new VitaruAPIContainer());

            VitaruConfigManager = new VitaruConfigManager(host.Storage);

            Bindable<string> gamemodeBindable = VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

            showDebugUi = VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugMode);
            themes = VitaruConfigManager.GetBindable<ThemesPresets>(VitaruSetting.ThemesPreset);

            ChapterStore.ReloadChapterSets();

            List<string> gamemodeItems = new List<string>();
            foreach (ChapterStore.LoadedChapterSet g in ChapterStore.LoadedChapterSets)
                gamemodeItems.Add(g.ChapterSet.Name);
            gamemodeBindable.ValueChanged += g => { VitaruConfigManager.Set(VitaruSetting.Gamemode, g); };

            SettingsDropdown<string> gamemodeDropdown = new SettingsDropdown<string>
            {
                LabelText = "Gamemode",
                Items = gamemodeItems.Distinct().ToList(),
            };

            editor = VitaruConfigManager.GetBindable<bool>(VitaruSetting.Editor);
            patterns = VitaruConfigManager.GetBindable<bool>(VitaruSetting.Patterns);
            bulletGraphics = VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.BulletVisuals);

            Children = new Drawable[]
            {
                gamemodeDropdown,
                new CharacterSelection(),
                new SettingsEnumDropdown<ThemesPresets>
                {
                    LabelText = "Theme Preset",
                    Bindable = themes
                },
                graphicsOptions = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = transition_duration,
                    AutoSizeEasing = Easing.OutQuint,
                    Masking = true,

                    Children = new Drawable[]
                    {
                        new SettingsEnumDropdown<SoundsOptions>
                        {
                            LabelText = "Sounds",
                            Bindable = VitaruConfigManager.GetBindable<SoundsOptions>(VitaruSetting.Sounds)
                        },
                        new SettingsEnumDropdown<GraphicsOptions>
                        {
                            LabelText = "Player Visuals",
                            Bindable = VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.PlayerVisuals)
                        },
                        new SettingsEnumDropdown<GraphicsOptions>
                        {
                            LabelText = "Enemy Visuals",
                            Bindable = VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.EnemyVisuals)
                        },
                        new SettingsEnumDropdown<GraphicsOptions>
                        {
                            LabelText = "Bullet Visuals",
                            Bindable = VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.BulletVisuals)
                        },
                        new SettingsEnumDropdown<GraphicsOptions>
                        {
                            LabelText = "Lasers Visuals",
                            Bindable = VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.LaserVisuals)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Pitch Shade",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.PitchShade)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Kiai Boss",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.KiaiBoss)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Playfield Border",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.PlayfieldBorder)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Combo Fire",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.ComboFire)
                        },
                    } 
                },
                new SettingsCheckbox
                {
                    LabelText = "Touch Mode",
                    Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.Touch)
                },
                new SettingsCheckbox
                {
                    LabelText = "Debug Mode",
                    Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.DebugMode)
                },
                debugUiSettings = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = transition_duration,
                    AutoSizeEasing = Easing.OutQuint,
                    Masking = true,

                    Children = new Drawable[]
                    {
                        new SettingsCheckbox
                        {
                            LabelText = "Enable Editor",
                            Bindable = editor
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Show Boss in Editor",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.EditorBoss)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Ranked Play Filter",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.RankedFilter)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Disable Projectiles",
                            Bindable = VitaruConfigManager.GetBindable<bool>(VitaruSetting.DisableBullets)
                        },
                        new SettingsCheckbox
                        {
                            LabelText = "Enable Experimental Patterns",
                            Bindable = patterns
                        },
                        new SettingsEnumDropdown<DebugConfiguration>
                        {
                            LabelText = "Debug Tools Configuration",
                            Bindable = VitaruConfigManager.GetBindable<DebugConfiguration>(VitaruSetting.DebugConfiguration)
                        },
                        new SettingsEnumDropdown<AutoType>
                        {
                            LabelText = "Auto Type",
                            Bindable = VitaruConfigManager.GetBindable<AutoType>(VitaruSetting.AutoType)
                        },
                    }
                },
            };

            gamemodeDropdown.Bindable = VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

            themes.ValueChanged += value =>
            {
                graphicsOptions.ClearTransforms();


                if (value == ThemesPresets.Custom)
                    graphicsOptions.AutoSizeAxes = Axes.Y;
                else
                {
                    graphicsOptions.AutoSizeAxes = Axes.None;
                    graphicsOptions.ResizeHeightTo(0, transition_duration, Easing.OutQuint);
                }

                switch (value)
                {
                    case ThemesPresets.Standard:
                        VitaruConfigManager.Set(VitaruSetting.Sounds, SoundsOptions.Lazer);
                        VitaruConfigManager.Set(VitaruSetting.PlayerVisuals, GraphicsOptions.Standard);
                        VitaruConfigManager.Set(VitaruSetting.EnemyVisuals, GraphicsOptions.Standard);
                        VitaruConfigManager.Set(VitaruSetting.BulletVisuals, GraphicsOptions.Standard);
                        VitaruConfigManager.Set(VitaruSetting.LaserVisuals, GraphicsOptions.Standard);

                        VitaruConfigManager.Set<bool>(VitaruSetting.PitchShade, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.KiaiBoss, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.PlayfieldBorder, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.ComboFire, true);
                        break;
                    case ThemesPresets.Classic:
                        VitaruConfigManager.Set(VitaruSetting.Sounds, SoundsOptions.Classic);
                        VitaruConfigManager.Set(VitaruSetting.PlayerVisuals, GraphicsOptions.Classic);
                        VitaruConfigManager.Set(VitaruSetting.EnemyVisuals, GraphicsOptions.Classic);
                        VitaruConfigManager.Set(VitaruSetting.BulletVisuals, GraphicsOptions.Classic);
                        VitaruConfigManager.Set(VitaruSetting.LaserVisuals, GraphicsOptions.Classic);

                        VitaruConfigManager.Set<bool>(VitaruSetting.PitchShade, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.KiaiBoss, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.PlayfieldBorder, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.ComboFire, false);
                        break;
                    case ThemesPresets.HighPerformance:
                        VitaruConfigManager.Set(VitaruSetting.Sounds, SoundsOptions.Lazer);
                        VitaruConfigManager.Set(VitaruSetting.PlayerVisuals, GraphicsOptions.HighPerformance);
                        VitaruConfigManager.Set(VitaruSetting.EnemyVisuals, GraphicsOptions.HighPerformance);
                        VitaruConfigManager.Set(VitaruSetting.BulletVisuals, GraphicsOptions.HighPerformance);
                        VitaruConfigManager.Set(VitaruSetting.LaserVisuals, GraphicsOptions.HighPerformance);

                        VitaruConfigManager.Set<bool>(VitaruSetting.PitchShade, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.KiaiBoss, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.PlayfieldBorder, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.ComboFire, false);
                        break;
                    case ThemesPresets.Experimental:
                        VitaruConfigManager.Set(VitaruSetting.Sounds, SoundsOptions.Experimental);
                        VitaruConfigManager.Set(VitaruSetting.PlayerVisuals, GraphicsOptions.Experimental);
                        VitaruConfigManager.Set(VitaruSetting.EnemyVisuals, GraphicsOptions.Experimental);
                        VitaruConfigManager.Set(VitaruSetting.BulletVisuals, GraphicsOptions.Experimental);
                        VitaruConfigManager.Set(VitaruSetting.LaserVisuals, GraphicsOptions.Experimental);

                        VitaruConfigManager.Set<bool>(VitaruSetting.PitchShade, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.KiaiBoss, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.PlayfieldBorder, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.ComboFire, true);
                        break;
                    case ThemesPresets.Old:
                        VitaruConfigManager.Set(VitaruSetting.Sounds, SoundsOptions.Lazer);
                        VitaruConfigManager.Set(VitaruSetting.PlayerVisuals, GraphicsOptions.Old);
                        VitaruConfigManager.Set(VitaruSetting.EnemyVisuals, GraphicsOptions.Old);
                        VitaruConfigManager.Set(VitaruSetting.BulletVisuals, GraphicsOptions.Old);
                        VitaruConfigManager.Set(VitaruSetting.LaserVisuals, GraphicsOptions.Old);

                        VitaruConfigManager.Set<bool>(VitaruSetting.PitchShade, true);
                        VitaruConfigManager.Set<bool>(VitaruSetting.KiaiBoss, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.PlayfieldBorder, false);
                        VitaruConfigManager.Set<bool>(VitaruSetting.ComboFire, true);
                        break;
                }
            };
            themes.TriggerChange();

            showDebugUi.ValueChanged += isVisible =>
            {
                debugUiSettings.ClearTransforms();
                debugUiSettings.AutoSizeAxes = isVisible ? Axes.Y : Axes.None;

                if (!isVisible)
                    debugUiSettings.ResizeHeightTo(0, transition_duration, Easing.OutQuint);
            };
            showDebugUi.TriggerChange();
        }

        public VitaruSettings(Rulesets.Ruleset ruleset)
            : base(ruleset)
        {
        }
    }
}
