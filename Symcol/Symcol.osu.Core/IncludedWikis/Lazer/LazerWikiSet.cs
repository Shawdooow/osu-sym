using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.IncludedWikis.Lazer
{
    public sealed class LazerWikiSet : WikiSet
    {
        public override string Name => "lazer";

        public override string Description => "osu!lazer is the complete re-write of osu! and will one day replace osu!stable as the new and improved client!";

        public override string IndexTooltip => "not so official lazer wiki!";

        public override Texture Icon => SymcolOsuModSet.LazerTextures.Get("Menu\\logo");

        public override Texture HeaderBackground => SymcolOsuModSet.LazerTextures.Get("Backgrounds\\bg1");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new WhatIsLazer(), 
        };
    }
}
