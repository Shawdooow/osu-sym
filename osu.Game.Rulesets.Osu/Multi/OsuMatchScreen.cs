using osu.Framework.Screens;
using osu.Game.Rulesets.Osu.UI;
using Symcol.Core.Networking;
using Symcol.Rulesets.Core;
using Symcol.Rulesets.Core.Multiplayer.Networking;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Osu.Multi
{
    public class OsuMatchScreen : RulesetMatchScreen
    {
        public readonly RulesetNetworkingClientHandler OsuNetworkingClientHandler;

        public OsuMatchScreen(RulesetNetworkingClientHandler osuNetworkingClientHandler) : base(osuNetworkingClientHandler)
        {
            OsuNetworkingClientHandler = osuNetworkingClientHandler;
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);

            OsuInputManager.LoadPlayerList = new List<RulesetClientInfo>();
            OsuInputManager.RulesetNetworkingClientHandler = OsuNetworkingClientHandler;
            MakeCurrent();
            OsuNetworkingClientHandler.OnLoadGame = (i) => Load(i);
        }

        protected override void OnResuming(Screen last)
        {
            base.OnResuming(last);
            OsuInputManager.LoadPlayerList = new List<RulesetClientInfo>();
        }

        protected override void Load(List<ClientInfo> playerList)
        {
            MakeCurrent();
            foreach (ClientInfo client in playerList)
                if (client is RulesetClientInfo rulesetClientInfo)
                    OsuInputManager.LoadPlayerList.Add(rulesetClientInfo);

            Push(new MultiPlayer(OsuNetworkingClientHandler, playerList));
        }
    }
}
