using osu.Core.Config;
using osu.Core.OsuMods;
using osu.Framework.Graphics;
using osu.Game.Graphics;
using osu.Game.Overlays.Settings;

namespace osu.Core.Settings
{
    public class SymSection : SettingsSection
    {
        public override string Header => "Modloader";
        public override FontAwesome Icon => FontAwesome.fa_bathtub;

        public SymSection()
        {
            Child = new SymSubSection();

            foreach (OsuModSet mod in OsuModStore.LoadedModSets)
            {
                ModSubSection s = mod.GetSettings();
                if (s != null) Add(s);
            }
        }

        private class SymSubSection : ModSubSection
        {
            protected override string Header => "Sym";

            public SymSubSection()
            {
                Children = new Drawable[]
                {
                    new SettingsEnumDropdown<AutoJoin>
                    {
                        LabelText = "Connect on launch?",
                        Bindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<AutoJoin>(SymcolSetting.Auto)
                    },
                    new SettingsTextBox
                    {
                        LabelText = "Player Color (HEX)",
                        Bindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.PlayerColor)
                    },
                };
            }
        }
    }
}
