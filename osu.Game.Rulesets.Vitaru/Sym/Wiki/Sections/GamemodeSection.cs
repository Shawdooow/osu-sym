#region usings

using System.Collections.Generic;
using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.OptionExplanations;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections
{
    public class GamemodeSection : WikiSection
    {
        public override string Title => "Gamemodes";

        private Bindable<string> selectedGamemode;

        private WikiOptionExplanation<string> gamemodeDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            selectedGamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

            List<string> gamemodeItems = new List<string>();
            foreach (ChapterStore.LoadedChapterSet g in ChapterStore.LoadedChapterSets)
                gamemodeItems.Add(g.ChapterSet.Name);

            Content.Add(new WikiParagraph("This ruleset has multiple gamemodes built in, similar to how Mania can have different key amounts. " +
                "However instead of just increasing the lanes these change how bullets will be coming at you. " +
                "What is the same in all of the gamemodes however, is that you will be dodging bullets to the beat to stay alive."));
            Content.Add(gamemodeDescription = new WikiOptionExplanation<string>(selectedGamemode, gamemodeItems));

            selectedGamemode.ValueChanged += gamemode => { gamemodeDescription.Description.Text = ChapterStore.GetChapterSet(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode)).Description; };
            selectedGamemode.TriggerChange();
        }
    }
}
