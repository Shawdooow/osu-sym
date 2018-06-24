﻿using osu.Framework.Graphics.Textures;
using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Mods.CasterBible.Wiki
{
    public class CasterWikiSet : WikiSet
    {
        public override string Name => "Caster Bible";

        public override string Description => "The caster bible aims to make the casters' lives easier by making it much easier to archive and share important information about tournements.";

        public override string IndexTooltip => "the caster bible wiki!";

        public override Texture Icon => CasterBibleModSet.CasterTextures.Get("Casters icon 1080");

        public override Texture HeaderBackground => CasterBibleModSet.CasterTextures.Get("Casters");

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new Teams(),
            new Maps(),
            new Results(),
            new Credits(), 
        };
    }
}
