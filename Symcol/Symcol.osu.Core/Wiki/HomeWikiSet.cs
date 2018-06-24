using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki
{
    public sealed class HomeWikiSet : WikiSet
    {
        public override string Name => "Home";

        public override Texture Icon => SymcolOsuModSet.SymcolTextures.Get("Symcol@2x");

        public override Texture HeaderBackground => SymcolOsuModSet.SymcolTextures.Get("symcol spring 2018 1080");

        public override WikiSection[] GetSections()
        {
            throw new System.NotImplementedException();
        }
    }
}
