﻿using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Core.Wiki.IncludedWikis.Symcol
{
    public class SymcolWikiSet : WikiSet
    {
        public override string Name => "symcol";

        public override string IndexTooltip => "symcol mod loader wiki";

        public override string Description => "The Symcol ModLoader aims to make modding lazer easier by offering APIs to access the core game and make changes without having to learn exactly how osu.Game works.";

        public override Texture Icon => SymcolOsuModSet.SymcolTextures.Get("Symcol@2x");

        public override Texture HeaderBackground => SymcolOsuModSet.SymcolTextures.Get("symcol summer 2018 1080");

        public override WikiSection[] GetSections() => new WikiSection[]
        {

        };
    }
}