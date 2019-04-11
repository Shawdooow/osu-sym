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
    public class MultiplayerDropdownEnumOption<T> : MultiplayerOption
        where T : struct
    {
        public readonly Bindable<T> BindableEnum;

        public MultiplayerDropdownEnumOption(OsuNetworkingHandler networking, Bindable<T> bindable, string name, int quadrant, Sync sync = Sync.All) : base(networking, name, quadrant, sync)
        {
            BindableEnum = bindable;

            OptionContainer.Child = new BetterSettingsEnumDropdown<T>
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Bindable = bindable,
            };

            BindableEnum.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<T>
            {
                Name = Title.Text,
                Value = value.NewValue,
                Sync = Sync,
            }));
        }

        protected override void TriggerBindableChange() => BindableEnum.TriggerChange();

        protected override void SetValue(SettingsPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (s is Setting<T> setting && setting.Name == Title.Text)
                    BindableEnum.Value = setting.Value;
        }

        private class BetterSettingsEnumDropdown<J> : SettingsEnumDropdown<J>
            where J : struct
        {
            protected override OsuDropdown<J> CreateDropdown() => new BetterOsuEnumDropdown<J>
            {
                Margin = new MarginPadding { Top = 5 },
                RelativeSizeAxes = Axes.X,
            };

            private class BetterOsuEnumDropdown<I> : OsuEnumDropdown<I>
                where I : struct
            {
                public BetterOsuEnumDropdown()
                {
                    Menu.MaxHeight = 160;
                }
            }
        }
    }
}
