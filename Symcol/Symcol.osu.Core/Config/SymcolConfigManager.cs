using osu.Framework.Configuration;
using osu.Framework.Platform;

namespace Symcol.osu.Core.Config
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
            Set(SymcolSetting.HostIP, "Host's IP Address");
            Set(SymcolSetting.LocalIP, "Local IP Address");
            Set(SymcolSetting.HostPort, 25570);
            Set(SymcolSetting.LocalPort, 25570);
            Set(SymcolSetting.SavedName, "Guest");
            Set(SymcolSetting.SavedUserID, -1);
        }
    }

    public enum SymcolSetting
    {
        Version,
        FreshInstall,

        //Old networking stuff
        PlayerColor,
        HostIP,
        LocalIP,
        HostPort,
        LocalPort,
        SavedName,
        SavedUserID
    }
}
