using osu.Framework.Allocation;
using osu.Framework.Platform;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Mix.Settings
{
    public class MixSettings : SymcolSettingsSubsection
    {
        protected override string Header => "mix!";

        public static MixConfigManager MixConfigManager;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            MixConfigManager = new MixConfigManager(host.Storage);
        }

        public MixSettings(Ruleset ruleset)
            : base(ruleset)
        {
        }
    }
}
