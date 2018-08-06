using osu.Framework.Allocation;
using osu.Game;
using osu.Game.Overlays.Settings;
using osu.Framework.Platform;
using osu.Game.Rulesets;
using Symcol.osu.Core;
using Symcol.osu.Core.Config;

namespace Symcol.Rulesets.Core.Rulesets
{
    public abstract class SymcolSettingsSubsection : RulesetSettingsSubsection
    {
        protected SymcolSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu, Storage storage)
        {
            if (SymcolOsuModSet.SymcolConfigManager == null)
                SymcolOsuModSet.SymcolConfigManager = new SymcolConfigManager(storage);
        }
    }
}
