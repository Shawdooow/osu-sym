#region usings

using osu.Framework.Configuration;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Settings
{
    public class VitaruConfigManager : IniConfigManager<VitaruSetting>
    {
        protected override string Filename => @"vitaru.ini";

        public VitaruConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(VitaruSetting.Gamemode, "Vitaru");
            Set(VitaruSetting.Chapter, "Vitaru");
            Set(VitaruSetting.Character, "Alex");

            Set(VitaruSetting.Touch, false);

            Set(VitaruSetting.ThemesPreset, ThemesPresets.Standard);
            Set(VitaruSetting.Sounds, SoundsOptions.Lazer);
            Set(VitaruSetting.PlayerVisuals, GraphicsOptions.Standard);
            Set(VitaruSetting.EnemyVisuals, GraphicsOptions.Standard);
            Set(VitaruSetting.BulletVisuals, GraphicsOptions.Standard);
            Set(VitaruSetting.LaserVisuals, GraphicsOptions.Standard);
            Set(VitaruSetting.PitchShade, true);
            Set(VitaruSetting.KiaiBoss, true);
            Set(VitaruSetting.PlayfieldBorder, true);
            Set(VitaruSetting.ComboFire, true);

            Set(VitaruSetting.DebugMode, false);
            Set(VitaruSetting.Editor, false);
            Set(VitaruSetting.EditorBoss, false);
            Set(VitaruSetting.DisableBullets, false);
            Set(VitaruSetting.RankedFilter, false);
            Set(VitaruSetting.Patterns, false);
            Set(VitaruSetting.DebugConfiguration, DebugConfiguration.General);
            Set(VitaruSetting.AutoType, AutoType.New);

            Set(VitaruSetting.Souls, 0);
            Set(VitaruSetting.Temperature, 37.1d);
        }

    }

    public enum VitaruSetting
    {
        Gamemode,
        Chapter,
        Character,

        Touch,

        ThemesPreset,
        Sounds,
        PlayerVisuals,
        EnemyVisuals,
        BulletVisuals,
        LaserVisuals,
        PitchShade,
        KiaiBoss,
        PlayfieldBorder,
        ComboFire,

        DebugMode,
        Editor,
        EditorBoss,
        DisableBullets,
        RankedFilter,
        Patterns,
        DebugConfiguration,
        AutoType,

        Souls,
        Temperature,
    }

    public enum ThemesPresets
    {
        Standard,
        Classic,
        HighPerformance,
        Experimental,
        Old,
        Custom,
    }

    public enum GraphicsOptions
    {
        Standard,
        Classic,
        HighPerformance,
        Experimental,
        Old,
    }

    public enum SoundsOptions
    {
        Lazer,
        Classic,
        Experimental,
    }
}
