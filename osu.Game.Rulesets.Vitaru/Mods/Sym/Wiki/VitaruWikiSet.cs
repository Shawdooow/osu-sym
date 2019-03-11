using osu.Core.Wiki;
using osu.Core.Wiki.Sections;
using osu.Framework.Graphics.Textures;
using osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki
{
    public sealed class VitaruWikiSet : WikiSet
    {
        public override string Name => "vitaru!";

        public override string IndexTooltip => "official vitaru wiki!";

        public override Texture Icon => VitaruRuleset.VitaruTextures.Get("icon@2x");

        public override Texture HeaderBackground => VitaruRuleset.VitaruTextures.Get("Vitarutober 2018");

        public override Creator Creator => Creator.Shawdooow;

        public override WikiSection[] GetSections() => new WikiSection[]
        {
            new GeneralSection(),
            new GamemodeSection(),
            //new ChapterSection(),
            new CharactersSection(),
            //new EditorSection(),
            //new MappingSection(),
            //new MultiplayerSection(),
            //new CodeSection(),
            new CreditsSection(),
            new ChangelogSection()
        };
    }
}
