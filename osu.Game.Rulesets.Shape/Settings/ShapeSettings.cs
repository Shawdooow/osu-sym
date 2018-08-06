using osu.Framework.Allocation;
using osu.Framework.Platform;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Shape.Settings
{
    public class ShapeSettings : SymcolSettingsSubsection
    {
        protected override string Header => "shape!";

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
