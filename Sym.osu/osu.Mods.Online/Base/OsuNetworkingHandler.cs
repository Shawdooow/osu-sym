#region usings

using osu.Core;
using osu.Core.Config;
using osu.Framework.Allocation;
using osu.Framework.Logging;
using osu.Game.Online.API;
using osu.Mods.Online.Base.Packets;
using osu.Mods.Online.Multi.Lobby.Packets;
using Sym.Networking.NetworkingHandlers;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;

#endregion

namespace osu.Mods.Online.Base
{
    public class OsuNetworkingHandler : PeerNetworkingHandler, IOnlineComponent
    {
        protected override string Gamekey => "osu";

        public OsuUserInfo OsuUserInfo = new OsuUserInfo();

        private APIState apiState = APIState.Offline;

        private bool connectReady;
        private double forceTime = double.MinValue;

        public OsuNetworkingHandler()
        {
            OnConnectedToHost += host =>
            {
                if (Tcp && TcpNetworkingClient?.TcpClient != null && TcpNetworkingClient.TcpClient.Connected)
                    Logger.Log("Tcp connected", LoggingTarget.Network);
                else if (Tcp)
                    Logger.Log("Tcp failed to connect!", LoggingTarget.Network, LogLevel.Error);
            };
        }

        public override void Connect()
        {
            if (apiState != APIState.Online)
            {
                connectReady = true;
                forceTime = Time.Current + 2000;
            }
            else
                connect();
        }

        private void connect()
        {
            Logger.Log($"Attempting to connect to {UdpNetworkingClient.Address}", LoggingTarget.Network);
            SendToServer(new OsuConnectPacket());
            ConnectionStatues = ConnectionStatues.Connecting;
        }

        protected override void Update()
        {
            base.Update();

            if (forceTime <= Time.Current && ConnectionStatues <= ConnectionStatues.Disconnected)
            {
                connect();
                Logger.Log($"Joining server (without logging in) as ({OsuUserInfo.Username} + {OsuUserInfo.ID})", LoggingTarget.Network, LogLevel.Error);
            }
        }

        [BackgroundDependencyLoader]
        private void load(APIAccess api)
        {
            api.Register(this);

            OsuUserInfo.Colour = SymcolOsuModSet.SymConfigManager.Get<string>(SymSetting.PlayerColor);
            OsuUserInfo.Username = SymcolOsuModSet.SymConfigManager.Get<string>(SymSetting.SavedName);
            OsuUserInfo.ID = SymcolOsuModSet.SymConfigManager.Get<long>(SymSetting.SavedUserID);
        }

        protected override Packet SignPacket(Packet packet)
        {
            switch (packet)
            {
                case OsuConnectPacket connectPacket:
                    connectPacket.User = OsuUserInfo;
                    break;
                case OnlinePacket onlinePacket:
                    onlinePacket.User = OsuUserInfo;
                    break;
            }
            return base.SignPacket(packet);
        }

        public void APIStateChanged(APIAccess api, APIState state)
        {
            apiState = state;
            OsuUserInfo.Colour = SymcolOsuModSet.SymConfigManager.GetBindable<string>(SymSetting.PlayerColor);

            switch (state)
            {
                default:
                    OsuUserInfo.Username = SymcolOsuModSet.SymConfigManager.Get<string>(SymSetting.SavedName);
                    OsuUserInfo.ID = SymcolOsuModSet.SymConfigManager.Get<long>(SymSetting.SavedUserID);
                    break;
                case APIState.Online:
                    SymcolOsuModSet.SymConfigManager.Set(SymSetting.SavedName, api.LocalUser.Value.Username);
                    SymcolOsuModSet.SymConfigManager.Set(SymSetting.SavedUserID, api.LocalUser.Value.Id);

                    OsuUserInfo.Username = api.LocalUser.Value.Username;
                    OsuUserInfo.ID = api.LocalUser.Value.Id;
                    OsuUserInfo.Country = api.LocalUser.Value.Country.FullName;
                    OsuUserInfo.CountryFlagName = api.LocalUser.Value.Country.FlagName;
                    OsuUserInfo.Pic = api.LocalUser.Value.AvatarUrl;
                    OsuUserInfo.Background = api.LocalUser.Value.CoverUrl;

                    if (connectReady)
                        connect();
                    else
                        Logger.Log("We are now online, you should reconnect to the Sym Server at your earliest convenience!", LoggingTarget.Network, LogLevel.Important);
                    break;
            }
        }
    }
}
