using osu.Framework.Allocation;
using osu.Game;
using osu.Game.Overlays.Settings;
using osu.Framework.Platform;
using osu.Game.Rulesets;
using Symcol.osu.Core.Screens;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace Symcol.Rulesets.Core.Rulesets
{
    public abstract class SymcolSettingsSubsection : RulesetSettingsSubsection
    {
        public virtual RulesetLobbyItem RulesetLobbyItem => null;

        public static RulesetMultiplayerSelection LegacyRulesetMultiplayerSelection;
        public static Lobby Lobby;

        public static SymcolConfigManager SymcolConfigManager;

        protected SymcolSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
                if (RulesetLobbyItem != null)
                    RulesetMultiplayerSelection.LobbyItems.Add(RulesetLobbyItem);

                if (LegacyRulesetMultiplayerSelection == null)
                    LegacyRulesetMultiplayerSelection = new RulesetMultiplayerSelection();

            if (Lobby == null)
                Lobby = new Lobby();

            SymcolMenu.LegacyRulesetMultiplayerScreen = LegacyRulesetMultiplayerSelection;
            SymcolMenu.Lobby = Lobby;
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu, Storage storage)
        {
            if (SymcolConfigManager == null)
                SymcolConfigManager = new SymcolConfigManager(storage);
        }
    }
}
