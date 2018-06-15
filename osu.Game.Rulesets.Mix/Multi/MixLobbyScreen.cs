using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

namespace osu.Game.Rulesets.Mix.Multi
{
    public class MixLobbyScreen : RulesetLobbyScreen
    {
        public override RulesetMatchScreen MatchScreen => new MixMatchScreen(RulesetNetworkingClientHandler);
    }
}
