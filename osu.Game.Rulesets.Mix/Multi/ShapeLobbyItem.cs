using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Shape;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Shape.Multi
{
    public class ShapeLobbyItem : RulesetLobbyItem
    {
        public override Texture Icon => ShapeRuleset.MixTextures.Get("icon");

        public override string RulesetName => "Mix!";

        //public override Texture Background => ShapeRuleset.ShapeTextures.Get("shape bg");

        public override RulesetLobbyScreen RulesetLobbyScreen => new ShapeLobbyScreen();
    }
}
