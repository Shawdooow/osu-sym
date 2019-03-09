using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Screens.Pieces;

namespace osu.Mods.Online.Multi.Settings
{
    public class MultiplayerDropdownOption<T> : MultiplayerOption<T>
    {
        public readonly Bindable<T> Bindable;

        public MultiplayerDropdownOption(OsuNetworkingHandler networking, Bindable<T> bindable, string name, int quadrant, bool sync = true) : base(networking, name, quadrant, sync)
        {
            Bindable = bindable;

            OptionContainer.Child = new BetterSettingsDropdown<T>
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Bindable = bindable,
            };

            Bindable.ValueChanged += value => SendPacket(new SettingPacket(new Setting<T>
            {
                Name = Title.Text,
                Value = value,
            }));
        }

        protected override void SetValue(SettingPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (Sync && s is Setting<T> setting && setting.Name == Title.Text)
                    Bindable.Value = setting.Value;
        }

        private class BetterSettingsDropdown<J> : SettingsDropdown<J>
        {
            protected override OsuDropdown<J> CreateDropdown() => new BetterOsuDropdown<J>
            {
                Margin = new MarginPadding { Top = 5 },
                RelativeSizeAxes = Axes.X,
            };

            private class BetterOsuDropdown<I> : OsuDropdown<I>
            {
                public BetterOsuDropdown()
                {
                    Menu.MaxHeight = 160;
                }
            }
        }
    }
}
