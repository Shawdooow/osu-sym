using System;
using System.Collections.Generic;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi.Screens.Pieces;

namespace osu.Mods.Online.Multi.Packets.Match
{
    [Serializable]
    public class SettingPacket : OnlinePacket
    {
        public SettingPacket(Setting setting = null)
        {
            if (setting != null)
                Settings.Add(setting);
        }

        public List<Setting> Settings = new List<Setting>();
    }
}
