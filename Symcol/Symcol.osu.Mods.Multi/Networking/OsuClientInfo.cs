using osu.Game.Users;
using Symcol.Core.Networking;

namespace Symcol.osu.Mods.Multi.Networking
{
    public class OsuClientInfo : ClientInfo
    {
        //TODO: Use User instead
        public User User;

        public string Username = "";

        public long UserID = -1;

        public string UserPic;

        public string UserBackground;

        public string UserCountry;

        public string CountryFlagName;
    }
}
