using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Shape.Wiki.Sections;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Shape.Wiki
{
    public sealed class ShapeWikiSet : WikiSet
    {
        public override string Name => "shape!";

        public override string Description => "shape! is a 3rd party ruleset developed for osu!lazer. " +
                                              "Think of like four color taiko in a way, if the objects were placed like osu! objects but with a harder approach animation to read.";

        public override string IndexTooltip => "official shape wiki!";

        public override Texture Icon => ShapeRuleset.ShapeTextures.Get("icon@2x");

        public override Texture HeaderBackground => ShapeRuleset.ShapeTextures.Get("shape bg");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new Gameplay(), 
        };
    }
}
