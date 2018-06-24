using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki
{
    public class WikiSet
    {
        public virtual string Name => "";

        public virtual Texture Icon => null;

        public virtual Texture HeaderBackground => null;

        public virtual WikiSection[] GetSections() => null;
    }
}
