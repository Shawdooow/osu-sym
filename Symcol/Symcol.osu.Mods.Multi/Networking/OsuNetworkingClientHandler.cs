using osu.Framework.Allocation;
using osu.Game.Online.API;
using Symcol.Core.Networking;
using Symcol.Core.Networking.Packets;
using Symcol.osu.Core;
using Symcol.osu.Core.Config;
using Symcol.osu.Mods.Multi.Networking.Packets.Match;

namespace Symcol.osu.Mods.Multi.Networking
{
    public class OsuNetworkingClientHandler : NetworkingHandler, IOnlineComponent
    {
        protected override string Gamekey => "osu";

        public OsuClientInfo OsuClientInfo => (OsuClientInfo)ClientInfo;

        public OsuNetworkingClientHandler()
        {
            ClientInfo = new OsuClientInfo();
        }

        [BackgroundDependencyLoader]
        private void load(APIAccess api)
        {
            api.Register(this);
        }

        protected override Packet SignPacket(Packet packet)
        {
            if (packet is MatchPacket matchPacket)
                matchPacket.Player = OsuClientInfo;
            return base.SignPacket(packet);
        }

        public void APIStateChanged(APIAccess api, APIState state)
        {
            switch (state)
            {
                default:
                    OsuClientInfo.Username = SymcolOsuModSet.SymcolConfigManager.Get<string>(SymcolSetting.SavedName);
                    OsuClientInfo.UserID = SymcolOsuModSet.SymcolConfigManager.Get<int>(SymcolSetting.SavedUserID);
                    break;
                case APIState.Online:
                    SymcolOsuModSet.SymcolConfigManager.Set(SymcolSetting.SavedName, api.LocalUser.Value.Username);
                    SymcolOsuModSet.SymcolConfigManager.Set(SymcolSetting.SavedUserID, api.LocalUser.Value.Id);

                    OsuClientInfo.Username = api.LocalUser.Value.Username;
                    OsuClientInfo.UserID = api.LocalUser.Value.Id;
                    OsuClientInfo.UserCountry = api.LocalUser.Value.Country.FullName;
                    OsuClientInfo.CountryFlagName = api.LocalUser.Value.Country.FlagName;
                    OsuClientInfo.UserPic = api.LocalUser.Value.AvatarUrl;
                    OsuClientInfo.UserBackground = api.LocalUser.Value.CoverUrl;
                    break;
            }
        }
    }
}
