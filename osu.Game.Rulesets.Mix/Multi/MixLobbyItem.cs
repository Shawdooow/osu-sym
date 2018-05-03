using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Mix;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Mix.Multi
{
    public class MixLobbyItem : RulesetLobbyItem
    {
        public override Texture Icon => MixRuleset.MixTextures.Get("icon@2x");

        public override string RulesetName => "Mix!";

        public override Texture Background => MixRuleset.MixTextures.Get("mix BG");

        public override RulesetLobbyScreen RulesetLobbyScreen => new MixLobbyScreen();
    }
}
