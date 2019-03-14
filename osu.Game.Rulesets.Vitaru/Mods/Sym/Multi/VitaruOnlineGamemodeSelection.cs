using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Mods.Chaptersets;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Screens.Pieces;
using osu.Mods.Online.Multi.Settings;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Multi
{
    public sealed class VitaruOnlineGamemodeSelection : MultiplayerOption
    {
        private readonly Bindable<string> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

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
                Bindable = gamemode,
            };

            gamemode.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<string>
            {
                Name = Title.Text,
                Value = value,
            }));
        }

        protected override void TriggerBindableChange() => gamemode.TriggerChange();

        protected override void SetValue(SettingsPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (s is Setting<string> setting && setting.Name == Title.Text)
                    gamemode.Value = setting.Value;
        }
    }
}
