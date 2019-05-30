using System.Collections.Generic;
using System.Linq;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Platform;
using osu.Game.Overlays.Settings;
using Symcol.Rulesets.Core.Rulesets;

namespace osu.Game.Rulesets.Classic.Settings
{
    public class ClassicSettings : SymcolSettingsSubsection
    {
        protected override string Header => "Classic!";

        public static ClassicConfigManager ClassicConfigManager;
        private SettingsDropdown<string> skin;
        private Bindable<string> currentSkin;
        private Storage storage;

        [BackgroundDependencyLoader]
        private void load(GameHost host, Storage storage)
        {
            ClassicConfigManager = new ClassicConfigManager(host.Storage);
            this.storage = storage;
            Storage skinsStorage = storage.GetStorageForDirectory("Skins");

            Children = new Drawable[]
            {
                new SettingsCheckbox
                {
                    LabelText = "Dark Sliderbodys",
                    Bindable = ClassicConfigManager.GetBindable<bool>(ClassicSetting.Black)
                },
                new SettingsCheckbox
                {
                    LabelText = "Enable Hold Note",
                    Bindable = ClassicConfigManager.GetBindable<bool>(ClassicSetting.Hold)
                },
                new SettingsCheckbox
                {
                    LabelText = "Enable Accel Mod",
                    Bindable = ClassicConfigManager.GetBindable<bool>(ClassicSetting.Accel)
                },
                new SettingsCheckbox
                {
                    LabelText = "Enable Approaching Mod",
                    Bindable = ClassicConfigManager.GetBindable<bool>(ClassicSetting.Approaching)
                },
                new SettingsEnumDropdown<Easing>
                {
                    LabelText = "Current Slider Easing",
                    Bindable = ClassicConfigManager.GetBindable<Easing>(ClassicSetting.SliderEasing)
                },
                skin = new SettingsDropdown<string>
                {
                    LabelText = "Current Skin"
                },
                new SettingsButton
                {
                    Text = "Open skins folder",
                    Action = skinsStorage.OpenInNativeExplorer,
                }
            };

            List<KeyValuePair<string, string>> items = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("Default", "default") };

            try
            {
                foreach (string skinName in storage.GetDirectories("Skins"))
                {
                    string[] args = skinName.Split('\\');
                    items.Add(new KeyValuePair<string, string>(args.Last(), args.Last()));
                }

                skin.Items = items.Distinct().ToList();
                currentSkin = ClassicConfigManager.GetBindable<string>(ClassicSetting.Skin);
                skin.Bindable = currentSkin;

                currentSkin.ValueChanged += skin => { ClassicConfigManager.Set(ClassicSetting.Skin, skin); };
            }
            catch { }
        }

        public ClassicSettings(Ruleset ruleset)
            : base(ruleset)
        {
        }
    }
}
