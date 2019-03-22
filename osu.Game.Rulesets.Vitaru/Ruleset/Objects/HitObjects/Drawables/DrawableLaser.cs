using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables
{
    public class DrawableLaser : DrawableProjectile
    {
        private readonly GraphicsOptions graphics = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.LaserVisuals);

        // ReSharper disable once InconsistentNaming
        public static Bindable<int> LASER_COUNT = new Bindable<int>();

        private LaserPiece laserPiece;

        public new readonly Laser HitObject;

        public DrawableLaser(Laser laser, VitaruPlayfield playfield) : base(laser, playfield)
        {
            LASER_COUNT.Value++;
            OnFinalize += () => LASER_COUNT.Value--;

            HitObject = laser;

            Size = HitObject.Size;
        }

        protected override void HitCharacter()
        {
            base.HitCharacter();

            if (!ReturnedJudgement)
            {
                ApplyResult(r => r.Type = HitResult.Miss);
                ReturnedJudgement = true;
            }
        }

        protected override void Update()
        {
            base.Update();
                
            if (VitaruPlayfield.Current >= HitObject.StartTime && VitaruPlayfield.Current <= HitObject.EndTime)
            {
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
            }
        }

        protected override void Preempt()
        {
            base.Preempt();

            Origin = Anchor.BottomCentre;
            Position = HitObject.Position;
            Rotation =(float)HitObject.Angle;

            Alpha = 1;

            InternalChildren = new Drawable[]
            {
                laserPiece = new LaserPiece(AccentColour, HitObject.Width)
                {
                    Scale = new Vector2(1, 0.1f)
                },
                Hitbox = new VitaruHitbox
                {
                    Size = Size,
                    HitDetection = true
                }
            };
            double time = Math.Max(HitObject.StartTime - VitaruPlayfield.Current, 0);
            laserPiece.ScaleTo(Vector2.One, time, Easing.OutQuad)
                .FadeIn(time, Easing.InQuad);
        }

        protected override void Start()
        {
            if (ReturnedJudgement) return;
            base.Start();

            if (HitObject.TrueHidden)
                laserPiece.FadeOut(600);
        }

        protected override void End()
        {
            base.End();

            Hit = false;
            ReturnGreat = false;
            ForceJudgement = !ReturnedJudgement;

            if (laserPiece == null) return;
            Hitbox.HitDetection = false;

            if (graphics == GraphicsOptions.Old)
                laserPiece.FadeOut(100)
                           .OnComplete(b => { UnPreempt(); });
            else
            {
                laserPiece.FadeOut(250)
                           .ScaleTo(new Vector2(1.5f), 250, Easing.OutCubic)
                           .OnComplete(b => { UnPreempt(); });
                laserPiece.Box.FadeOut(100, Easing.InSine);
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            if (!Experimental)
            {
                RemoveInternal(laserPiece);
                laserPiece.Dispose();

                RemoveInternal(Hitbox);
                Hitbox.Dispose();
            }

            base.Dispose(isDisposing);
        }
    }
}
