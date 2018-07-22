﻿using System;
using System.Collections.Generic;
using Symcol.Core.LegacyNetworking;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

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
            VitaruNetworkingClientHandler = new VitaruNetworkingClientHandler(ClientType.Host, LocalIp.Text, Int32.Parse(LocalPort.Text));
            RulesetNetworkingClientHandler = VitaruNetworkingClientHandler;
            Add(RulesetNetworkingClientHandler);

            List<ClientInfo> list = new List<ClientInfo>
            {
                RulesetNetworkingClientHandler.RulesetClientInfo
            };

            JoinMatch(list);
        }

        protected override void JoinGame()
        {
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                VitaruNetworkingClientHandler.Dispose();
            }
            VitaruNetworkingClientHandler = new VitaruNetworkingClientHandler(ClientType.Peer, HostIp.Text, Int32.Parse(HostPort.Text));
            VitaruNetworkingClientHandler.OnConnectedToHost += (p) => JoinMatch(p);
            RulesetNetworkingClientHandler = VitaruNetworkingClientHandler;
            Add(RulesetNetworkingClientHandler);
        }
    }
}