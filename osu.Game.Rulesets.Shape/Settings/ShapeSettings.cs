using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Game.Rulesets.Shape.Multi;
using Symcol.Rulesets.Core.LegacyMultiplayer.Screens;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Shape.Settings
{
    public class ShapeSettings : SymcolSettingsSubsection
    {
        protected override string Header => "shape!";

        public override RulesetLobbyItem RulesetLobbyItem => shapeLobby;

        private readonly ShapeLobbyItem shapeLobby = new ShapeLobbyItem();

        public static ShapeConfigManager ShapeConfigManager;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            ShapeConfigManager = new ShapeConfigManager(host.Storage);

            Storage skinsStorage = storage.GetStorageForDirectory("Skins");
        }

        public ShapeSettings(Ruleset ruleset)
            : base(ruleset)
        {
        }
    }
}
