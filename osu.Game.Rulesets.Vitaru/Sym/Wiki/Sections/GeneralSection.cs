﻿#region usings

using System.Collections.Generic;
using osu.Core.Wiki.Sections;
using osu.Core.Wiki.Sections.OptionExplanations;
using osu.Core.Wiki.Sections.SectionPieces;
using osu.Core.Wiki.Sections.Subsection;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Timing;
using osu.Game.Rulesets.Vitaru.Ruleset.Edit;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables.Pieces;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Sym.Wiki.Sections
{
    public class GeneralSection : WikiSection
    {
        public override string Title => "General";

        private readonly Bindable<Patterns> pattern = new Bindable<Patterns>();

        private readonly DecoupleableInterpolatingFramedClock patternClock;

        private readonly VitaruEditPlayfield playfield;

        private readonly WikiParagraph patternDescription;

        public GeneralSection()
        {
            patternClock = new DecoupleableInterpolatingFramedClock
            {
                IsCoupled = false,
            };

            playfield = new VitaruEditPlayfield(null)
            {
                AlwaysPresent = true,
                Clock = patternClock,
            };

            patternDescription = new WikiParagraph("Spoopy Bug.txt");

            pattern.ValueChanged += value =>
            {
                List<Projectile> projectiles;

                switch (value)
                {
                    default:
                        projectiles = Ruleset.Patterns.Wave(1, 8, 0, new Vector2(20, 200), 0, 0);
                        patternDescription.Text = "The Wave pattern.\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n";
                        break;
                }

                patternClock.Reset();
                playfield.Gamefield.Children = new Drawable[] { };

                foreach (Projectile p in projectiles)
                {
                    if (p is Bullet b)
                        playfield.Gamefield.Add(new DrawableBullet(b, playfield));
                    //if (p is Laser l)
                    //playfield.Gamefield.Add(new DrawableLaser(l));
                }

                patternClock.Seek(1000);
            };
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            Content.Add(new WikiParagraph("Your objective in vitaru is simple, don't get hit by the bullets flying at you, although this is easier said than done."));

            Content.Add(new WikiSubSectionHeader("Converts (Standard => Vitaru)"));
            Content.Add(new WikiParagraph("Circle Size (CS) affects projectile size.\n" +
                        "Accuracy (OD) affects how large the graze box is / how forgiving the score zones are.\n" +
                        "Health Drain (HP) affects how much damage bullets do to you.\n" +
                        "Approach Rate (AR) affects enemy enter + leave speeds.\n" +
                        "Slider Velocity (SV) affects bullet speeds.\n" +
                        "Hitsounds affect the pattern that will be thrown, see the \"Patterns\" subsection for more\n\n" +
                        "Object positions are mapped to the top half of the playfield (or whole playfield for dodge) in the same orientation as standard."));

            Content.Add(new WikiSubSectionHeader("Patterns"));
            Content.Add(new WikiOptionEnumSplitExplanation<Patterns>(pattern, playfield, patternDescription));
            pattern.TriggerChange();

            Content.Add(new WikiSubSectionHeader("Controls"));
            Content.Add(new WikiParagraph("Controls by default will probably be the most confortable and fitting for all of the gamemodes in this ruleset (if they aren't / weren't they will be changed before release).\n\n" +
                        "W = Move the player up\n" +
                        "S = Down\n" +
                        "A = Left\n" +
                        "D = Right\n" +
                        "Shift = Slow the player to half speed and show the hitbox.\n" +
                        "Left Mouse = Shoot"));

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
                            Texture = VitaruRuleset.VitaruTextures.Get("Sakuya IzayoiKiai")
                        },
                        new CircularContainer
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Scale = new Vector2(2),
                            Size = new Vector2(8),
                            BorderThickness = 3,
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
                new BulletPiece(Color4.Green, 32, Shape.Circle)
                {
                    RelativeSizeAxes = Axes.None,
                    Size = new Vector2(32),
                },
                new WikiParagraph("On the left we have a bullet. Bullets are pretty simple, " +
                "see the white circle in the middle? If that touches the white circle in your hitbox you take damage.")));
            Content.Add(new WikiSplitColum(
                new WikiParagraph("On the right here is a laser. " +
                        "Basically they work like a bullet in that the white rectangle in the middle is the actual dangerous part " +
                        "but unlike a bullet their damage will be spread out for as long as you are getting hit."),
                    new LaserPiece(Color4.Red, 8)
                    {
                        RelativeSizeAxes = Axes.None,
                        Size = new Vector2(128, 16),
                    }));
            Content.Add(new WikiSplitColum(
                new Container(),
                new WikiParagraph("Over on the left will be a seeking bullet, once they are working.")));
            Content.Add(new WikiSubSectionHeader("Scoring"));
            Content.Add(new WikiParagraph("Score per bullet is based on how close it got to hitting you, the closer a bullet got the more score it will give. The \"Great\" area is about the same size as the green health ring, " +
                "the \"Good\" is twice that and \"Meh\" is infinite (so by default a bullet gives meh unless you got close)."));
        }

        private enum Patterns
        {
            Wave,
            Line,
            Triangle,
            Cross,
            Flower,
        }
    }
}
