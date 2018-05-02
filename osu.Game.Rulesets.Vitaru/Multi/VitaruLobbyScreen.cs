using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    public class VitaruLobbyScreen : RulesetLobbyScreen
    {
        public VitaruNetworkingClientHandler VitaruNetworkingClientHandler;

        public override RulesetMatchScreen MatchScreen => new VitaruMatchScreen(VitaruNetworkingClientHandler);

        protected override void HostGame()
        {
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                VitaruNetworkingClientHandler.Dispose();
            }
            VitaruNetworkingClientHandler = new VitaruNetworkingClientHandler(ClientType.Host, Ip.Text, Int32.Parse(HostPort.Text));
            RulesetNetworkingClientHandler = VitaruNetworkingClientHandler;
            Add(RulesetNetworkingClientHandler);

            List<ClientInfo> list = new List<ClientInfo>
            {
                RulesetNetworkingClientHandler.RulesetClientInfo
            };

            JoinMatch(list);
        }

        protected override void DirectConnect()
        {
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                VitaruNetworkingClientHandler.Dispose();
            }
            VitaruNetworkingClientHandler = new VitaruNetworkingClientHandler(ClientType.Peer, Ip.Text, Int32.Parse(HostPort.Text));
            VitaruNetworkingClientHandler.OnConnectedToHost += (p) => JoinMatch(p);
            RulesetNetworkingClientHandler = VitaruNetworkingClientHandler;
            Add(RulesetNetworkingClientHandler);
        }
    }
}
