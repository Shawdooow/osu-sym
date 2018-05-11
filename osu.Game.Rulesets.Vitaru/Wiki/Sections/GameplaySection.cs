using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Rulesets.Core.Wiki;
using osu.Framework.Graphics;
using OpenTK;
using osu.Framework.Graphics.Shapes;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers;
using osu.Game.Overlays.Settings;
using System.Collections.Generic;
using System.Linq;
using Symcol.Core.Extentions;
using osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class GameplaySection : WikiSection
    {
        public override string Title => "Gameplay";

        private Bindable<Gamemodes> selectedGamemode;

        private Bindable<string> selectedCharacter;

        private FillFlowContainer vitaruCharacter;
        private SettingsDropdown<string> vitaruCharacterDropdown;
        private Bindable<string> selectedVitaruCharacter;
        private FillFlowContainer touhosuCharacter;
        private SettingsDropdown<string> touhosuCharacterDropdown;
        private Bindable<string> selectedTouhosuCharacter;

        private WikiParagraph characterDescription;

        private Bindable<Mod> selectedMod = new Bindable<Mod> { Default = Mod.Hidden };

        private WikiOptionEnumExplanation<Mod> modsDescription;
        private WikiOptionEnumExplanation<Gamemodes> gamemodeDescription;

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

            Content.Add(new WikiParagraph("Your objective in vitaru is simple, don't get hit by the bullets flying at you, although this is easier said than done."));

            Content.Add(new WikiSubSectionHeader("Converts (Standard => Vitaru)"));
            Content.Add(new WikiParagraph("Circle Size (CS) affects bullet size.\n" +
                        "Accuracy (OD) affects how large the graze box is / how forgiving the score zones are.\n" +
                        "Health Drain (HP) affects nothing atm (will affect how much damage bullets do to you).\n" +
                        "Approach Rate (AR) affects enemy enter + leave speeds.\n" +
                        "Slider Velocity (SV) affects bullet speeds.\n" +
                        "Hitsounds affect the pattern that will be thrown, see the \"Patterns\" subsection for more\n\n" +
                        "Object positions are mapped to the top half of the playfield (or whole playfield for dodge) in the same orientation as standard."));

            Content.Add(new WikiSubSectionHeader("Patterns"));
            Content.Add(new WikiParagraph("Check back later!"));

            Content.Add(new WikiSubSectionHeader("Controls"));
            Content.Add(new WikiParagraph("Controls by default will probably be the most confortable and fitting for all of the gamemodes in this ruleset (if they aren't / weren't they will be changed before release).\n\n" +
                        "W = Move the player up\n" +
                        "S = Down\n" +
                        "A = Left\n" +
                        "D = Right\n" +
                        "Shift = Slow the player to half speed and show the hitbox.\n" +
                        //"Space = Speed up to twice as fast (vitaru gamemode only)\n" +
                        "Left Mouse = Shoot (while in vitaru or touhosu mode)"));

            Content.Add(new WikiSubSectionHeader("Anatomy"));
            Content.Add(new WikiParagraph("Lets get you familiar with the anatomy of the Player first. " +
                        "Unfortunetly I have not had time to implement squishy insides so for now we are just going to go over the basics.\n"));
            Content.Add(new WikiSplitColum(
                new WikiParagraph("On the right we have the Player, I also have revealed the hitbox so I can explain why thats the only part that actually matters. " +
                        "First, see that little white dot with the colored ring in the middle of the player? Thats the hitbox. " +
                        "You only take damage if that white part gets hit, bullets will pass right over the rest of the player without actually harming you in any way, infact that heals you!"),
                new Container
                {
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,

                    Children = new Drawable[]
                    {
                        new Sprite
                        {
                            Scale = new Vector2(2),
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Texture = VitaruRuleset.VitaruTextures.Get("SakuyaIzayoiKiai")
                        },
                        new CircularContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Scale = new Vector2(2),
                            Size = new Vector2(4),
                            BorderThickness = 4 / 3,
                            BorderColour = Color4.Navy,
                            Masking = true,

                            Child = new Box
                            {
                                RelativeSizeAxes = Axes.Both
                            },
                            EdgeEffect = new EdgeEffectParameters
                            {
                                Radius = 4,
                                Type = EdgeEffectType.Shadow,
                                Colour = Color4.Navy.Opacity(0.5f)
                            }
                        }
                    }
                }));
            Content.Add(new WikiSplitColum(
                new CircularContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Scale = new Vector2(2),
                    Size = new Vector2(16),
                    BorderThickness = 16 / 4,
                    BorderColour = Color4.Green,
                    Masking = true,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    EdgeEffect = new EdgeEffectParameters
                    {
                        Radius = 4,
                        Type = EdgeEffectType.Shadow,
                        Colour = Color4.Green.Opacity(0.5f)
                    }
                },
                new WikiParagraph("On the left we have a bullet. Bullets are pretty simple, " +
                "see the white circle in the middle? If that touches the white circle in your hitbox you take damage.")));
            Content.Add(new WikiSplitColum(
                new WikiParagraph("On the right here is a laser. " +
                        "Basically they work like a bullet in that the white rectangle in the middle is the actual dangerous part " +
                        "but unlike a bullet their damage will be spread out for as long as you are getting hit."),
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        Size = new Vector2(200, 40),
                        Masking = true,
                        CornerRadius = 16,
                        BorderThickness = 8,
                        BorderColour = Color4.Aquamarine,

                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    }));
            Content.Add(new WikiSubSectionHeader("Gamemodes"));
            Content.Add(new WikiParagraph("This ruleset has multiple gamemodes built in, similar to how Mania can have different key amounts. " +
                "However instead of just increasing the lanes these change how bullets will be coming at you. " +
                "What is the same in all 3 of the gamemodes however, is that you will be dodging bullets to the beat to stay alive."));
            Content.Add(gamemodeDescription = new WikiOptionEnumExplanation<Gamemodes>(selectedGamemode));
            Content.Add(new WikiSubSectionHeader("Scoring"));
            Content.Add(new WikiParagraph("Score per bullet is based on how close it got to hitting you, the closer a bullet got the more score it will give. The \"Great\" area is about the same size as the green health ring, " +
                "the \"Good\" is twice that and \"Meh\" is infinite (so by default a bullet gives meh unless you got close)."));
            Content.Add(new WikiSubSectionHeader("Mods"));
            Content.Add(new WikiParagraph("Mods affect gameplay just like the other rulesets in the game, but here is how they affect vitaru so you aren't scratching your head trying to figure it out just by playing with it."));
            Content.Add(modsDescription = new WikiOptionEnumExplanation<Mod>(selectedMod));
            Content.Add(new WikiSubSectionHeader("Characters"));
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
                    case"TomajiHakurei":
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
                    "\n-Energy Cost per Second: " + touhosuPlayer.EnergyCostPerSecond +
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
                switch (gamemode)
                {
                    case Gamemodes.Vitaru:
                        gamemodeDescription.Description.Text = "The default gamemode in this ruleset which is based on the touhou series danmaku games. " +
                        "Allows you to kill enemies while dodging bullets to the beat!";
                        break;
                    case Gamemodes.Gravaru:
                        gamemodeDescription.Description.Text = "Gravity Enabled!\n" +
                        "Currently a very incomplete experiance, just messing with gravity physics atm. Stay tuned!";
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

            selectedMod.ValueChanged += mod =>
            {
                switch (mod)
                {
                    default:
                        modsDescription.Description.Text = "Check back later!";
                        break;
                }
            };
            selectedMod.TriggerChange();
        }
    }

    public enum Mod
    {
        Easy,

        HardRock,
        Hidden,
        Flashlight,
        SuddenDeath,
        Perfect,

        Relax
    }
}
