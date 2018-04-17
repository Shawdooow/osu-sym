using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.Settings;
using Symcol.Rulesets.Core.Wiki;
using osu.Framework.Graphics;
using osu.Game.Graphics.Containers;
using OpenTK;
using osu.Game.Rulesets.Vitaru.Wiki.Sections.Pieces;
using osu.Framework.Graphics.Shapes;
using OpenTK.Graphics;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Vitaru.Scoring;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Players;

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class GameplaySection : WikiSection
    {
        public override string Title => "Gameplay";

        private Bindable<VitaruGamemode> selectedGamemode;
        private Bindable<ScoringMetric> selectedScoring;
        private Bindable<SelectableCharacters> selectedCharacter;

        private Bindable<bool> familiar;
        private Bindable<bool> late;
        private Bindable<bool> lastDance;
        private Bindable<bool> insane;
        private Bindable<bool> awoken;
        private Bindable<bool> sacred;
        private Bindable<bool> bonded;
        private Bindable<bool> resurrected;

        private WikiOptionEnumExplanation<SelectableCharacters> characterDescription;

        private Bindable<Mod> selectedMod = new Bindable<Mod> { Default = Mod.Hidden };

        private WikiOptionEnumExplanation<Mod> modsDescription;
        private WikiOptionEnumExplanation<VitaruGamemode> gamemodeDescription;
        private WikiOptionEnumExplanation<ScoringMetric> scoringDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            selectedGamemode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);
            selectedScoring = VitaruSettings.VitaruConfigManager.GetBindable<ScoringMetric>(VitaruSetting.ScoringMetric);
            selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<SelectableCharacters>(VitaruSetting.Characters);

            familiar = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Familiar);
            late = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Late);
            lastDance = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.LastDance);
            insane = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Insane);
            awoken = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Awoken);
            sacred = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Sacred);
            bonded = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Bonded);
            resurrected = VitaruSettings.VitaruConfigManager.GetBindable<bool>(VitaruSetting.Resurrected);

            Content.Add(new WikiParagraph("Your objective in vitaru is simple, don't get hit by the bullets flying at you, although this is easier said than done."));

            Content.Add(new WikiSubSectionHeader("Converts - Difficulty"));
            Content.Add(new WikiParagraph("The way vitaru converts standard maps to vitaru maps.\n\n" +
                        "Circle Size (CS) affects bullet size.\n" +
                        "Accuracy (OD) affects how large the graze box is / how forgiving the score zones are.\n" +
                        "Health Drain (HP) affects nothing atm (will affect how much damage bullets do to you).\n" +
                        "Approach Rate (AR) affects enemy enter + leave speeds.\n\n" +
                        "Object positions are mapped to the top half of the playfield (or whole playfield for dodge) in the same orientation as standard."));

            Content.Add(new WikiSubSectionHeader("Controls"));
            Content.Add(new WikiParagraph("Controls by default will probably be the most confortable and fitting for all of the gamemodes in this ruleset (if they aren't / weren't they will be changed before release).\n\n" +
                        "W = Move the player up\n" +
                        "S = Down\n" +
                        "A = Left\n" +
                        "D = Right\n" +
                        "Shift = Slow the player to half speed and show the hitbox.\n" +
                        //"Space = Speed up to twice as fast (vitaru gamemode only)\n" +
                        "Left Mouse = Shoot (while in vitaru or touhosu mode)\n" +
                        "Right mouse = Spell (while in touhosu mode)\n\n" +
                        "Some individual character's spells will use additional binds, those will be listed in their spell's description under the \"Characters\" section."));

            Content.Add(new WikiSubSectionHeader("Anatomy"));
            Content.Add(new WikiParagraph("Lets get you familiar with the anatomy of the Player first. " +
                        "Unfortunetly I have not had time to implement squishy insides so for now we are just going to go over the basics.\n"));
            Content.Add(new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,

                Children = new Drawable[]
                {
                    new Anatomy
                    {
                        Position = new Vector2(-20, 0),
                    },
                    new OsuTextFlowContainer(t => { t.TextSize = 20; })
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Width = 400,
                        AutoSizeAxes = Axes.Y,
                        Text = "On the right we have the Player, I also have revealed the hitbox so I can explain why thats the only part that actually matters. " +
                        "First, see that little white dot with the colored ring in the middle of the player? Thats the hitbox. " +
                        "You only take damage if that white part gets hit, bullets will pass right over the rest of the player without actually harming you in any way, infact that heals you!\n"
                    }
                }
            });
            Content.Add(new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,

                Children = new Drawable[]
                {
                    //Just a bullet
                    new CircularContainer
                    {
                        Position = new Vector2(20, 0),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
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
                    new OsuTextFlowContainer(t => { t.TextSize = 20; })
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Width = 400,
                        AutoSizeAxes = Axes.Y,
                        Text = "On the left we have a bullet. Bullets are pretty simple, see the white circle in the middle? If that touches the white circle in your hitbox you take damage.\n"
                    }
                }
            });
            Content.Add(new Container
            {
                Anchor = Anchor.TopCentre,
                Origin = Anchor.TopCentre,
                AutoSizeAxes = Axes.Y,
                RelativeSizeAxes = Axes.X,

                Children = new Drawable[]
                {
                    new Container
                    {
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreRight,
                        Position = new Vector2(-80, 0),
                        Size = new Vector2(200, 40),
                        Masking = true,
                        CornerRadius = 16,
                        BorderThickness = 8,
                        BorderColour = Color4.Aquamarine,

                        Child = new Box
                        {
                            RelativeSizeAxes = Axes.Both
                        }
                    },
                    new OsuTextFlowContainer(t => { t.TextSize = 20; })
                    {
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreLeft,
                        Width = 400,
                        AutoSizeAxes = Axes.Y,
                        Text = "On the right here is a laser. " +
                        "Basically they work like a bullet in that the white rectangle in the middle is the actual dangerous part but unlike a bullet their damage will be spread out for as long as you are getting hit."
                    }
                }
            });
            Content.Add(new WikiSubSectionHeader("Gamemodes"));
            Content.Add(new WikiParagraph("This ruleset has multiple gamemodes built in, similar to how Mania can have different key amounts but instead of just increasing the lanes these change how bullets will be coming at you. " +
                        "What is the same in all 3 of the gamemodes however, is that you will be dodging bullets to the beat to stay alive."));
            Content.Add(gamemodeDescription = new WikiOptionEnumExplanation<VitaruGamemode>(selectedGamemode));
            Content.Add(new WikiSubSectionHeader("Scoring"));
            Content.Add(new WikiParagraph("Scoring done on a per-bullet level and can be done in different ways depending on what you have selected. " +
                        "When vitaru move out of alpha and into beta this will be locked to one metric, the best one."));
            Content.Add(scoringDescription = new WikiOptionEnumExplanation<ScoringMetric>(selectedScoring));
            Content.Add(new WikiSubSectionHeader("Mods"));
            Content.Add(new WikiParagraph("Mods affect gameplay just like the other rulesets in the game, but here is how they affect vitaru so you aren't scratching your head trying to figure it out just by playing with it."));
            Content.Add(modsDescription = new WikiOptionEnumExplanation<Mod>(selectedMod));
            Content.Add(new WikiSubSectionHeader("Characters"));
            Content.Add(new WikiParagraph("Selecting a different character in dodge or vitaru should only change what you look like " +
                "(however I am sure that some parts of Touhosu slip into them at this stage in the ruleset's development). " +
                "In Touhosu however, this will change a number of stats listed below. " +
                "I also listed their " +
                "difficulty to play (Easy, Normal, Hard, Insane, Another, Extra) " +
                "and their Role in a multiplayer setting (Offense, Defense, Support). " +
                "Most of it is subjective but ¯\\_(ツ)_/¯"));
            Content.Add(characterDescription = new WikiOptionEnumExplanation<SelectableCharacters>(selectedCharacter));

            //basically just an ingame wiki for the characters
            selectedCharacter.ValueChanged += character =>
            {
                restart:

                string stats = "\nMax Health: " + VitaruPlayer.DefaultHealth +
                        "\nMax Energy: " + VitaruPlayer.DefaultEnergy +
                        "\nEnergy Cost: " + VitaruPlayer.DefaultEnergyCost +
                        "\nEnergy Cost per Second: " + VitaruPlayer.DefaultEnergyCostPerSecond +
                        "\nRole: ???" +
                        "\nDifficulty: ???" +
                        "\nAbility: Not Implemented Yet!";

                switch (character)
                {
                    case SelectableCharacters.ReimuHakurei:
                        if (selectedGamemode == VitaruGamemode.Touhosu)
                            stats = Reimu.Background;
                        else
                        {
                            selectedCharacter.Value = SelectableCharacters.RyukoyHakurei;
                            goto restart;
                        }
                        break;
                    case SelectableCharacters.RyukoyHakurei:
                        stats = "\nMax Health: " + Ryukoy.RyukoyHealth +
                        "\nMax Energy: " + Ryukoy.RyukoyEnergy +
                        "\nEnergy Cost: " + Ryukoy.RyukoyEnergyCost +
                        "\nEnergy Cost per Second: " + Ryukoy.RyukoyEnergyCostPerSecond +
                        "\nRole: ???" +
                        "\nDifficulty: ???" +
                        "\nAbility: Spirit Walker (Not Implemented Yet!)";

                        if (selectedGamemode == VitaruGamemode.Touhosu)
                        {
                            stats = stats + "\n\n" + Ryukoy.Background;
                        }
                        break;
                    case SelectableCharacters.TomajiHakurei:
                        stats = "\nMax Health: " + Tomaji.TomajiHealth +
                        "\nMax Energy: " + Tomaji.TomajiEnergy +
                        "\nEnergy Cost: " + Tomaji.TomajiEnergyCost +
                        "\nEnergy Cost per Second: " + Tomaji.TomajiEnergyCostPerSecond +
                        "\nRole: ???" +
                        "\nDifficulty: ???" +
                        "\nAbility: Time Warden";

                        if (selectedGamemode == VitaruGamemode.Touhosu)
                        {
                            stats = stats + "\n\n" + Tomaji.Background;
                        }
                        break;
                    case SelectableCharacters.SakuyaIzayoi:
                        if (selectedGamemode == VitaruGamemode.Touhosu)
                            stats = Sakuya.Background;
                        else
                        {
                            selectedCharacter.Value = SelectableCharacters.RyukoyHakurei;
                            goto restart;
                        }
                        break;
                }

                characterDescription.Description.Text = stats;
            };
            selectedCharacter.TriggerChange();

            selectedGamemode.ValueChanged += gamemode =>
            {
                switch (gamemode)
                {
                    case VitaruGamemode.Vitaru:
                        gamemodeDescription.Description.Text = "The default gamemode in this ruleset which is based on the touhou series danmaku games. " +
                        "Allows you to kill enemies while dodging bullets to the beat!";
                        break;
                    case VitaruGamemode.Gravaru:
                        gamemodeDescription.Description.Text = "Gravity Enabled!\n" +
                        "Currently a very incomplete experiance, just messing with gravity physics atm. Stay tuned!";
                        break;
                    case VitaruGamemode.Dodge:
                        gamemodeDescription.Description.Text = "Completly changes how vitaru is played. " +
                        "The Dodge gamemode changes the playfield to a much shorter rectangle and send bullets your way from all directions while also taking away your ability to shoot!";
                        break;
                    case VitaruGamemode.Touhosu:
                        gamemodeDescription.Description.Text = "The \"amplified\" gamemode. Touhosu mode is everything Vitaru is and so much more. " +
                        "Selecting different characters no longer just changes your skin but also your stats and allows you to use spells!\n\n" +
                        "Also allows you to start story mode.";
                        break;
                }
                selectedCharacter.TriggerChange();
            };
            selectedGamemode.TriggerChange();

            selectedScoring.ValueChanged += scoring =>
            {
                switch (scoring)
                {
                    case ScoringMetric.Graze:
                        scoringDescription.Description.Text = "Score per bullet is based on how close it got to hitting you, the closer a bullet got the more score it will give.";
                        break;
                    case ScoringMetric.ScoreZones:
                        scoringDescription.Description.Text = "Based on where you are located on the screen, the closer to the center the more score you will get.";
                        break;
                    case ScoringMetric.InverseCatch:
                        scoringDescription.Description.Text = "Quite litterally the opposite of catch, if a bullet doesn't hit you its a Perfect";
                        break;
                }
            };
            selectedScoring.TriggerChange();

            selectedMod.ValueChanged += mod =>
            {
                switch (mod)
                {
                    default:
                        modsDescription.Description.Text = "Check back later!";
                        break;
                    case Mod.Easy:
                        modsDescription.Description.Text = "Bullets are smaller (Singleplayer only),\n" +
                        "You deal more damage (Multiplayer only),\n" +
                        "You take less damage,\n" +
                        "In Touhosu mode you will generate energy faster.";
                        break;
                    case Mod.Hidden:
                        modsDescription.Description.Text = "Bullets fade out over time";
                        break;
                    case Mod.Flashlight:
                        modsDescription.Description.Text = "Bullets are only visable near you";
                        break;
                    case Mod.HardRock:
                        modsDescription.Description.Text = "You deal less damage (Singleplayer only),\n" +
                        "Your Hitbox is larger,\n" +
                        "You take more damage,\n" +
                        "In Touhosu mode you will generate energy slower.";
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
