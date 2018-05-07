using eden.Game.GamePieces;
using osu.Framework.Configuration;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;

namespace osu.Game.Rulesets.Vitaru.Settings
{
    public class VitaruConfigManager : IniConfigManager<VitaruSetting>
    {
        protected override string Filename => @"vitaru.ini";

        public VitaruConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(VitaruSetting.DebugOverlay, false);
            Set(VitaruSetting.DebugUIConfiguration, DebugUiConfiguration.PerformanceMetrics);
            Set(VitaruSetting.GraphicsPresets, GraphicsPresets.Standard);
            Set(VitaruSetting.GameMode, VitaruGamemode.Vitaru);
            Set(VitaruSetting.Characters, SelectableCharacters.SakuyaIzayoi);

            //Leaks like crazy atm
            Set(VitaruSetting.ComboFire, false);
            Set(VitaruSetting.ShittyMultiplayer, false);
            Set(VitaruSetting.FriendlyPlayerCount, 0, 0, 7);
            Set(VitaruSetting.FriendlyPlayerOverride, false);
            Set(VitaruSetting.EnemyPlayerCount, 0, 0, 8);
            Set(VitaruSetting.EnemyPlayerOverride, false);

            //Set(VitaruSetting.PlayerOne, PlayableCharacters.MarisaKirisame);
            //Set(VitaruSetting.PlayerTwo, PlayableCharacters.SakuyaIzayoi);
            /*
            Set(VitaruSetting.PlayerThree, Player.FlandreScarlet);
            Set(VitaruSetting.PlayerFour, Player.RemiliaScarlet);
            Set(VitaruSetting.PlayerFive, Player.Cirno);
            Set(VitaruSetting.PlayerSix, Player.YuyukoSaigyouji);
            Set(VitaruSetting.PlayerSeven, Player.YukariYakumo);
            */

            //Set(VitaruSetting.EnemyOne, PlayableCharacters.MarisaKirisame);
            //Set(VitaruSetting.EnemyTwo, PlayableCharacters.SakuyaIzayoi);
            /*
            Set(VitaruSetting.EnemyThree, Player.FlandreScarlet);
            Set(VitaruSetting.EnemyFour, Player.RemiliaScarlet);
            Set(VitaruSetting.EnemyFive, Player.Cirno);
            Set(VitaruSetting.EnemySix, Player.YuyukoSaigyouji);
            Set(VitaruSetting.EnemySeven, Player.YukariYakumo);
            Set(VitaruSetting.EnemyEight, Player.ByakurenHijiri);
            */

            Set(VitaruSetting.VectorVideos, true);
            Set(VitaruSetting.Skin, "default");
        }

    }

    public enum VitaruSetting
    {
        DebugOverlay,
        DebugUIConfiguration,
        GraphicsPresets,
        GameMode,
        Characters,
        ComboFire,
        ShittyMultiplayer,
        FriendlyPlayerCount,
        FriendlyPlayerOverride,
        EnemyPlayerCount,
        EnemyPlayerOverride,

        //Becuase fuck arrays
        PlayerOne,
        PlayerTwo,
        PlayerThree,
        PlayerFour,
        PlayerFive,
        PlayerSix,
        PlayerSeven,

        //See above comment
        EnemyOne,
        EnemyTwo,
        EnemyThree,
        EnemyFour,
        EnemyFive,
        EnemySix,
        EnemySeven,
        EnemyEight,

        VectorVideos,
        Skin,
    }

    public enum GraphicsPresets
    {
        HighPerformance,
        Standard,
    }
}
