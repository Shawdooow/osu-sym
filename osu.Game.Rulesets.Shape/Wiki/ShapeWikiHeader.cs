using osu.Framework.Graphics.Textures;
using osu.Game.Users;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Shape.Wiki
{
    public class ShapeWikiHeader : WikiHeader
    {
        protected override Texture RulesetIcon => ShapeRuleset.ShapeTextures.Get("shape@2x");

        protected override Texture HeaderBackground => ShapeRuleset.ShapeTextures.Get("shape bg");

        protected override string RulesetName => "shape";

        protected override string RulesetDescription => "shape! is a 3rd party ruleset developed for osu!lazer. " +
            "Think of like four color taiko in a way, if the objects were placed like osu! objects but with a harder approach animation to read.";

        protected override string RulesetUrl => $@"https://github.com/Symcol/osu/tree/symcol/osu.Game.Rulesets.Shape";

        protected override User Creator => new User
        {
            Username = "Shawdooow",
            Id = 7726082
        };

        protected override string DiscordInvite => $@"https://discord.gg/JvS5cxA";
    }
}
