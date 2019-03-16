using System;

namespace osu.Mods.Online.Multi.Packets
{
    [Serializable]
    public class Host
    {
        public string Username = @"Guest";

        public long UserID = -1;

        public string UserCountry = "US";
    }
}
