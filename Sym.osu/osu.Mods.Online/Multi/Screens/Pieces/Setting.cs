using System;

namespace osu.Mods.Online.Multi.Screens.Pieces
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
    }
}
