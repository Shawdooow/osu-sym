#region usings

using System.ComponentModel;
using osu.Framework.Configuration;
using osu.Framework.Platform;

#endregion

namespace osu.Core.Config
{
    public class SymConfigManager : IniConfigManager<SymSetting>
    {
        protected override string Filename => "sym.ini";

        public SymConfigManager(Storage storage) : base(storage) { }

        protected override void InitialiseDefaults()
        {
            Set(SymSetting.Version, "");
            Set(SymSetting.FreshInstall, true);

            Set(SymSetting.PlayerColor, "#ffffff");
            Set(SymSetting.SavedIP, "IP Address");
            Set(SymSetting.SavedPort, 25570);
            Set(SymSetting.SavedName, "Guest");
            Set(SymSetting.SavedUserID, -1L);
            Set(SymSetting.Auto, AutoJoin.None);
        }
    }

    public enum SymSetting
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
