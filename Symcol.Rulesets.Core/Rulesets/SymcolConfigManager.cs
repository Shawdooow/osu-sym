using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Symcol.Rulesets.Core.Rulesets
{
    public class SymcolConfigManager : IniConfigManager<SymcolSetting>
    {
        protected override string Filename => "symcol.ini";

        public SymcolConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(SymcolSetting.PlayerColor, "#ffffff");
            Set(SymcolSetting.IP, "IP Address");
            Set(SymcolSetting.Port, 25570);
            Set(SymcolSetting.SavedName, "Guest");
            Set(SymcolSetting.SavedUserID, -1);
        }
    }

    public enum SymcolSetting
    {
        PlayerColor,
        IP,
        Port,
        SavedName,
        SavedUserID
    }
}
