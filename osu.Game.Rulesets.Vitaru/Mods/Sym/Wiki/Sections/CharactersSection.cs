using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Mods.ChapterSets;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Mods.Sym.Wiki.Sections
{
    public class CharactersSection : WikiSection
    {
        public override string Title => "Characters";

        private Bindable<string> gamemode;

        private Bindable<string> selectedCharacter;

        private WikiParagraph characterDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            gamemode = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Gamemode);

            selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

            CharacterSelection selection = new CharacterSelection();

            foreach (SettingsDropdown<string> dropdown in selection.CharacterDropdowns)
                dropdown.LabelText = "";

            Content.Add(new WikiParagraph("Selecting a different character is purely cosmetic, except in Touhosu mode. " +
                "In touhosu mode your stats will also be changed and are listed here when set to touhosu mode."));
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
                characterDescription = new WikiParagraph("Erm, looks like you found a spooky easter egg! Please go tell Shawdooow you broke something. . .")
                ));

            Bindable<int> souls = VitaruSettings.VitaruConfigManager.GetBindable<int>(VitaruSetting.Souls);

            if (souls < 100)
                souls.ValueChanged += value =>
                {
                    if (souls == 100)
                        selectedCharacter.TriggerChange();
                };

            //basically just an ingame wiki for the characters
            selectedCharacter.ValueChanged += character =>
            {
                VitaruPlayer vitaruPlayer = ChapterStore.GetPlayer(character);

                string stats = vitaruPlayer.Background;

                if (ChapterStore.GetGamemode(gamemode) is TouhosuGamemode && vitaruPlayer is TouhosuPlayer touhosuPlayer)
                {
                    stats = "";

                    if (!touhosuPlayer.Implemented)
                        stats = "WARNING: Character not marked as Implemented! (Character may be incomplete)\n\n\n";

                    stats = stats + "Stats:\n\n" +
                    "-Health: " + touhosuPlayer.MaxHealth +
                    "\n-Energy: " + touhosuPlayer.MaxEnergy +
                    "\n-Energy Cost: " + touhosuPlayer.EnergyCost +
                    "\n-Energy Drain Rate: " + touhosuPlayer.EnergyDrainRate +
                    "\n-Role: " + touhosuPlayer.Role +
                    "\n-Difficulty: " + touhosuPlayer.Difficulty +
                    "\n-Ability: " + touhosuPlayer.Ability;

                    if (touhosuPlayer.AbilityStats != null)
                        stats = stats + "\n" + touhosuPlayer.AbilityStats;

                    if (souls.Value >= 100)
                        stats = stats + "\n\n" + touhosuPlayer.Background;
                }

                characterDescription.Text = stats;
            };
            selectedCharacter.TriggerChange();
        }
    }
}
