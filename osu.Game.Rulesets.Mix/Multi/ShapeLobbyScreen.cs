using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Shape.Multi
{
    public class ShapeLobbyScreen : RulesetLobbyScreen
    {
        public override RulesetMatchScreen MatchScreen => new ShapeMatchScreen(RulesetNetworkingClientHandler);
    }
}
