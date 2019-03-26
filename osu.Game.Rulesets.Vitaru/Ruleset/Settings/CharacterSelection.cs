#region usings

using System.Collections.Generic;
using System.Linq;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Settings
{
    /// <summary>
    /// This class is a hack. Make no attempt to use it!
    /// </summary>
    public class CharacterSelection : Container
    {
        public readonly List<SettingsDropdown<string>> CharacterDropdowns = new List<SettingsDropdown<string>>();

        public readonly Bindable<string> SelectedCharacter;

        public CharacterSelection(Bindable<string> mode = null)
        {
            SelectedCharacter = mode != null ?
                VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character).GetUnboundCopy() :
                VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

            Bindable<string> gamemode = mode ?? VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            AutoSizeDuration = 0;

            foreach (ChapterStore.LoadedChapterSet g in ChapterStore.LoadedChapterSets)
            {
                List<string> items = new List<string>();
                foreach (VitaruPlayer player in g.Players)
                    items.Add(player.Name);

                FillFlowContainer characterFlow;
                SettingsDropdown<string> characterDropdown;

                Add(characterFlow = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 0,
                    Masking = true,

                    Child = characterDropdown = new SettingsDropdown<string>
                    {
                        LabelText = $"Selected {g.ChapterSet.Name} Character",
                        Items = items.Distinct().ToList()
                    },
                });

                //TODO: get default from gamemode?
                characterDropdown.Bindable = mode == null ? g.SelectedCharacter : g.SelectedCharacter.GetUnboundCopy();

                bool enabled = false;

                if (mode == null)
                    SelectedCharacter.ValueChanged += value =>
                    {
                        if (enabled)
                            g.SelectedCharacter.Value = items.Contains(SelectedCharacter.Value) ? SelectedCharacter.Value : g.SelectedCharacter.Default;
                    };
                else
                    SelectedCharacter.ValueChanged += value =>
                    {
                        if (enabled)
                            characterDropdown.Bindable.Value = items.Contains(SelectedCharacter.Value) ? SelectedCharacter.Value : characterDropdown.Bindable.Default;
                    };

                if (mode == null)
                    g.SelectedCharacter.ValueChanged += value =>
                    {
                        if (enabled)
                            SelectedCharacter.Value = value;
                    };
                else
                    characterDropdown.Bindable.ValueChanged += value =>
                    {
                        if (enabled)
                            SelectedCharacter.Value = value;
                    };

                CharacterDropdowns.Add(characterDropdown);

                gamemode.ValueChanged += value =>
                {
                    if (ChapterStore.GetChapterSet(value).Name == g.ChapterSet.Name)
                    {
                        enabled = true;
                        characterFlow.ClearTransforms();
                        characterFlow.AutoSizeAxes = Axes.Y;
                    }
                    else
                    {
                        enabled = false;
                        characterFlow.ClearTransforms();
                        characterFlow.AutoSizeAxes = Axes.None;
                        characterFlow.ResizeHeightTo(0, 0, Easing.OutQuint);
                    }

                    SelectedCharacter.TriggerChange();

                    if (mode == null)
                        g.SelectedCharacter.TriggerChange();
                    else
                        characterDropdown.Bindable.TriggerChange();
                };
            }

            gamemode.TriggerChange();
        }
    }
}
