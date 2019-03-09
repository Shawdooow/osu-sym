using System.Collections.Generic;
using System.Linq;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.VitaruPlayers;
using Symcol.Base.Graphics.Containers;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Settings
{
    /// <summary>
    /// This class is a hack. Make no attempt to use it!
    /// </summary>
    public class CharacterSelection : SymcolContainer
    {
        public readonly List<SettingsDropdown<string>> CharacterDropdowns = new List<SettingsDropdown<string>>();

        private readonly Bindable<string> selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

        private readonly Bindable<string> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

        public CharacterSelection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            AutoSizeDuration = 0;

            foreach (ChapterStore.LoadedGamemode g in ChapterStore.LoadedGamemodes)
            {
                List<string> items = new List<string>();
                foreach (VitaruPlayer player in g.Players)
                    items.Add(player.Name);

                FillFlowContainer character;
                SettingsDropdown<string> characterDropdown;

                Add(character = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 0,
                    Masking = true,

                    Child = characterDropdown = new SettingsDropdown<string>
                    {
                        LabelText = $"Selected {g.Gamemode.Name} Character",
                        Items = items.Distinct().ToList()
                    },
                });

                //TODO: get default from gamemode?
                characterDropdown.Bindable = g.SelectedCharacter;

                bool enabled = false;

                selectedCharacter.ValueChanged += value =>
                {
                    if (enabled)
                        g.SelectedCharacter.Value = items.Contains(selectedCharacter.Value) ? selectedCharacter.Value : g.SelectedCharacter.Default;
                };

                g.SelectedCharacter.ValueChanged += value =>
                {
                    if (enabled)
                        selectedCharacter.Value = value;
                };

                CharacterDropdowns.Add(characterDropdown);

                gamemode.ValueChanged += value =>
                {
                    if (ChapterStore.GetGamemode(value).Name == g.Gamemode.Name)
                    {
                        enabled = true;
                        character.ClearTransforms();
                        character.AutoSizeAxes = Axes.Y;
                    }
                    else
                    {
                        enabled = false;
                        character.ClearTransforms();
                        character.AutoSizeAxes = Axes.None;
                        character.ResizeHeightTo(0, 0, Easing.OutQuint);
                    }

                    selectedCharacter.TriggerChange();
                    g.SelectedCharacter.TriggerChange();
                };
            }

            gamemode.TriggerChange();
        }
    }
}
