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
        private readonly Gamemodes gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.BulletVisuals);

        //Playfield size + Margin of 10 on each side
        public Vector4 BulletBounds = new Vector4(-10, -10, 522, 830);

        //Set to "true" when a judgement should be returned
        private bool returnJudgement;

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
            else if (gamemode == Gamemodes.Gravaru)
                BulletBounds = new Vector4(-10, -10, 384 * 2 + 10, 394);
            else if (gamemode == Gamemodes.Touhosu)
                BulletBounds = new Vector4(-10, -10, 512 * 2 + 10, 820);
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
                if (graphics == GraphicsOptions.StandardV2)
                    bulletPiece.ScaleTo(new Vector2(0.75f))
                               .FadeColour(Color4.Red, 500, Easing.OutCubic)
                               .FadeOut(500, Easing.InCubic);
                else
                    bulletPiece.Alpha = 0;

                AddJudgement(new VitaruJudgement { Result = HitResult.Miss });
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

                if (Bullet.ObeyBoundries && (Position.Y < BulletBounds.Y || Position.X < BulletBounds.X || Position.Y > BulletBounds.W || Position.X > BulletBounds.Z) && !returnJudgement && !BoundryHacks)
                    End();
            }
        }

        protected override void Load()
        {
            base.Load();

            Alpha = 0;
            Size = new Vector2((float)Bullet.BulletDiameter);

            if (graphics != GraphicsOptions.StandardV2)
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
            if (returnJudgement) return;
            base.Start();

            Position = Bullet.PositionAt(0);
            Hitbox.HitDetection = true;

            if (graphics == GraphicsOptions.StandardV2)
            {
                Alpha = 1;
                bulletPiece.Scale = new Vector2(1.5f);
                bulletPiece.Scale = new Vector2(0.75f);
                bulletPiece.FadeInFromZero(200, Easing.InCubic)
                           .ScaleTo(Vector2.One, 200, Easing.OutQuint);
                bulletPiece.Box.FadeInFromZero(100, Easing.OutQuint)
                           .ScaleTo(Vector2.One, 200, Easing.OutQuint);
            }
            else
                this.FadeInFromZero(100)
                    .ScaleTo(Vector2.One, 100);

        }

        protected override void End()
        {
            base.End();

            if (graphics == GraphicsOptions.StandardV2)
            {
                bulletPiece.FadeOut(200, Easing.InCubic)
                           .ScaleTo(new Vector2(1.5f), 200, Easing.OutQuint)
                           .OnComplete((b) => { Unload(); });
                bulletPiece.Box.FadeOut(100, Easing.OutQuint)
                           .ScaleTo(new Vector2(0.75f), 200, Easing.OutQuint);
            }
            else
                bulletPiece.FadeOut(100)
                    .OnComplete((b) => { Unload(); });

            returnJudgement = true;
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
