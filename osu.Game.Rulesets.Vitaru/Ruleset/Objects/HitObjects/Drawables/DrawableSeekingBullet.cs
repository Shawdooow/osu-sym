#region usings

using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces;
using osuTK;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables
{
    public class DrawableSeekingBullet : DrawableProjectile
    {
        public DrawableCharacter NearestEnemy;

        private double startTime;

        //Result of bulletSpeed + bulletAngle math, should never be modified outside of this class
        private Vector2 bulletVelocity;

        //Incase we want to be deleted in the near future
        public double BulletDeleteTime = -1;

        public readonly SeekingBullet SeekingBullet;

        //Playfield size + Margin of 10 on each side
        public Vector4 BulletBounds = new Vector4(-10, -10, 520, 830);

        public DrawableSeekingBullet(SeekingBullet seekingBullet, VitaruPlayfield playfield) : base(seekingBullet, playfield)
        {
            AlwaysPresent = true;
            Alpha = 0;
            Scale = new Vector2(0.1f);
            Size = new Vector2(20);

            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            this.FadeInFromZero(100);
            this.ScaleTo(Vector2.One, 100);

            SeekingBullet = seekingBullet;

            if (Gamemode is DodgeGamemode)
                BulletBounds = new Vector4(-10, -10, 522, 394);

            InternalChildren = new Drawable[]
            {
                new SeekingBulletPiece(this),
                //Hitbox = new VitaruHitbox(Size, Shape.Rectangle)
            };
        }

        protected override void LoadComplete()
        {
            startTime = VitaruPlayfield.Current;
        }

        private void nearestEnemy()
        {
            float minDist = float.MaxValue;
            foreach (Drawable draw in CurrentPlayfield)
            {
                DrawableCharacter enemy = draw as DrawableCharacter;
                if (enemy?.Hitbox != null && enemy.Team != SeekingBullet.Team && enemy.Alpha > 0)
                {
                    Vector2 pos = enemy.ToSpaceOfOtherDrawable(Vector2.Zero, this) + new Vector2(6);
                    float distance = (float)Math.Sqrt(Math.Pow(pos.X, 2) + Math.Pow(pos.Y, 2));

                    if (distance < minDist)
                    {
                        NearestEnemy = enemy;
                        minDist = distance;
                    }
                }
            }
        }

        public double EnemyRelativePositionAngle()
        {
            //Returns a Radian
            double enemyAngle = Math.Atan2(NearestEnemy.Position.Y - Position.Y, NearestEnemy.Position.X - Position.X);
            return enemyAngle;
        }

        private Vector2 getBulletVelocity(double angle)
        {
            Vector2 velocity = new Vector2(SeekingBullet.Speed * (float)Math.Cos(angle), SeekingBullet.Speed * (float)Math.Sin(angle));
            return velocity;
        }

        private void unload()
        {
            Alpha = 0;
            Expire();
        }

        protected override void Update()
        {
            base.Update();

            if (Hit)
                unload();

            Rotation = Rotation + 0.25f;

            if (BulletDeleteTime <= VitaruPlayfield.Current && BulletDeleteTime != -1)
                unload();

            if (SeekingBullet.ObeyBoundries && Position.Y < BulletBounds.Y | Position.X < BulletBounds.X | Position.Y > BulletBounds.W | Position.X > BulletBounds.Z && BulletDeleteTime == -1)
            {
                BulletDeleteTime = VitaruPlayfield.Current + HitObject.TimePreempt / 6;
                this.FadeOutFromOne(HitObject.TimePreempt / 6);
            }

            //IdleTimer
            float frameTime = (float)Clock.ElapsedFrameTime;
            bulletVelocity = getBulletVelocity(MathHelper.DegreesToRadians(SeekingBullet.StartAngle - 90));

            if (startTime + 300 <= VitaruPlayfield.Current)
            {
                nearestEnemy();
                if (NearestEnemy != null && !NearestEnemy.Dead)
                {
                    bulletVelocity = getBulletVelocity(EnemyRelativePositionAngle());
                    this.MoveToOffset(new Vector2(bulletVelocity.X * frameTime, bulletVelocity.Y * frameTime));

                }
                else
                    this.MoveToOffset(new Vector2(bulletVelocity.X * frameTime, bulletVelocity.Y * frameTime));
            }
            else
                this.MoveToOffset(new Vector2(bulletVelocity.X * frameTime, bulletVelocity.Y * frameTime));

        }
    }
}
