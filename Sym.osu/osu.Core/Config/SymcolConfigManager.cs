using System.ComponentModel;
using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace osu.Core.Config
{
    public class SymcolConfigManager : IniConfigManager<SymcolSetting>
    {
        protected override string Filename => "symcol.ini";

        public SymcolConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(SymcolSetting.Version, "");
            Set(SymcolSetting.FreshInstall, true);

            Set(SymcolSetting.PlayerColor, "#ffffff");
            Set(SymcolSetting.SavedIP, "IP Address");
            Set(SymcolSetting.SavedPort, 25570);
            Set(SymcolSetting.SavedName, "Guest");
            Set(SymcolSetting.SavedUserID, -1L);
            Set(SymcolSetting.Auto, AutoJoin.None);
        }
    }

    public enum SymcolSetting
    {
        Version,
        FreshInstall,

        //Old networking stuff
        PlayerColor,
        SavedIP,
        SavedPort,
        SavedName,
        SavedUserID,
        Auto
    }

    public enum AutoJoin
    {
        None,
        [Description("Join Server")]
        Join,
        [Description("Host Server")]
        Host,
    }
}
