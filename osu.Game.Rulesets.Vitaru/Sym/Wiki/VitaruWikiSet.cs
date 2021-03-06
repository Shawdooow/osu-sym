﻿#region usings

using osu.Core.Wiki;
using osu.Core.Wiki.Sections;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Wiki
{
    public sealed class VitaruWikiSet : WikiSet
    {
        public override string Name => "vitaru";

        public override string IndexTooltip => "Official vitaru wiki!";

        public override Texture Icon => VitaruRuleset.VitaruTextures.Get("icon@2x");

        public override Texture HeaderBackground => VitaruRuleset.VitaruTextures.Get("Vitarutober 2018");

        public override Creator Creator => Creator.Shawdooow;

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new GeneralSection(),
            new GamemodeSection(),
            new CharactersSection(),
            new EditorSection(),
            new MappingSection(),
            new MultiplayerSection(),
            new CreditsSection(),
            new ChangelogSection()
        };
    }
}
