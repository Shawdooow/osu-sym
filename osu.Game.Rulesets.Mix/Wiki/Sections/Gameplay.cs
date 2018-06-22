using osu.Framework.Allocation;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Mix.Wiki.Sections
{
    public class Gameplay : WikiSection
    {
        public override string Title => "Gameplay";

        [BackgroundDependencyLoader]
        private void load()
        {
            Content.Add(new WikiSubSectionHeader("Hitobject Conversion"));
            Content.Add(new WikiParagraph("Each base Hitsound (there are twelve in osu! by default) gets its own pair of keys (the same soundboard copied to both halves of your keyboard). " +
                "You must successfully hit the note or notes (Easier difficulties will only have 1 - 2 at  a time, harder maps might be like 9 key mania) by hitting the corisponding hitsound button. " +
                "If a hitsound is not within four measures of being used it will be greyed out to help read."));
        }
    }
}
