#region usings

using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Match.Packets;
using osuTK;

#endregion

namespace osu.Mods.Online.Multi.Settings.Options
{
    public class MultiplayerToggleOption : MultiplayerOption
    {
        public readonly Bindable<bool> BindableBool;

        public MultiplayerToggleOption(OsuNetworkingHandler networking, Bindable<bool> bindable, string name, int quadrant, Sync sync = Sync.All) : base(networking, name, quadrant, sync)
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

            BindableBool.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<bool>
            {
                Name = Title.Text,
                Value = value.NewValue,
                Sync = Sync,
            }));
        }

        protected override void TriggerBindableChange() => BindableBool.TriggerChange();

        protected override void SetValue(SettingsPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (s is Setting<bool> setting && setting.Name == Title.Text)
                    BindableBool.Value = setting.Value;
        }
    }
}
