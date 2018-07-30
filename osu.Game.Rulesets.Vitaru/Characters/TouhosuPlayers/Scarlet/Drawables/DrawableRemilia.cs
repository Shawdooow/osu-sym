using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.OldMulti;
using osu.Game.Rulesets.Vitaru.UI;
using OpenTK;
using OpenTK.Graphics;

namespace osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Scarlet.Drawables
{
    public class DrawableRemilia : DrawableTouhosuPlayer
    {
        public DrawableRemilia(VitaruPlayfield playfield, VitaruNetworkingClientHandler vitaruNetworkingClientHandler)
            : base(playfield, new Remilia(), vitaruNetworkingClientHandler)
        {
            Spell += action => leechPattern();
        }

        private void bulletAddRad(double speed, double angle, Color4 color, double size, double damage)
        {
            DrawableBullet drawableBullet;

            CurrentPlayfield.Add(drawableBullet = new DrawableBullet(new Bullet
            {
                StartTime = Time.Current,
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
                    size = 20;
                    damage = 24;
                    color = PrimaryColor;
                }
                else
                {
                    size = 12;
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
