using osu.Framework.Configuration;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Debug;

namespace osu.Game.Rulesets.Vitaru.Settings
{
    public class VitaruConfigManager : IniConfigManager<VitaruSetting>
    {
        protected override string Filename => @"vitaru.ini";

        public VitaruConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(VitaruSetting.AnnoyPlayer, true);

            Set(VitaruSetting.GameMode, Gamemodes.Vitaru);

            Set(VitaruSetting.Character, "Alex");
            Set(VitaruSetting.VitaruCharacter, "Alex");
            Set(VitaruSetting.TouhosuCharacter, "SakuyaIzayoi");

            Set(VitaruSetting.DebugMode, false);
            Set(VitaruSetting.DebugConfiguration, DebugConfiguration.General);

            Set(VitaruSetting.GraphicsPreset, GraphicsPresets.Standard);
            Set(VitaruSetting.BulletVisuals, GraphicsOptions.Standard);
            Set(VitaruSetting.PlayerVisuals, GraphicsOptions.Standard);
            Set(VitaruSetting.PitchShade, true);
            Set(VitaruSetting.KiaiBoss, false);
            Set(VitaruSetting.PlayfieldBorder, false);
            Set(VitaruSetting.ComboFire, true);

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
        AnnoyPlayer,

        GameMode,

        Character,
        VitaruCharacter,
        TouhosuCharacter,

        DebugMode,
        DebugConfiguration,
        NeuralNetworkState,

        GraphicsPreset,
        BulletVisuals,
        PlayerVisuals,
        PitchShade,
        KiaiBoss,
        PlayfieldBorder,
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
        Standard,
        StandardV2,
        HighPerformance,
        Custom
    }

    public enum GraphicsOptions
    {
        Standard,
        StandardV2,
        HighPerformance
    }
}
