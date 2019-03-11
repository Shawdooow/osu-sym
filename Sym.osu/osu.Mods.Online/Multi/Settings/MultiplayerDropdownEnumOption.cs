﻿using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Screens.Pieces;

namespace osu.Mods.Online.Multi.Settings
{
    public class MultiplayerDropdownEnumOption<T> : MultiplayerOption<T>
        where T : struct
    {
        public readonly Bindable<T> BindableEnum;

        public MultiplayerDropdownEnumOption(OsuNetworkingHandler networking, Bindable<T> bindable, string name, int quadrant, bool sync = true) : base(networking, name, quadrant, sync)
        {
            BindableEnum = bindable;

            OptionContainer.Child = new BetterSettingsEnumDropdown<T>
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Bindable = bindable,
            };

            BindableEnum.ValueChanged += value => SendPacket(new SettingPacket(new Setting<T>
            {
                Name = Title.Text,
                Value = value,
            }));
        }

        protected override void SetValue(SettingPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (Sync && s is Setting<T> setting && setting.Name == Title.Text)
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