using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Match.Packets;
using osu.Mods.Online.Multi.Settings;
using osu.Mods.Online.Multi.Settings.Options;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Multi
{
    public sealed class VitaruOnlineGamemodeSelection : MultiplayerOption
    {
        public readonly Bindable<string> Gamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode).GetUnboundCopy();

        public VitaruOnlineGamemodeSelection(OsuNetworkingHandler networking, int quadrant)
            : base(networking, "Gamemode", quadrant)
        {
            List<string> gamemodeItems = new List<string>();
            foreach (ChapterStore.LoadedGamemode g in ChapterStore.LoadedGamemodes)
                gamemodeItems.Add(g.Gamemode.Name);

            OptionContainer.Child = new SettingsDropdown<string>
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.TopLeft,
                RelativeSizeAxes = Axes.X,
                Items = gamemodeItems,
                Bindable = Gamemode,
            };

            Gamemode.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<string>
            {
                Name = Title.Text,
                Value = value,
                Sync = Sync,
            }));
        }

        protected override void TriggerBindableChange() => Gamemode.TriggerChange();

        protected override void SetValue(SettingsPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (s is Setting<string> setting && setting.Name == Title.Text)
                    Gamemode.Value = setting.Value;
        }
    }
}
