using osu.Game.Rulesets.Vitaru.Wiki.Sections;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki
{
    public class VitaruWikiOverlay : WikiOverlay
    {
        protected override WikiHeader Header => new VitaruWikiHeader();

        protected override WikiSection[] Sections => new WikiSection[]
        {
            new GeneralSection(),
            new GamemodeSection(),
            new CharactersSection(),
            new EditorSection(),
            new MappingSection(),
            new MultiplayerSection(),
            new CodeSection(),
            new CreditsSection(),
            new ChangelogSection()
        };
    }
}
