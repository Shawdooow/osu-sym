﻿#region usings

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Scarlet.Characters.Drawables
{
    public class DrawableRemilia : DrawableTouhosuPlayer
    {
        public DrawableRemilia(VitaruPlayfield playfield)
            : base(playfield, new Remilia())
        {
        }

        protected override void SpellActivate(VitaruAction action)
        {
            base.SpellActivate(action);
            leechPattern();
        }

        private void bulletAddRad(float speed, float angle, Color4 color, float size, float damage)
        {
            DrawableBullet drawableBullet;

            CurrentPlayfield.Add(drawableBullet = new DrawableBullet(new Bullet
            {
                PatternStartTime = VitaruPlayfield.Current,
                Position = Position,
                Angle = angle,
                Speed = speed,
                Diameter = size,
                Damage = damage,
                ColorOverride = color,
                Team = Team,
                Dummy = true,
                SliderType = SliderType.Straight,
                SpeedEasing = Easing.OutCubic,
                Shape = Shape.Triangle,
            }, VitaruPlayfield));

            drawableBullet.OnHit += () => Heal(2f);
            drawableBullet.MoveTo(Position);
        }

        private void leechPattern()
        {
            const int numberbullets = 5;
            float directionModifier = -0.2f;
            float cursorAngle = MathHelper.RadiansToDegrees((float)Math.Atan2(Cursor.Position.Y - Position.Y, Cursor.Position.X - Position.X)) + 90 + Rotation;

            for (int i = 1; i <= numberbullets; i++)
            {
                Color4 color;
                float size;
                float damage;

                if (i % 2 == 0)
                {
                    size = 28;
                    damage = 24;
                    color = PrimaryColor;
                }
                else
                {
                    size = 20;
                    damage = 18;
                    color = SecondaryColor;
                }

                //-90 = up
                bulletAddRad(1.25f, MathHelper.DegreesToRadians(-90 + cursorAngle) + directionModifier, color, size, damage);
                directionModifier += 0.1f;
            }
        }
    }
}
