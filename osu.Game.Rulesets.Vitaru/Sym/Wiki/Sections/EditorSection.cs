#region usings

using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;
using osu.Framework.Allocation;

#endregion

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections
{
    public class EditorSection : WikiSection
    {
        public override string Title => "Editor";

        [BackgroundDependencyLoader]
        private void load()
        {
            //Bindable<EditorConfiguration> bindable = VitaruSettings.VitaruConfigManager.GetBindable<EditorConfiguration>(VitaruSetting.EditorConfiguration);
            //WikiOptionEnumExplanation<EditorConfiguration> explanation;

            Content.Add(new WikiParagraph("Seeing as the editor is not implemented this section will be used to explain ideas, designs, and features that I would like to see in the editor."));
            Content.Add(new WikiSubSectionHeader("Editor Configuration"));
            Content.Add(new WikiParagraph("The editor will have two main configurations: simple and complex."));
            /*Content.Add(explanation = new WikiOptionEnumExplanation<EditorConfiguration>(bindable));

            bindable.ValueChanged += value =>
            {
                switch (value)
                {
                    case EditorConfiguration.Simple:
                        explanation.Description.Text = "Exactly as it sounds, quickly get mapping with the included patterns used for converts plus a few extras.";
                        break;
                    case EditorConfiguration.Complex:
                        explanation.Description.Text = "Fine control over everything. Includes a custom pattern and bullet maker.";
                        break;
                    case EditorConfiguration.Custom:
                        explanation.Description.Text = "Chose what parts of the complex editor you want avalible and what parts you want hidden. " +
                        "All out complex might be too bloated for some people!";
                        break;
                }
            };
            bindable.TriggerChange();*/
        }
    }
}
