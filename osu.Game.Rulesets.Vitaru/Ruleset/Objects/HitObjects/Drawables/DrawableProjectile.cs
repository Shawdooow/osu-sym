using System;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using Symcol.Base.Extentions;
// ReSharper disable InconsistentNaming

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables
{
    public class DrawableProjectile : DrawableVitaruHitObject
    {
        public new readonly Projectile HitObject;

        public VitaruHitbox Hitbox;

        //Can be set for the Graze ScoringMetric
        public double MinDistance = double.MaxValue;

        //Should be set to true when a character is hit
        public bool Hit;

        //Set to "true" when a judgement should be returned
        public bool ForceJudgement;

        //Set to "true" when a judgement has been returned
        protected bool ReturnedJudgement;

        //return a great
        public bool ReturnGreat;

        public Action OnHit;

        public DrawableProjectile(Projectile projectile, VitaruPlayfield playfield)
            : base(projectile, playfield)
        {
            HitObject = projectile;
            Alpha = 0;
            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;
        }

        protected override void CheckForResult(bool userTriggered, double timeOffset)
        {
            base.CheckForResult(userTriggered, timeOffset);

            if (ReturnedJudgement) return;

            if (ForceJudgement)
            {
                ReturnedJudgement = true;

                if (MinDistance <= 64)
                    ApplyResult(HitResult.Great);
                else if (MinDistance <= 128)
                    ApplyResult(HitResult.Good);
                else
                    ApplyResult(HitResult.Meh);
            }

            else if (Hit)
                HitCharacter();

            else if (ReturnGreat)
            {
                ApplyResult(HitResult.Great);
                End();
                ReturnedJudgement = true;
            }
        }

        protected virtual void ApplyResult(HitResult type) =>
            ApplyResult(r =>
            {
                r.Type = type;
                ((VitaruJudgement)r.Judgement).Weight = Weight(MinDistance);
            });

        protected virtual double Weight(double distance)
        {
            return distance > 128 ? 0 : 500 / Math.Max(distance, 1);
        }

        protected virtual void HitCharacter()
        {

        }

        protected override void Update()
        {
            base.Update();

            if (OnHit != null && Hit)
            {
                OnHit();
                OnHit = null;
            }
        }

        protected double GetHDAlpha(double distance) => Math.Min(SymcolMath.Scale(distance, 128, 265, 1, 0), 1);

        protected double GetFLAlpha(double distance) => Math.Max(SymcolMath.Scale(distance, 0, 256), 0);

        protected override void UnPreempt()
        {
            base.UnPreempt();
            Delete();
        }

        public override void Delete()
        {
            CurrentPlayfield.Remove(this);
            base.Delete();
        }
    }
}
