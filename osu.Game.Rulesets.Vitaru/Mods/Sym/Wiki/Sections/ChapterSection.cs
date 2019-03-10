using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Mods.Chaptersets;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections
{
    public class ChapterSection : WikiSection
    {
        public override string Title => "Chapters";



        private WikiParagraph chapterDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            Bindable<string> selectedChapter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Chapter);

            ChapterSelection selection = new ChapterSelection();

            foreach (SettingsDropdown<string> dropdown in selection.ChapterDropdowns)
                dropdown.LabelText = "";

            Content.Add(new WikiParagraph(""));
            Content.Add(new WikiSplitColum(
                new Container
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,

                    Position = new Vector2(-10, 0),

                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,

                    Child = selection,
                },
                chapterDescription = new WikiParagraph("Erm, looks like you found a spooky easter egg! Please go tell Shawdooow you broke something. . .")
                ));

            //basically just an ingame wiki for the chapters
            selectedChapter.ValueChanged += chapter =>
            {
                chapterDescription.Text = ChapterStore.GetChapter(chapter).Description;
            };
            selectedChapter.TriggerChange();
        }
    }
}
