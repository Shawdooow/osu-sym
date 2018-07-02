using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Classic.Wiki
{
    public sealed class ClassicWikiSet : WikiSet
    {
        public override string Name => "classic!";

        public override string Description => "classic! is a 3rd party ruleset developed for osu!lazer. It seeks to be an identical copy of osu!stable's osu!standard for everyone who doesn't like the way lazer plays.";

        public override string IndexTooltip => "official classic wiki!";

        public override Texture Icon => ClassicRuleset.ClassicTextures.Get("icon@2x");

        public override Texture HeaderBackground => ClassicRuleset.ClassicTextures.Get("osu!classic Thumbnail");

        public override WikiSection[] GetSections() => new WikiSection[]
        {

        };
    }
}
