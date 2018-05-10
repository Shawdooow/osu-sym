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

namespace osu.Game.Rulesets.Vitaru.Wiki.Sections
{
    public class GameplaySection : WikiSection
    {
        public override string Title => "Gameplay";

        private Bindable<Gamemodes> selectedGamemode;
        private Bindable<string> selectedCharacter;

        private WikiOptionExplanation<string> characterDescription;

        private Bindable<Mod> selectedMod = new Bindable<Mod> { Default = Mod.Hidden };

        private WikiOptionEnumExplanation<Mod> modsDescription;
        private WikiOptionEnumExplanation<Gamemodes> gamemodeDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
            selectedGamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);
            selectedCharacter = VitaruSettings.VitaruConfigManager.GetBindable<string>(VitaruSetting.Character);

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
            Content.Add(new WikiParagraph("Selecting a different character in anything except Touhosu should only change what you look like " +
                "(however I am sure that some parts of Touhosu slip out at this stage in the ruleset's development). " +
                "In Touhosu however, this will change a number of stats listed below. " +
                "I also listed their " +
                "difficulty to play (Easy, Normal, Hard, Insane, Another, Extra) " +
                "and their Role in a multiplayer setting (Offense, Defense, Support)."));
            //Content.Add(characterDescription = new WikiOptionExplanation<string>(selectedCharacter));

            //basically just an ingame wiki for the characters
            selectedCharacter.ValueChanged += character =>
            {
                TouhosuPlayer touhosuCharacter = new TouhosuPlayer();

                switch (character)
                {
                    case "ReimuHakurei":
                        touhosuCharacter = new Reimu();
                        break;
                    case "RyukoyHakurei":
                        touhosuCharacter = new Ryukoy();
                        break;
                    case"TomajiHakurei":
                        touhosuCharacter = new Tomaji();
                        break;
                    case "SakuyaIzayoi":
                        touhosuCharacter = new Sakuya();
                        break;
                }

                string stats = "\nHealth: " + touhosuCharacter.MaxHealth +
                "\nEnergy: " + touhosuCharacter.MaxEnergy +
                "\nEnergy Cost: " + touhosuCharacter.EnergyCost +
                "\nEnergy Cost per Second: " + touhosuCharacter.EnergyCostPerSecond +
                "\nRole: ???" +
                "\nDifficulty: ???" +
                "\nAbility: Not Implemented Yet!\n\n" +
                touhosuCharacter.Background;

                //characterDescription.Description.Text = stats;
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
                selectedCharacter.TriggerChange();
            };
            selectedGamemode.TriggerChange();

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
