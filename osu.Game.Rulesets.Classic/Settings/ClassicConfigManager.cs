using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Classic.Settings
{
    public class ClassicConfigManager : IniConfigManager<ClassicSetting>
    {
        protected override string Filename => @"classic.ini";

        public ClassicConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(ClassicSetting.Hold, false);
            Set(ClassicSetting.Skin, "default");
        }

    }

    public enum ClassicSetting
    {
        Hold,
        Skin
    }
}
