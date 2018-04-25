using osu.Framework.Graphics;
using OpenTK;
using System;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Judgements;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Game.Rulesets.Scoring;
using Symcol.Core.GameObjects;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawableBullet : DrawableVitaruHitObject
    { 
        public static int BulletCount;

        private readonly VitaruGamemode currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);

        //Used like a multiple (useful for spells in multiplayer)
        public static float BulletSpeedModifier = 1;

        //Playfield size + Margin of 10 on each side
        public Vector4 BulletBounds = new Vector4(-10, -10, 520, 830);

        //Set to "true" when a judgement should be returned
        private bool returnJudgement;

        public bool ReturnGreat = false;

        //Can be set for the Graze ScoringMetric
        public int ScoreZone = 50;

        //Should be set to true when a character is hit
        public bool Hit;

        //Incase we want to be deleted in the near future
        public double BulletDeleteTime = -1;

        private readonly DrawablePattern drawablePattern;
        public readonly Bullet Bullet;

        public Action OnHit;

        public SymcolHitbox Hitbox;

        private BulletPiece bulletPiece;

        public DrawableBullet(Bullet bullet, DrawablePattern drawablePattern, VitaruPlayfield playfield) : base(bullet, playfield)
        {
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            BulletCount++;

            Bullet = bullet;
            this.drawablePattern = drawablePattern;

            if (currentGameMode == VitaruGamemode.Dodge)
                BulletBounds = new Vector4(-10, -10, 522, 394);
            else if (currentGameMode == VitaruGamemode.Gravaru)
                BulletBounds = new Vector4(-10, -10, 384 * 2 + 10, 394);
        }

        public DrawableBullet(Bullet bullet, VitaruPlayfield playfield) : base(bullet, playfield)
        {
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            BulletCount++;

            Bullet = bullet;

            if (currentGameMode == VitaruGamemode.Dodge)
                BulletBounds = new Vector4(-10, -10, 522, 394);
            else if (currentGameMode == VitaruGamemode.Gravaru)
                BulletBounds = new Vector4(-10, -10, 384 * 2 + 10, 394);
        }

        protected override void CheckForJudgements(bool userTriggered, double timeOffset)
        {
            base.CheckForJudgements(userTriggered, timeOffset);

            if (returnJudgement)
            {
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
                AddJudgement(new VitaruJudgement { Result = HitResult.Miss });
                bulletPiece.Alpha = 0;
                End();
            }

            else if (ReturnGreat)
            {
                AddJudgement(new VitaruJudgement { Result = HitResult.Great });
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

                if (Bullet.ObeyBoundries && Position.Y < BulletBounds.Y | Position.X < BulletBounds.X | Position.Y > BulletBounds.W | Position.X > BulletBounds.Z && !returnJudgement)
                    End();
            }
        }

        protected override void Load()
        {
            base.Load();

            Alpha = 0;
            Size = new Vector2((float)Bullet.BulletDiameter);
            Scale = new Vector2(0.1f);

            Children = new Drawable[]
            {
                bulletPiece = new BulletPiece(this),
                Hitbox = new SymcolHitbox(new Vector2((float)Bullet.BulletDiameter), Shape.Circle)
                {
                    Team = Bullet.Team,
                    HitDetection = false
                }
            };
        }

        protected override void Start()
        {
            base.Start();

            Position = Bullet.PositionAt(0);
            Hitbox.HitDetection = true;
            this.FadeInFromZero(100)
                .ScaleTo(Vector2.One, 100);
        }

        protected override void End()
        {
            base.End();
            bulletPiece.FadeOut(100);
            returnJudgement = true;
        }

        protected override void Unload()
        {
            base.Unload();

            Remove(bulletPiece);
            Remove(Hitbox);

            bulletPiece.Dispose();
            Hitbox.Dispose();

            VitaruPlayfield.GameField.Remove(this);
            Dispose();
        }

        protected override void Dispose(bool isDisposing)
        {
            BulletCount--;
            base.Dispose(isDisposing);
        }
    }
}
