//Symcol.Rulesets.Core.Multiplayer.Screens.RulesetMultiplayerSelection
//Symcol.Rulesets.Core.Multiplayer.Pieces.MatchTools
#define SymcolMods

using osu.Framework.Allocation;
using osu.Game;
using osu.Game.Overlays.Settings;
using Symcol.Rulesets.Core.Wiki;
#if SymcolMods
#else
using osu.Framework.Logging;
#endif
using osu.Framework.Platform;
using osu.Game.Rulesets;
using Symcol.osu.Core.Screens;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;
using Symcol.Rulesets.Core.Multiplayer.Screens;

namespace Symcol.Rulesets.Core.Rulesets
{
    public abstract class SymcolSettingsSubsection : RulesetSettingsSubsection
    {
        public virtual WikiOverlay Wiki => null;

        public virtual RulesetLobbyItem RulesetLobbyItem => null;

#if SymcolMods
        public static RulesetMultiplayerSelection LegacyRulesetMultiplayerSelection;
        public static Lobby Lobby;
#endif

        public static SymcolConfigManager SymcolConfigManager;

        private OsuGame osu;

        protected SymcolSettingsSubsection(Ruleset ruleset)
            : base(ruleset)
        {
#if SymcolMods
                if (RulesetLobbyItem != null)
                    RulesetMultiplayerSelection.LobbyItems.Add(RulesetLobbyItem);

                if (LegacyRulesetMultiplayerSelection == null)
                    LegacyRulesetMultiplayerSelection = new RulesetMultiplayerSelection();

            if (Lobby == null)
                Lobby = new Lobby();

            SymcolMenu.LegacyRulesetMultiplayerScreen = LegacyRulesetMultiplayerSelection;
            SymcolMenu.Lobby = Lobby;
#else
            Logger.Log("osu.Game mods not installed! Online Multiplayer will not be avalible without them. . .", LoggingTarget.Information, LogLevel.Important);
#endif
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu, Storage storage)
        {
            this.osu = osu;

            if (SymcolConfigManager == null)
                SymcolConfigManager = new SymcolConfigManager(storage);
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            if (Wiki != null)
                osu.Add(Wiki);
        }
    }
}
