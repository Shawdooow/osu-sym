using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Mix.Wiki.Sections;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Mix.Wiki
{
    public sealed class MixWikiSet : WikiSet
    {
        public override string Name => "mix!";

        public override string Description => "Mix! is a 3rd party ruleset developed for osu!lazer. " +
                                              "Think of it like twelve color taiko in a way, by default a map will have twelve different hitsounds and each gets its own button.";

        public override string IndexTooltip => "official mix wiki!";

        public override Texture Icon => MixRuleset.MixTextures.Get("icon@2x");

        public override Texture HeaderBackground => MixRuleset.MixTextures.Get("mix BG");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new Gameplay(),
            new ChangelogSection()
        };
    }
}
