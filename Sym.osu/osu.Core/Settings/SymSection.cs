#region usings

using System;
using osu.Core.Config;
using osu.Core.OsuMods;
using osu.Framework.Allocation;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Game.Overlays.Settings;

#endregion

namespace osu.Core.Settings
{
    public sealed class SymSection : SettingsSection
    {
        public override string Header => "Modloader";
        public override IconUsage Icon => FontAwesome.Solid.Bath;

        public static Action<Storage> OnPurge;

        public SymSection()
        {
            OnPurge += storage => { Logger.Log("Purged old or unused files", LoggingTarget.Database, LogLevel.Error); };

            Child = new SymSubSection();

            foreach (OsuModSet mod in OsuModStore.LoadedModSets)
            {
                ModSubSection s = mod.GetSettings();
                if (s != null) Add(s);
            }
        }

        private sealed class SymSubSection : ModSubSection
        {
            protected override string Header => "Sym";

            public SymSubSection()
            {
                Children = new Drawable[]
                {
                    new SettingsTextBox
                    {
                        LabelText = "Player Color (HEX)",
                        Bindable = SymManager.SymConfigManager.GetBindable<string>(SymSetting.PlayerColor)
                    },
                };

                OnPurge += storage =>
                {
                    if (storage.ExistsDirectory("symcol")) storage.DeleteDirectory("symcol");
                };
            }

            [BackgroundDependencyLoader]
            private void load(Storage storage)
            {
                Add(new SettingsButton
                {
                    Text = "Purge",
                    Action = () => OnPurge?.Invoke(storage)
                });
            }
        }
    }
}
