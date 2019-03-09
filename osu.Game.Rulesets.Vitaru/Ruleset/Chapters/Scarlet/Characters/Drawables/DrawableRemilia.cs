using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables;
using osuTK;
using osuTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Chapters.Scarlet.Characters.Drawables
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

        private void bulletAddRad(double speed, double angle, Color4 color, double size, double damage)
        {
            DrawableBullet drawableBullet;

            CurrentPlayfield.Add(drawableBullet = new DrawableBullet(new Bullet
            {
                StartTime = VitaruPlayfield.Current,
                Position = Position,
                Angle = angle,
                Speed = speed,
                Diameter = size,
                Damage = damage,
                ColorOverride = color,
                Team = Team,
                DummyMode = true,
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
            double directionModifier = -0.2d;
            double cursorAngle = MathHelper.RadiansToDegrees(Math.Atan2(Cursor.Position.Y - Position.Y, Cursor.Position.X - Position.X)) + 90 + Rotation;

            for (int i = 1; i <= numberbullets; i++)
            {
                Color4 color;
                double size;
                double damage;

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
                directionModifier += 0.1d;
            }
        }
    }
}
