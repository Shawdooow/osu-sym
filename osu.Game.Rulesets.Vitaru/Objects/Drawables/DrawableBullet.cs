using osu.Framework.Graphics;
using OpenTK;
using System;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Scoring;
using OpenTK.Graphics;
using Symcol.Core.GameObjects;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableBullet : DrawableVitaruHitObject
    { 
        private readonly Gamemodes gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.Gamemode);

        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.BulletVisuals);

        //Playfield size + Margin of 10 on each side
        public Vector4 BulletBounds = new Vector4(-10, -10, 522, 830);

        //Set to "true" when a judgement should be returned
        public bool ReturnJudgement;

        //Set to "true" when a judgement has been returned
        private bool returnedJudgement;

        public new bool Masking
        {
            get => base.Masking;
            set => base.Masking = value;
        }

        public bool ReturnGreat = false;

        public static bool BoundryHacks;

        //Can be set for the Graze ScoringMetric
        public int ScoreZone = 50;

        //Should be set to true when a character is hit
        public bool Hit;

        public readonly Bullet Bullet;

        public Action OnHit;

        public SymcolHitbox Hitbox;

        private BulletPiece bulletPiece;

        public DrawableBullet(Bullet bullet, VitaruPlayfield playfield) : base(bullet, playfield)
        {
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            Bullet = bullet;

            if (gamemode == Gamemodes.Dodge)
                BulletBounds = new Vector4(-10, -10, 522, 394);
            else if (gamemode == Gamemodes.Touhosu)
                BulletBounds = new Vector4(-10, -10, 512 * 2 + 10, 820);
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            base.CheckForJudgements(userTriggered, timeOffset);

            if (returnedJudgement) return;

            if (ReturnJudgement)
            {
                returnedJudgement = true;
                switch (ScoreZone)
                {
                    case 0:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Miss });
                        break;
                    case 50:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Meh });
                        break;
                    case 100:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Good });
                        break;
                    case 300:
                        AddJudgement(new VitaruJudgement { Result = HitResult.Great });
                        break;
                }
            }

            else if (Hit)
            {
                if (graphics == GraphicsOptions.Old)
                    bulletPiece.Alpha = 0;
                else
                    bulletPiece.ScaleTo(new Vector2(0.75f))
                               .FadeColour(Color4.Red, 500, Easing.OutCubic)
                               .FadeOut(500, Easing.InCubic);

                AddJudgement(new VitaruJudgement { Result = HitResult.Miss });
                returnedJudgement = true;
                End();
            }

            else if (ReturnGreat)
            {
                AddJudgement(new VitaruJudgement { Result = HitResult.Great });
                returnedJudgement = true;
                End();
            }
        }

        protected override void Update()
        {
            base.Update();

            if (OnHit != null && Hit)
            {
                OnHit();
                OnHit = null;
            }

            if (Time.Current >= Bullet.StartTime)
            {
                double completionProgress = MathHelper.Clamp((Time.Current - Bullet.StartTime) / Bullet.Duration, 0, 1);

                Position = Bullet.PositionAt(completionProgress);

                if (Bullet.ObeyBoundries && (Position.Y < BulletBounds.Y || Position.X < BulletBounds.X || Position.Y > BulletBounds.W || Position.X > BulletBounds.Z) && !BoundryHacks)
                    End();
            }
        }

        protected override void Load()
        {
            if (returnedJudgement) return;
            base.Load();

            Alpha = 0;
            Size = new Vector2((float)Bullet.Diameter);

            if (graphics == GraphicsOptions.Old)
                Scale = new Vector2(0.1f);

            Children = new Drawable[]
            {
                bulletPiece = new BulletPiece(this),
                Hitbox = new SymcolHitbox
                {
                    Size = new Vector2((float)Bullet.Diameter),
                    Team = Bullet.Team,
                    HitDetection = false
                }
            };
        }

        protected override void Start()
        {
            if (returnedJudgement) return;
            base.Start();

            Position = Bullet.PositionAt(0);
            Hitbox.HitDetection = true;

            if (graphics == GraphicsOptions.Old)
                this.FadeInFromZero(100)
                    .ScaleTo(Vector2.One, 100);
            else
            {
                Alpha = 1;
                bulletPiece.Rotation = (float)MathHelper.RadiansToDegrees(Bullet.Angle) + 90;
                bulletPiece.Scale = new Vector2(1.5f);
                bulletPiece.FadeInFromZero(100, Easing.OutSine)
                           .ScaleTo(Vector2.One, 100, Easing.InSine);
                if (Bullet.Shape == Shape.Circle)
                    bulletPiece.Box.FadeInFromZero(150, Easing.InSine);
            }
        }

        protected override void End()
        {
            base.End();

            Hit = false;
            ReturnGreat = false;
            ReturnJudgement = true;

            if (bulletPiece == null) return;

            if (graphics == GraphicsOptions.Old)
                bulletPiece.FadeOut(100)
                           .OnComplete(b => { Unload(); });
            else
            {
                bulletPiece.FadeOut(300, Easing.OutCubic)
                           .ScaleTo(new Vector2(1.5f), 300, Easing.OutSine)
                           .OnComplete(b => { Unload(); });
                bulletPiece.Box.FadeOut(150, Easing.InSine);
            }
        }

        protected override void Unload()
        {
            base.Unload();

            Remove(bulletPiece);
            Remove(Hitbox);

            bulletPiece.Dispose();
            Hitbox.Dispose();

            Delete();
        }
    }
}
