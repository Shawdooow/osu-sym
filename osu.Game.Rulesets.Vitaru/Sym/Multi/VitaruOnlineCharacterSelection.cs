#region usings

using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Framework.Logging;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Multi.Match.Packets;
using osu.Mods.Online.Multi.Settings;
using osu.Mods.Online.Multi.Settings.Options;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Multi
{
    public sealed class VitaruOnlineCharacterSelection : MultiplayerOption
    {
        private readonly CharacterSelection characterSelection;

        public VitaruOnlineCharacterSelection(OsuNetworkingHandler networking, int quadrant, Bindable<string> gamemode)
            : base(networking, "Character", quadrant, Sync.Client)
        {
            OptionContainer.Child = characterSelection = new CharacterSelection(gamemode);

            foreach (SettingsDropdown<string> dropdown in characterSelection.CharacterDropdowns)
                dropdown.LabelText = string.Empty;

            characterSelection.SelectedCharacter.ValueChanged += value => SendPacket(new SettingsPacket(new Setting<string>
            {
                Name = Title.Text,
                Value = value.NewValue,
                Sync = Sync,
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
