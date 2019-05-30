using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Classic.Settings
{
    public class ClassicConfigManager : IniConfigManager<ClassicSetting>
    {
        protected override string Filename => @"classic.ini";

        public ClassicConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(ClassicSetting.Black, false);
            Set(ClassicSetting.Hold, false);
            Set(ClassicSetting.Accel, false);
            Set(ClassicSetting.Approaching, false);
            Set(ClassicSetting.SliderEasing, Easing.None);
            Set(ClassicSetting.Skin, "default");
        }

    }

    public enum ClassicSetting
    {
        Black,
        Hold,
        Accel,
        Approaching,
        SliderEasing,
        Skin
    }
}
