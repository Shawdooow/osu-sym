using System;
using Symcol.Core.LegacyNetworking;

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Networking
{
    /// <summary>
    /// Just a client signature basically
    /// </summary>
    [Serializable]
    public class RulesetClientInfo : ClientInfo
    {
        public string Username = "";

        public long UserID = -1;

        public string UserPic;

        public string UserBackground;

        public string UserCountry;

        public string CountryFlagName;
    }
}
