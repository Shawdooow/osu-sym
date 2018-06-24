using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki
{
    public abstract class WikiSet
    {
        public abstract string Name { get; }

        public virtual Texture Icon => null;

        public virtual Texture HeaderBackground => null;

        public abstract WikiSection[] GetSections();
    }
}
