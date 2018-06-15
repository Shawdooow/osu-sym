using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Shape;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

namespace osu.Game.Rulesets.Shape.Multi
{
    public class ShapeLobbyItem : RulesetLobbyItem
    {
        public override Texture Icon => ShapeRuleset.ShapeTextures.Get("icon@2x");

        public override string RulesetName => "Shape!";

        public override Texture Background => ShapeRuleset.ShapeTextures.Get("shape bg");

        public override RulesetLobbyScreen RulesetLobbyScreen => new ShapeLobbyScreen();
    }
}
