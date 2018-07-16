using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.osu.Core.Wiki.Sections;
using Symcol.osu.Core.Wiki.Sections.OptionExplanations;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;

namespace osu.Game.Rulesets.Vitaru.SymcolMods.Wiki.Sections
{
    public class GamemodeSection : WikiSection
    {
        public override string Title => "Gamemodes";

        private Bindable<Gamemodes> selectedGamemode;

        private WikiOptionEnumExplanation<Gamemodes> gamemodeDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            selectedGamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.Gamemode);

            Content.Add(new WikiParagraph("This ruleset has multiple gamemodes built in, similar to how Mania can have different key amounts. " +
                "However instead of just increasing the lanes these change how bullets will be coming at you. " +
                "What is the same in all 3 of the gamemodes however, is that you will be dodging bullets to the beat to stay alive."));
            Content.Add(gamemodeDescription = new WikiOptionEnumExplanation<Gamemodes>(selectedGamemode));

            selectedGamemode.ValueChanged += gamemode =>
            {
                switch (gamemode)
                {
                    case Gamemodes.Vitaru:
                        gamemodeDescription.Description.Text = "The default gamemode in this ruleset which is based on the touhou series danmaku games. " +
                        "Allows you to kill enemies while dodging bullets to the beat!";
                        break;
                    case Gamemodes.Dodge:
                        gamemodeDescription.Description.Text = "Completly changes how vitaru is played. " +
                        "The Dodge gamemode changes the playfield to a much shorter rectangle and send bullets your way from all directions while also taking away your ability to shoot!";
                        break;
                    case Gamemodes.Touhosu:
                        gamemodeDescription.Description.Text = "The \"amplified\" gamemode. Touhosu mode is everything base Vitaru is and so much more. " +
                        "Selecting different characters no longer just changes your skin but also your stats and your spell!";
                        break;
                }
            };
            selectedGamemode.TriggerChange();
        }
    }
}
