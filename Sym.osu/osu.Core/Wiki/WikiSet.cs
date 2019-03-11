using osu.Core.Wiki.Sections;
using osu.Framework.Graphics.Textures;
using osuTK;

namespace osu.Core.Wiki
{
    public abstract class WikiSet
    {
        public abstract string Name { get; }

        public virtual string IndexTooltip => "";

        public virtual Texture Icon => null;

        public virtual Texture HeaderBackground => null;

        public virtual Vector2 HeaderBackgroundBlur => new Vector2(5);

        public virtual Creator Creator => null;

        public abstract WikiSection[] GetSections();
    }
}
