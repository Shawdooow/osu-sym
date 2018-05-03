using osu.Framework.Graphics.Textures;
using osu.Game.Users;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Mix.Wiki
{
    public class MixWikiHeader : WikiHeader
    {
        protected override Texture RulesetIcon => MixRuleset.MixTextures.Get("icon");

        //protected override Texture HeaderBackground => MixRuleset.MixTextures.Get("shape bg");

        protected override string RulesetName => "mix";

        protected override string RulesetDescription => "Mix! is a 3rd party ruleset developed for osu!lazer. " +
            "Think of like twenty-four color taiko in a way.";

        protected override string RulesetUrl => $@"https://github.com/Symcol/osu/tree/symcol/osu.Game.Rulesets.Mix";

        protected override User Creator => new User
        {
            Username = "Shawdooow",
            Id = 7726082
        };

        protected override string DiscordInvite => $@"https://discord.gg/JvS5cxA";
    }
}
