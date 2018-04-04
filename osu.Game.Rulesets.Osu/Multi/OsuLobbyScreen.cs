using Symcol.Rulesets.Core.Multiplayer.Networking;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using System;
using System.Collections.Generic;

namespace osu.Game.Rulesets.Osu.Multi
{
    public class OsuLobbyScreen : RulesetLobbyScreen
    {
        public override string RulesetName => "osu!";

        public override RulesetMatchScreen MatchScreen => new OsuMatchScreen(RulesetNetworkingClientHandler);
    }
}
