using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Screens.Pieces;
using osuTK;

namespace osu.Mods.Online.Multi.Settings
{
    public class MultiplayerToggleOption : MultiplayerOption<bool>
    {
        public readonly Bindable<bool> BindableBool;

        public MultiplayerToggleOption(OsuNetworkingHandler networking, Bindable<bool> bindable, string name, int quadrant, bool sync = true) : base(networking, name, quadrant, sync)
        {
            BindableBool = bindable;

            Child = new SettingsCheckbox
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Bindable = bindable,
                LabelText = " " + name,
                Position = new Vector2(-16, 24),
            };

            BindableBool.ValueChanged += value => SendPacket(new SettingPacket(new Setting<bool>
            {
                Name = Title.Text,
                Value = value,
            }));
        }

        protected override void SetValue(SettingPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (Sync && s is Setting<bool> setting && setting.Name == Title.Text)
                    BindableBool.Value = setting.Value;
        }
    }
}
