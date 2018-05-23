using osu.Framework.Graphics.Textures;
using osu.Game.Users;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Classic.Wiki
{
    public class ClassicWikiHeader : WikiHeader
    {
        protected override Texture RulesetIcon => ClassicRuleset.ClassicTextures.Get("icon@2x");

        protected override Texture HeaderBackground => ClassicRuleset.ClassicTextures.Get("osu!classic Thumbnail");

        protected override string RulesetName => "classic";

        protected override string RulesetDescription => "classic! is a 3rd party ruleset developed for osu!lazer. It seeks to be an identical copy of osu!stable's osu!standard for everyone who doesn't like the way lazer plays.";

        protected override string RulesetUrl => $@"https://github.com/Symcol/osu/tree/symcol/osu.Game.Rulesets.Classic";

        protected override User Maintainer => new User
        {
            Username = "Shawdooow",
            Id = 7726082
        };

        protected override string DiscordInvite => $@"https://discord.gg/h9Ffde8";
    }
}
