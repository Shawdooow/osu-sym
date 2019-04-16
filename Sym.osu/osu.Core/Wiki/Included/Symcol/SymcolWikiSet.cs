#region usings

using osu.Core.Wiki.Included.Symcol.Sections;
using osu.Core.Wiki.Sections;
using osu.Framework.Graphics.Textures;

#endregion

namespace osu.Core.Wiki.Included.Symcol
{
    public class SymcolWikiSet : WikiSet
    {
        public override string Name => "Sym";

        public override string IndexTooltip => "Sym modloader wiki";

        public override Texture Icon => SymManager.SymcolTextures.Get("Symcol@2x");

        public override Texture HeaderBackground => SymManager.SymcolTextures.Get("Symcoltober 1440 2018");

        public override Creator Creator => Creator.Shawdooow;

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new SymcolChangelog()
        };
    }
}
