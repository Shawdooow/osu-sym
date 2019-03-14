using osu.Framework.Logging;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Packets.Match;
using osu.Mods.Online.Multi.Screens.Pieces;
using osu.Mods.Online.Multi.Settings;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Multi
{
    public sealed class VitaruOnlineCharacterSelection : MultiplayerOption
    {
        private readonly CharacterSelection characterSelection;

        public VitaruOnlineCharacterSelection(OsuNetworkingHandler networking, int quadrant)
            : base(networking, "Character", quadrant, Sync.Client)
        {
            OptionContainer.Child = characterSelection = new CharacterSelection();

            foreach (SettingsDropdown<string> dropdown in characterSelection.CharacterDropdowns)
                dropdown.LabelText = string.Empty;

            characterSelection.SelectedCharacter.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<string>
            {
                Name = Title.Text,
                Value = value,
            }));
        }

        protected override void TriggerBindableChange() => characterSelection.SelectedCharacter.TriggerChange();

        protected override void SetValue(SettingsPacket settings)
        {
            foreach (Setting s in settings.Settings)
                if (s is Setting<string> setting && setting.Name == Title.Text)
                    Logger.Log($"Server just tried to set {setting.Name} which is {setting.Sync}", LoggingTarget.Network, LogLevel.Error);
        }
    }
}
