﻿using Symcol.osu.Core.Wiki;
using Symcol.osu.Core.Wiki.Sections;

namespace Symcol.osu.Mods.CasterBible
{
    public class CasterWikiSet : WikiSet
    {
        public override string Name => "Caster Bible";

        public override string Description => "The caster bible aims to make the caster's lives easier by making it much easier to archive and share important information about tournements.";

        public override string IndexTooltip => "the caster bible wiki!";

        public override WikiSection[] GetSections() => null;
    }
}
