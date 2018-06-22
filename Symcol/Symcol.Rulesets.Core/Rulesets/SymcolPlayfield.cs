using OpenTK;
using osu.Game.Rulesets.UI;
using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;

namespace Symcol.Rulesets.Core.Rulesets
{
    public class SymcolPlayfield : Playfield
    {
        public static RulesetNetworkingClientHandler RulesetNetworkingClientHandler;

        public SymcolPlayfield(Vector2 size) : base(size.X, size.Y)
        {
        }
    }
}
