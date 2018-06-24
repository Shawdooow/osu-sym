using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace osu.Game.Rulesets.Vitaru.Wiki
{
    public sealed class VitaruWikiSet : WikiSet
    {
        public override string Name => "vitaru!";

        public override string Description => "vitaru! is a 3rd party ruleset for osu!lazer.";

        public override string IndexTooltip => "official vitaru wiki!";

        public override Texture Icon => VitaruRuleset.VitaruTextures.Get("icon@2x");

        public override Texture HeaderBackground => VitaruRuleset.VitaruTextures.Get("vitaru spring 2018");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new General(), 
        };
    }
}
