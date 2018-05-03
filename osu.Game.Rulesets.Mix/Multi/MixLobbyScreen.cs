using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Mix.Multi
{
    public class MixLobbyScreen : RulesetLobbyScreen
    {
        public override RulesetMatchScreen MatchScreen => new MixMatchScreen(RulesetNetworkingClientHandler);
    }
}
