using OpenTK;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Core.Extentions;
using Symcol.Rulesets.Core.Wiki;
using System.Collections.Generic;
using System.Linq;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class CharactersSection : WikiSection
    {
        public override string Title => "Characters";

        private Bindable<Gamemodes> selectedGamemode;

        private Bindable<string> selectedCharacter;

        private FillFlowContainer vitaruCharacter;
        private SettingsDropdown<string> vitaruCharacterDropdown;
        private Bindable<string> selectedVitaruCharacter;
        private FillFlowContainer touhosuCharacter;
        private SettingsDropdown<string> touhosuCharacterDropdown;
        private Bindable<string> selectedTouhosuCharacter;

        private WikiParagraph characterDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            selectedGamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);
            selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

            selectedVitaruCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.VitaruCharacter);
            selectedTouhosuCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.TouhosuCharacter);

            List<KeyValuePair<string, string>> vitaruItems = new List<KeyValuePair<string, string>>();
            foreach (VitaruCharacters character in System.Enum.GetValues(typeof(VitaruCharacters)))
                vitaruItems.Add(new KeyValuePair<string, string>(character.GetDescription(), character.ToString()));
            selectedVitaruCharacter.ValueChanged += character => { VitaruSettings.VitaruConfigManager.Set(VitaruSetting.VitaruCharacter, character); VitaruSettings.VitaruConfigManager.Set(VitaruSetting.Character, character); };

            List<KeyValuePair<string, string>> touhosuItems = new List<KeyValuePair<string, string>>();
            foreach (TouhosuCharacters character in System.Enum.GetValues(typeof(TouhosuCharacters)))
                touhosuItems.Add(new KeyValuePair<string, string>(character.GetDescription(), character.ToString()));
            selectedTouhosuCharacter.ValueChanged += character => { VitaruSettings.VitaruConfigManager.Set(VitaruSetting.TouhosuCharacter, character); VitaruSettings.VitaruConfigManager.Set(VitaruSetting.Character, character); };

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

                    Children = new Drawable[]
                    {
                        vitaruCharacter = new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            AutoSizeDuration = 0,
                            AutoSizeEasing = Easing.OutQuint,
                            Masking = true,

                            Child = vitaruCharacterDropdown = new SettingsDropdown<string>
                            {
                                Items = vitaruItems.Distinct().ToList()
                            },
                        },
                        touhosuCharacter = new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,
                            AutoSizeDuration = 0,
                            AutoSizeEasing = Easing.OutQuint,
                            Masking = true,

                            Child = touhosuCharacterDropdown = new SettingsDropdown<string>
                            {
                                Items = touhosuItems.Distinct().ToList()
                            },
                        },
                    }
                },
                characterDescription = new WikiParagraph("Erm, looks like you found a spooky easter egg! Please go tell Shawdooow you broke something. . .")
                ));

            vitaruCharacterDropdown.Bindable = selectedVitaruCharacter;
            touhosuCharacterDropdown.Bindable = selectedTouhosuCharacter;

            //basically just an ingame wiki for the characters
            selectedCharacter.ValueChanged += character =>
            {
                VitaruPlayer vitaruPlayer = new VitaruPlayer();
                TouhosuPlayer touhosuPlayer = new TouhosuPlayer();

                switch (character)
                {
                    case "Alex":
                        vitaruPlayer = new Alex();
                        break;
                    case "ReimuHakurei":
                        touhosuPlayer = new Reimu();
                        break;
                    case "RyukoyHakurei":
                        touhosuPlayer = new Ryukoy();
                        break;
                    case "TomajiHakurei":
                        touhosuPlayer = new Tomaji();
                        break;
                    case "SakuyaIzayoi":
                        touhosuPlayer = new Sakuya();
                        break;
                }

                string stats = vitaruPlayer.Background;

                if (selectedGamemode.Value == Gamemodes.Touhosu)
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
                    "\n-Spell: " + touhosuPlayer.Spell +
                    "\n\nCharacter Background:\n\n" + touhosuPlayer.Background;
                }


                characterDescription.Text = stats;
            };
            selectedCharacter.TriggerChange();

            selectedGamemode.ValueChanged += gamemode =>
            {
                if (gamemode == Gamemodes.Touhosu)
                {
                    touhosuCharacter.ClearTransforms();
                    touhosuCharacter.AutoSizeAxes = Axes.Y;

                    vitaruCharacter.ClearTransforms();
                    vitaruCharacter.AutoSizeAxes = Axes.None;
                    vitaruCharacter.ResizeHeightTo(0, 0, Easing.OutQuint);

                    selectedCharacter.Value = selectedTouhosuCharacter.Value;
                }
                else
                {
                    vitaruCharacter.ClearTransforms();
                    vitaruCharacter.AutoSizeAxes = Axes.Y;

                    touhosuCharacter.ClearTransforms();
                    touhosuCharacter.AutoSizeAxes = Axes.None;
                    touhosuCharacter.ResizeHeightTo(0, 0, Easing.OutQuint);

                    selectedCharacter.Value = selectedVitaruCharacter.Value;
                }
            };
            selectedGamemode.TriggerChange();
        }
    }
}
