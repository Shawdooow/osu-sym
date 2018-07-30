using System;
using Symcol.Core.Networking;

namespace Symcol.osu.Mods.Multi.Networking
{
    /// <summary>
    /// Includes osu User information
    /// </summary>
    [Serializable]
    public class OsuClientInfo : ClientInfo
    {
        //TODO: use User instead?
        //public User User;

        public string Username = "";

        public long UserID = -1;

        public string UserPic;

        public string UserBackground;

        public string UserCountry;

        public string CountryFlagName;
    }
}
