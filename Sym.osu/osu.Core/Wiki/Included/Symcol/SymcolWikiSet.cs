using osu.Core.Wiki.Included.Symcol.Sections;
using osu.Core.Wiki.Sections;
using osu.Framework.Graphics.Textures;

namespace osu.Core.Wiki.Included.Symcol
{
    public class SymcolWikiSet : WikiSet
    {
        public override string Name => "symcol";

        public override string IndexTooltip => "symcol mod loader wiki";

        public override Texture Icon => SymcolOsuModSet.SymcolTextures.Get("Symcol@2x");

        public override Texture HeaderBackground => SymcolOsuModSet.SymcolTextures.Get("Symcoltober 1440 2018");

        public override Creator Creator => Creator.Shawdooow;

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new SymcolChangelog()
        };
    }
}
