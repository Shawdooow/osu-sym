using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Overlays.Settings;
using Symcol.Rulesets.Core.Wiki;
using osu.Game.Rulesets.Mix.Wiki;
using Symcol.Rulesets.Core.Multiplayer.Screens;
using osu.Game.Rulesets.Mix.Multi;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Mix.Settings
{
    public class MixSettings : SymcolSettingsSubsection
    {
        protected override string Header => "mix!";

        public override WikiOverlay Wiki => mixWiki;

        private readonly MixWikiOverlay mixWiki = new MixWikiOverlay();

        public override RulesetLobbyItem RulesetLobbyItem => mixLobby;

        private readonly MixLobbyItem mixLobby = new MixLobbyItem();

        public static MixConfigManager MixConfigManager;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            MixConfigManager = new MixConfigManager(host.Storage);

            Children = new Drawable[]
            {
                new SettingsButton
                {
                    Text = "Open In-game Wiki",
                    Action = mixWiki.Show
                }
            };
        }
    }
}
