using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Game.Online.API;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Networking;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    public class VitaruNetworkingClientHandler : RulesetNetworkingClientHandler, IOnlineComponent
    {
        private Bindable<string> selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

        public readonly VitaruClientInfo VitaruClientInfo;

        public VitaruNetworkingClientHandler(ClientType type, string ip, int port = 25570) : base(type, ip, port)
        {
            VitaruClientInfo = new VitaruClientInfo()
            {
                PlayerInformation = new VitaruPlayerInformation(),
                Port = port,
                IP = ip
            };

            RulesetClientInfo = VitaruClientInfo;
            ClientInfo = RulesetClientInfo;

            selectedCharacter.ValueChanged += character =>
            {
                VitaruClientInfo.PlayerInformation.Character = character;
                SendToHost(new VitaruPacket(VitaruClientInfo) { ChangeCharacter = true });
            };
            selectedCharacter.TriggerChange();

            OnPacketReceive += (Packet packet) =>
            {
                if (packet is VitaruPacket vitaruPacket)
                    if (vitaruPacket.ChangeCharacter)
                        foreach(ClientInfo clientInfo in ConncetedClients)
                            if (vitaruPacket.ClientInfo.IP == clientInfo.IP)
                            {
                                ConncetedClients.Remove(clientInfo);
                                InMatchClients.Remove(clientInfo);
                                ConncetedClients.Add(vitaruPacket.ClientInfo);
                                InMatchClients.Add(vitaruPacket.ClientInfo);
                                break;
                            }
            };
        }

        [BackgroundDependencyLoader]
        private void load(APIAccess api)
        {
            api.Register(this);
        }

        public new void APIStateChanged(APIAccess api, APIState state)
        {
            switch (state)
            {
                default:
                    VitaruClientInfo.Username = SymcolSettingsSubsection.SymcolConfigManager.Get<string>(SymcolSetting.SavedName);
                    VitaruClientInfo.UserID = SymcolSettingsSubsection.SymcolConfigManager.Get<int>(SymcolSetting.SavedUserID);
                    break;
                case APIState.Online:
                    SymcolSettingsSubsection.SymcolConfigManager.Set(SymcolSetting.SavedName, api.LocalUser.Value.Username);
                    SymcolSettingsSubsection.SymcolConfigManager.Set(SymcolSetting.SavedUserID, api.LocalUser.Value.Id);

                    VitaruClientInfo.Username = api.LocalUser.Value.Username;
                    VitaruClientInfo.UserID = api.LocalUser.Value.Id;
                    VitaruClientInfo.UserCountry = api.LocalUser.Value.Country.FullName;
                    VitaruClientInfo.CountryFlagName = api.LocalUser.Value.Country.FlagName;
                    VitaruClientInfo.UserPic = api.LocalUser.Value.AvatarUrl;
                    VitaruClientInfo.UserBackground = api.LocalUser.Value.CoverUrl;
                    break;
            }
            VitaruClientInfo.PlayerInformation.PlayerID = VitaruClientInfo.IP + VitaruClientInfo.UserID;
        }
    }
}
