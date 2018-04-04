using osu.Framework.Graphics.Textures;
using osu.Game.Graphics;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace osu.Game.Rulesets.Osu.Multi
{
    public class OsuLobbyItem : RulesetLobbyItem
    {
        //public override Texture Icon => new SpriteIcon { Icon = FontAwesome.fa_osu_osu_o };

        public override string RulesetName => "osu!";

        //public override Texture Background => ShapeRuleset.ShapeTextures.Get("VitaruTouhosuModeTrue2560x1440");

        public override RulesetLobbyScreen RulesetLobbyScreen => new OsuLobbyScreen();
    }
}
