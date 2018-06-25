using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.IncludedWikis.HomeWiki
{
    public sealed class HomeWikiSet : WikiSet
    {
        public override string Name => "Home";

        public override string Description => "Welcome to the new wiki!";

        public override Texture Icon => SymcolOsuModSet.SymcolTextures.Get("Symcol@2x");

        public override Texture HeaderBackground => SymcolOsuModSet.SymcolTextures.Get("symcol spring 2018 1080");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new WhatIsTheWiki()
        };
    }
}
