using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Classic.Multi
{
    public class ClassicLobbyScreen : RulesetLobbyScreen
    {
        public override RulesetMatchScreen MatchScreen => new ClassicMatchScreen(RulesetNetworkingClientHandler);
    }
}
