using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;
using osuTK.Graphics;
using Sym.Base.Extentions;

// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables
{
    public class DrawableBullet : DrawableProjectile
    {
        // ReSharper disable once InconsistentNaming
        public static Bindable<int> BULLET_COUNT = new Bindable<int>();

        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.BulletVisuals);

        public static bool BoundryHacks;

        public new readonly Bullet HitObject;

        private BulletPiece bulletPiece;

        //bullet + this
        protected override double object_size => 269.34d + 1193.92d;

        public DrawableBullet(Bullet bullet, VitaruPlayfield playfield) : base(bullet, playfield)
        {
            BULLET_COUNT.Value++;

            OnFinalize += () => BULLET_COUNT.Value--;

            HitObject = bullet;

            Size = new Vector2(HitObject.Diameter);
        }

        protected override double Weight(double distance)
        {
            double difficulty = 1;

            difficulty *= HitObject.Speed;

            return (distance > 128 ? 0 : 2000 / Math.Max(SymcolMath.Scale(distance, 180, Hitbox.Width, 1, 8), 1)) * difficulty;
        }

        protected override void HitCharacter()
        {
            base.HitCharacter();

            if (graphics == GraphicsOptions.Old)
                bulletPiece.Alpha = 0;
            else
                bulletPiece.FadeColour(Color4.Red, 50, Easing.OutCubic);

            ApplyResult(HitResult.Miss);
            End();
            ReturnedJudgement = true;
        }

        protected override void Update()
        {
            base.Update();

            float current = VitaruPlayfield.Current;

            if (current >= HitObject.StartTime && current <= HitObject.EndTime)
            {
                double completionProgress = MathHelper.Clamp((current - HitObject.StartTime) / HitObject.Duration, 0, 1);

                Position = HitObject.PositionAt(completionProgress);

                if (HitObject.Hidden)
                {
                    Vector2 playerPos = VitaruPlayfield.Player.ToSpaceOfOtherDrawable(Vector2.Zero, this);
                    double distance = Math.Sqrt(Math.Pow(playerPos.X, 2) + Math.Pow(playerPos.Y, 2));
                    Alpha = (float)GetHDAlpha(distance);
                }

                if (HitObject.Flashlight)
                {
                    Vector2 playerPos = VitaruPlayfield.Player.ToSpaceOfOtherDrawable(Vector2.Zero, this);
                    double distance = Math.Sqrt(Math.Pow(playerPos.X, 2) + Math.Pow(playerPos.Y, 2));
                    Alpha = (float)GetFLAlpha(distance);
                }

                if (HitObject.ObeyBoundries && (Position.Y <= Gamemode.PlayfieldBounds.Y - 10 || Position.X <= Gamemode.PlayfieldBounds.X - 10 || Position.Y >= Gamemode.PlayfieldBounds.W + 10 || Position.X >= Gamemode.PlayfieldBounds.Z + 10) && !BoundryHacks)
                    End();
            }
        }

        protected override void Preempt()
        {
            base.Preempt();

            Alpha = 0;

            if (graphics == GraphicsOptions.Old)
                Scale = new Vector2(0.1f);

            InternalChildren = new Drawable[]
            {
                bulletPiece = new BulletPiece(AccentColour, HitObject.Diameter, HitObject.Shape),
                Hitbox = new VitaruHitbox
                {
                    Size = Size,
                    HitDetection = false
                }
            };
        }

        protected override void Start()
        {
            if (ReturnedJudgement) return;
            base.Start();

            Position = HitObject.PositionAt(0);

            Hitbox.HitDetection = true;

            if (graphics == GraphicsOptions.Old)
                this.FadeInFromZero(100)
                    .ScaleTo(Vector2.One, 100);
            else
            {
                Alpha = 1;
                bulletPiece.Rotation = MathHelper.RadiansToDegrees(HitObject.Angle) + 90;
                bulletPiece.Scale = new Vector2(1.5f);
                bulletPiece.FadeInFromZero(100, Easing.OutSine)
                           .ScaleTo(Vector2.One, 100, Easing.InSine);
                if (HitObject.Shape == Shape.Circle)
                    bulletPiece.Box.FadeInFromZero(150, Easing.InSine);
            }

            if (HitObject.TrueHidden)
                this.FadeOut(600);
        }

        protected override void End()
        {
            if (ReturnedJudgement) return;
            base.End();

            Hit = false;
            ReturnGreat = false;
            ForceJudgement = true;

            if (bulletPiece == null) return;

            if (graphics == GraphicsOptions.Old)
                bulletPiece.FadeOut(100)
                           .OnComplete(b => { UnPreempt(); });
            else
            {
                bulletPiece.FadeOut(250)
                           .ScaleTo(new Vector2(1.5f), 250, Easing.OutCubic)
                           .OnComplete(b => { UnPreempt(); });
                bulletPiece.Box.FadeOut(100, Easing.InSine);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!Experimental)
            {
                RemoveInternal(bulletPiece);
                bulletPiece.Dispose();

                RemoveInternal(Hitbox);
                Hitbox.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
