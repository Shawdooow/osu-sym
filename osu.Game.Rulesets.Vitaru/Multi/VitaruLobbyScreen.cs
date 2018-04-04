using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Vitaru.Multi
{
    public class VitaruLobbyScreen : RulesetLobbyScreen
    {
        public override string RulesetName => "vitaru";

        public VitaruNetworkingClientHandler VitaruNetworkingClientHandler;

        public override RulesetMatchScreen MatchScreen => new VitaruMatchScreen(VitaruNetworkingClientHandler);

        private readonly Bindable<string> hostIP = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.HostIP);
        private readonly Bindable<string> localIP = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.LocalIP);

        protected override void LoadComplete()
        {
            base.LoadComplete();

            //HostIP.Text = hostIP;
            Ip.Text = localIP;
        }

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

            List<ClientInfo> list = new List<ClientInfo>();
            list.Add(RulesetNetworkingClientHandler.RulesetClientInfo);

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

        protected override void Dispose(bool isDisposing)
        {
            //hostIP.Value = HostIP.Text;
            localIP.Value = Ip.Text;

            base.Dispose(isDisposing);
        }
    }
}
