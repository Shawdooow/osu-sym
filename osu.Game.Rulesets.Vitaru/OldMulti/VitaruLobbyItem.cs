using osu.Framework.Graphics.Textures;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;

namespace osu.Game.Rulesets.Vitaru.OldMulti
{
    public class VitaruLobbyItem : RulesetLobbyItem
    {
        public override Texture Icon => VitaruRuleset.VitaruTextures.Get("icon@2x");

        public override string RulesetName => "Vitaru!";

        public override Texture Background => VitaruRuleset.VitaruTextures.Get("vitaru spring 2018");

        public override RulesetLobbyScreen RulesetLobbyScreen => new VitaruLobbyScreen();
    }
}
