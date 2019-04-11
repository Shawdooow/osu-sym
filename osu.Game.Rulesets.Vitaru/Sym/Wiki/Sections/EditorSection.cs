#region usings

using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;
using osu.Framework.Allocation;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections
{
    public class EditorSection : WikiSection
    {
        public override string Title => "Editor";

        [BackgroundDependencyLoader]
        private void load()
        {
            Content.Add(new WikiParagraph("Seeing as the editor is not implemented this section will be used to explain ideas, designs and features that I would like to see in the editor."));
            Content.Add(new WikiSubSectionHeader("Custom Pattern Editor"));
            Content.Add(new WikiParagraph("I think it goes without saying that a ruleset like vitaru NEEDS something like this. "
                                          + "It will likely allow you to create some crazy colorful, beauiful and deadly patterns for use against the players. "
                                          + "It will also almost certainly have a way to easily copy and paste / migrate patterns from one map to another, "
                                          + "this way if you create a pattern so unique you want it to be your signiture you can sign all your maps with it."));
        }
    }
}
