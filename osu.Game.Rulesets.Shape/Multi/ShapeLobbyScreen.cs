using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

namespace osu.Game.Rulesets.Shape.Multi
{
    public class ShapeLobbyScreen : RulesetLobbyScreen
    {
        public override RulesetMatchScreen MatchScreen => new ShapeMatchScreen(RulesetNetworkingClientHandler);
    }
}
