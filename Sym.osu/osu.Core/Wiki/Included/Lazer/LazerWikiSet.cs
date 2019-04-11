#region usings

using osu.Core.Wiki.Included.Lazer.Sections;
using osu.Core.Wiki.Sections;
using osu.Framework.Graphics.Textures;

#endregion

namespace osu.Core.Wiki.Included.Lazer
{
    public sealed class LazerWikiSet : WikiSet
    {
        public override string Name => "lazer";

        public override string IndexTooltip => "not so official lazer wiki!";

        public override Texture Icon => SymcolOsuModSet.LazerTextures.Get("Menu/logo");

        public override Texture HeaderBackground => SymcolOsuModSet.LazerTextures.Get("Backgrounds/bg1");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new WhatsChanged(),
            new WhatsNew(), 
        };
    }
}
