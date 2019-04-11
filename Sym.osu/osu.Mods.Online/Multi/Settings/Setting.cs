#region usings

using System;
using osu.Mods.Online.Multi.Settings.Options;

#endregion

namespace osu.Mods.Online.Multi.Settings
{
    [Serializable]
    public class Setting<T> : Setting
    {
        public T Value;
    }

    [Serializable]
    public class Setting
    {
        public string Name;

        public Sync Sync;
    }
}
