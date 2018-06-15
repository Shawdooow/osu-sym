using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

namespace osu.Game.Rulesets.Mix.Multi
{
    public class MixMatchScreen : RulesetMatchScreen
    {
        public readonly RulesetNetworkingClientHandler MixNetworkingClientHandler;

        public MixMatchScreen(RulesetNetworkingClientHandler mixNetworkingClientHandler) : base(mixNetworkingClientHandler)
        {
            MixNetworkingClientHandler = mixNetworkingClientHandler;
        }
    }
}
