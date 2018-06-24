using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Mix.Multi;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Mix.Settings
{
    public class MixSettings : SymcolSettingsSubsection
    {
        protected override string Header => "mix!";

        public override RulesetLobbyItem RulesetLobbyItem => mixLobby;

        private readonly MixLobbyItem mixLobby = new MixLobbyItem();

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
