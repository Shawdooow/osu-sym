using System;
using osu.Framework.Allocation;
using osu.Game;
using osu.Game.Beatmaps;
using osu.Game.Online.API;
using Symcol.Core.LegacyNetworking;
using Symcol.Rulesets.Core.Rulesets;

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Networking
{
    //TODO: This NEEDS its own clock to avoid fuckery later on with DoubleTime and HalfTime
    public class RulesetNetworkingClientHandler : NetworkingClientHandler, IOnlineComponent
    {
        public RulesetClientInfo RulesetClientInfo;

        public Action<WorkingBeatmap> OnMapChange;

        private OsuGame osu;

        public RulesetNetworkingClientHandler(ClientType type, string ip, int port = 25570) : base(type, ip, port)
        {
            if (RulesetClientInfo == null)
            {
                RulesetClientInfo = new RulesetClientInfo
                {
                    Port = port,
                    IP = ip
                };

                ClientInfo = RulesetClientInfo;
            }
        }

        /// <summary>
        /// Send Map to Peers
        /// </summary>
        /// <param name="map"></param>
        public void SetMap(WorkingBeatmap map)
        {
            RulesetPacket packet;
            try
            {
                packet = new RulesetPacket(RulesetClientInfo)
                {
                    SetMap = true,
                    OnlineBeatmapSetID = (int)map.BeatmapSetInfo.OnlineBeatmapSetID,
                    OnlineBeatmapID = (int)map.BeatmapInfo.OnlineBeatmapID
                };
                SendToInMatchClients(packet);
                //OnMapChange?.Invoke(osu.Beatmap.Value);
            }
            catch
            {
                packet = new RulesetPacket(RulesetClientInfo)
                {
                    SetMap = true,
                    BeatmapName = map.Metadata.Title,
                    BeatmapDifficulty = map.BeatmapInfo.Version,
                    Mapper = map.Metadata.Author.Username
                };
                SendToInMatchClients(packet);
                //OnMapChange?.Invoke(osu.Beatmap.Value);
            }
        }

        [BackgroundDependencyLoader]
        private void load(APIAccess api, OsuGame osu)
        {
            api.Register(this);
            this.osu = osu;
        }

        public void APIStateChanged(APIAccess api, APIState state)
        {
            switch (state)
            {
                default:
                    RulesetClientInfo.Username = SymcolSettingsSubsection.SymcolConfigManager.Get<string>(SymcolSetting.SavedName);
                    RulesetClientInfo.UserID = SymcolSettingsSubsection.SymcolConfigManager.Get<int>(SymcolSetting.SavedUserID);
                    break;
                case APIState.Online:
                    SymcolSettingsSubsection.SymcolConfigManager.Set(SymcolSetting.SavedName, api.LocalUser.Value.Username);
                    SymcolSettingsSubsection.SymcolConfigManager.Set(SymcolSetting.SavedUserID, api.LocalUser.Value.Id);

                    RulesetClientInfo.Username = api.LocalUser.Value.Username;
                    RulesetClientInfo.UserID = api.LocalUser.Value.Id;
                    RulesetClientInfo.UserCountry = api.LocalUser.Value.Country.FullName;
                    RulesetClientInfo.CountryFlagName = api.LocalUser.Value.Country.FlagName;
                    RulesetClientInfo.UserPic = api.LocalUser.Value.AvatarUrl;
                    RulesetClientInfo.UserBackground = api.LocalUser.Value.CoverUrl;
                    break;
            }
        }
    }
}
