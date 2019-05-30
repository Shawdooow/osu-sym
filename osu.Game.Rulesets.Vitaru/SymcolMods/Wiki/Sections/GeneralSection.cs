using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Wiki.Sections;
using Symcol.osu.Core.Wiki.Sections.OptionExplanations;
using Symcol.osu.Core.Wiki.Sections.SectionPieces;
using Symcol.osu.Core.Wiki.Sections.Subsection;

namespace osu.Game.Rulesets.Vitaru.SymcolMods.Wiki.Sections
{
    public class GeneralSection : WikiSection
    {
        public override string Title => "General";

        private Bindable<Mod> selectedMod = new Bindable<Mod> { Default = Mod.Hidden };

        private WikiOptionEnumExplanation<Mod> modsDescription;

        [BackgroundDependencyLoader]
        private void load()
        {
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
            Content.Add(new WikiParagraph("Lets get you familiar with the anatomy of different game objects.\n"));
            Content.Add(new WikiSplitColum(
                new WikiParagraph("On the right we have the Player, unfortunetly I have not had time to implement squishy insides so for now we are just going to go over the basics." +
                        "I have also revealed the hitbox so I can explain why thats the only part that actually matters. " +
                        "First, see that little white dot with the colored ring in the middle of the player? Thats the hitbox. " +
                        "You only take damage if that white part gets hit, bullets will pass right over the rest of the player without actually harming you in any way, infact that will heal you!"),
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
            Content.Add(new WikiSubSectionHeader("Scoring"));
            Content.Add(new WikiParagraph("Score per bullet is based on how close it got to hitting you, the closer a bullet got the more score it will give. The \"Great\" area is about the same size as the green health ring, " +
                "the \"Good\" is twice that and \"Meh\" is infinite (so by default a bullet gives meh unless you got close)."));
            Content.Add(new WikiSubSectionHeader("Mods"));
            Content.Add(new WikiParagraph("Mods affect gameplay just like the other rulesets in the game, but here is how they affect vitaru so you aren't scratching your head trying to figure it out just by playing with it."));
            Content.Add(modsDescription = new WikiOptionEnumExplanation<Mod>(selectedMod));



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
