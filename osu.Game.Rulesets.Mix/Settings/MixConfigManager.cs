using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace osu.Game.Rulesets.Mix.Settings
{
    public class MixConfigManager : IniConfigManager<MixSetting>
    {
        protected override string Filename => @"mix.ini";

        public MixConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
        }
    }

    public enum MixSetting
    {
    }
}
