using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

namespace osu.Game.Rulesets.Shape.Multi
{
    public class ShapeMatchScreen : RulesetMatchScreen
    {
        public readonly RulesetNetworkingClientHandler ShapeNetworkingClientHandler;

        public ShapeMatchScreen(RulesetNetworkingClientHandler shapeNetworkingClientHandler) : base(shapeNetworkingClientHandler)
        {
            ShapeNetworkingClientHandler = shapeNetworkingClientHandler;
        }
    }
}
