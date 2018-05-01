using Symcol.Rulesets.Core.Multiplayer.Networking;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Classic.Multi
{
    public class ClassicMatchScreen : RulesetMatchScreen
    {
        public readonly RulesetNetworkingClientHandler ClassicNetworkingClientHandler;

        public ClassicMatchScreen(RulesetNetworkingClientHandler classicNetworkingClientHandler) : base(classicNetworkingClientHandler)
        {
            ClassicNetworkingClientHandler = classicNetworkingClientHandler;
        }
    }
}
