#region usings

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Match.Packets;

#endregion

namespace osu.Mods.Online.Multi.Settings.Options
{
    public class MultiplayerDropdownOption<T> : MultiplayerOption
    {
        public readonly Bindable<T> Bindable;

        public MultiplayerDropdownOption(OsuNetworkingHandler networking, Bindable<T> bindable, string name, int quadrant, Sync sync = Sync.All) : base(networking, name, quadrant, sync)
        {
            Bindable = bindable;

            OptionContainer.Child = new BetterSettingsDropdown<T>
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Bindable = bindable,
            };

            Bindable.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<T>
            {
                Name = Title.Text,
                Value = value.NewValue,
                Sync = Sync,
            }));
        }

        protected override void TriggerBindableChange() => Bindable.TriggerChange();

        protected override void SetValue(SettingsPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (s is Setting<T> setting && setting.Name == Title.Text)
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
