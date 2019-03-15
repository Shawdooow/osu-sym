using System.Collections.Generic;
using System.Linq;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets.Chapters;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Settings
{
    public class ChapterSelection : FillFlowContainer
    {
        public readonly List<SettingsDropdown<string>> ChapterDropdowns = new List<SettingsDropdown<string>>();

        private readonly Bindable<string> selectedChapter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Chapter);

        private readonly Bindable<string> gamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

        public ChapterSelection()
        {
            RelativeSizeAxes = Axes.X;
            AutoSizeAxes = Axes.Y;
            AutoSizeDuration = 0;

            foreach (ChapterStore.LoadedGamemode g in ChapterStore.LoadedGamemodes)
            {
                List<string> items = new List<string>();
                foreach (VitaruChapter chapter in g.Chapters)
                    items.Add(chapter.Title);

                FillFlowContainer character;
                SettingsDropdown<string> chapterDropdown;

                Add(character = new FillFlowContainer
                {
                    Direction = FillDirection.Vertical,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    AutoSizeDuration = 0,
                    Masking = true,

                    Child = chapterDropdown = new SettingsDropdown<string>
                    {
                        LabelText = "Selected Chapter",
                        Items = items.Distinct().ToList()
                    },
                });

                //TODO: get default from gamemode?
                chapterDropdown.Bindable = new Bindable<string> { Default = items.First() };

                selectedChapter.ValueChanged += value =>
                {
                    try { chapterDropdown.Bindable.Value = selectedChapter.Value; }
                    catch { chapterDropdown.Bindable.Value = items.First(); }
                };
                selectedChapter.TriggerChange();

                chapterDropdown.Bindable.ValueChanged += value => selectedChapter.Value = value;
                ChapterDropdowns.Add(chapterDropdown);

                gamemode.ValueChanged += value =>
                {
                    if (ChapterStore.GetGamemode(value).Name == g.Gamemode.Name)
                    {
                        character.ClearTransforms();
                        character.AutoSizeAxes = Axes.Y;
                    }
                    else
                    {
                        character.ClearTransforms();
                        character.AutoSizeAxes = Axes.None;
                        character.ResizeHeightTo(0, 0, Easing.OutQuint);
                    }
                };
            }

            gamemode.TriggerChange();
        }
    }
}
