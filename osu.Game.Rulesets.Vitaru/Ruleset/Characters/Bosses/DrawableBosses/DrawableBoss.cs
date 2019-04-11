#region usings

using System;
using System.Collections.Generic;
using System.Linq;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables.Pieces;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters.Bosses.DrawableBosses
{
    //TODO: Seal
    public class DrawableBoss : DrawableCharacter
    {
        public readonly Boss Boss;

        public List<DrawableCluster> DrawableClusters = new List<DrawableCluster>();

        protected override string CharacterName => Boss.Name;

        //TODO: make this depend on amout of kiai time (if there even is any)
        public override double MaxHealth => 5000;

        public override Color4 PrimaryColor => Boss.PrimaryColor;

        public override Color4 SecondaryColor => Boss.SecondaryColor;

        public override Color4 ComplementaryColor => Boss.TrinaryColor;

        protected override float HitboxWidth => 64;

        private Sprite dean;

        public DrawableBoss(VitaruPlayfield playfield, Boss boss) : base(playfield)
        {
            Boss = boss;
            Position = new Vector2(256, 384 / 2f);
            AlwaysPresent = true;
            Alpha = 0;
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Hitbox.HitDetection = false;
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            const double beat_in_time = 60;

            Seal.ScaleTo(2 - 0.05f * amplitudeAdjust, beat_in_time, Easing.Out);
            using (Seal.BeginDelayedSequence(beat_in_time))
                Seal.ScaleTo(2, timingPoint.BeatLength * 2, Easing.OutQuint);

            if (effectPoint.KiaiMode)
            {
                Seal.FadeTo(0.25f * amplitudeAdjust, beat_in_time, Easing.Out);
                using (Seal.BeginDelayedSequence(beat_in_time))
                    Seal.FadeOut(timingPoint.BeatLength);
            }

            if (effectPoint.KiaiMode && Alpha < 1)
            {
                Hitbox.HitDetection = true;
                this.FadeInFromZero(timingPoint.BeatLength / 4);
                Seal.FadeTo(0.15f, timingPoint.BeatLength / 4);
            }
            if (!effectPoint.KiaiMode && Alpha > 0)
            {
                Hitbox.HitDetection = false;
                this.FadeOutFromOne(timingPoint.BeatLength);
                Seal.FadeTo(0f, timingPoint.BeatLength);
            }
        }

        protected override void MovementAnimations()
        {
            //base.MovementAnimations();

            if (Seal.Alpha > 0)
                Seal.RotateTo((float)(Clock.CurrentTime / 1000 * 90));
        }

        protected override void Update()
        {
            base.Update();

            if (BulletPiece.ExclusiveTestingHax && KiaiStillSprite.Alpha == 1)
            {
                KiaiStillSprite.Alpha = 0;
                Add(dean = new Sprite
                {
                    RelativeSizeAxes = Axes.Both,
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                    Texture = VitaruRuleset.VitaruTextures.Get("Dean"),
                });
            }
            else if (!BulletPiece.ExclusiveTestingHax && KiaiStillSprite.Alpha == 0)
            {
                KiaiStillSprite.Alpha = 1;
                Remove(dean);
                dean.Dispose();
            }

            if (BulletPiece.ExclusiveTestingHax)
                dean.Rotation += (float)Clock.ElapsedFrameTime / 4;

            if (currentDrawableCluster != null && (VitaruPlayfield.Current < currentDrawableCluster.HitObject.StartTime - currentDrawableCluster.HitObject.TimePreempt || VitaruPlayfield.Current >= currentDrawableCluster.HitObject.EndTime))
                currentDrawableCluster = null;

            DrawableCluster nextCluster = DrawableClusters.Count > 0 ? DrawableClusters.First() : null;

            restart:
            foreach (DrawableCluster drawableCluster in DrawableClusters)
            {
                Cluster cluster = drawableCluster.HitObject;
                if (VitaruPlayfield.Current < cluster.StartTime - cluster.TimePreempt || VitaruPlayfield.Current >= cluster.EndTime)
                {
                    DrawableClusters.Remove(drawableCluster);
                    goto restart;
                }

                if (drawableCluster.HitObject.StartTime < nextCluster?.HitObject.StartTime)
                    nextCluster = drawableCluster;
            }

            if (currentDrawableCluster == null)
            {
                Move(nextCluster);
                DrawableClusters.Remove(nextCluster);
            }

            if (currentDrawableCluster != null)
            {
                Cluster cluster = currentDrawableCluster.HitObject;

                if (cluster.IsSlider)
                {
                    double completionProgress = MathHelper.Clamp((VitaruPlayfield.Current - cluster.StartTime) / cluster.Duration, 0, 1);

                    if (currentDrawableCluster.Started)
                        Position = cluster.PositionAt(completionProgress);
                    else if (VitaruPlayfield.Current < cluster.StartTime - cluster.TimePreempt || VitaruPlayfield.Current >= cluster.EndTime)
                        currentDrawableCluster = null;
                }
            }
        }

        private DrawableCluster currentDrawableCluster;

        protected virtual void Move(DrawableCluster drawableCluster)
        {
            currentDrawableCluster = drawableCluster;
            Cluster cluster = drawableCluster.HitObject;

            double moveTime = Math.Max(cluster.StartTime - VitaruPlayfield.Current, 0);
            this.FadeColour(drawableCluster.AccentColour, moveTime);

            if (cluster.Position != Position)
                this.MoveTo(cluster.Position, moveTime, Easing.OutSine)
                            .OnComplete(complete);
            else
            {
                Rotation = 0;
                this.RotateTo(Rotation + 360, moveTime, Easing.InSine)
                               .OnComplete(complete);
            }

            void complete(DrawableBoss b)
            {
                //TODO: Get a new sound that IS Dean...
                //if (BulletPiece.ExclusiveTestingHax && Alpha > 0)
                    //VitaruRuleset.VitaruAudio.Sample.Get($"meme/deanery").Play();
                if (!cluster.IsSlider)
                    currentDrawableCluster = null;
            }
        }

        protected override void LoadAnimationSprites(TextureStore textures, Storage storage)
        {
            SoulContainer.Alpha = 0;
            KiaiContainer.Alpha = 1;

            KiaiLeftSprite.Alpha = 0;
            KiaiRightSprite.Alpha = 0;
            KiaiStillSprite.Alpha = 1;

            KiaiStillSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + " Kiai", storage);

            Size = new Vector2(128);
        }

        protected override void Death()
        {
            //base.Death();
            Hitbox.HitDetection = false;
            throw new NotImplementedException("Don't kill the boss or it will kill your game!");
        }

        protected override void Dispose(bool isDisposing)
        {
            DrawableClusters = new List<DrawableCluster>();
            base.Dispose(isDisposing);
        }
    }
}
