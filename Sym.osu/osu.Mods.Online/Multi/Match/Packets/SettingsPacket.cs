#region usings

using System;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi.Settings;

#endregion

namespace osu.Mods.Online.Multi.Match.Packets
{
    [Serializable]
    public class SettingsPacket : OnlinePacket
    {
        public readonly Setting[] Settings;

        public SettingsPacket(Setting[] settings) => Settings = settings;

        public SettingsPacket(Setting setting) => Settings = new[] {setting};
    }
}
