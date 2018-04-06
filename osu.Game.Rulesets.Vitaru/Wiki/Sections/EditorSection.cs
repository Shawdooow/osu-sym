using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Edit;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Rulesets.Core.Wiki;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class EditorSection : WikiSection
    {
        public override string Title => "Editor";

        [BackgroundDependencyLoader]
        private void load()
        {
        }
    }
}
