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
            Content.Add(new WikiParagraph("Check back later!"));
        }
    }
}
