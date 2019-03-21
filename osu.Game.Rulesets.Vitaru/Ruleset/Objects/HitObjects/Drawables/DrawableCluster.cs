using System;
using System.Collections.Generic;
using osu.Framework.Graphics;
using osu.Game.Audio;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Rulesets.Scoring;
using osu.Game.Rulesets.Vitaru.Mods.Gamemodes;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables.Pieces;
using osu.Game.Rulesets.Vitaru.Ruleset.Scoring.Judgements;
using osu.Mods.Rulesets.Core.Skinning;
using osuTK;

namespace osu.Game.Rulesets.Vitaru.Ruleset.Objects.HitObjects.Drawables
{
    public class DrawableCluster : DrawableVitaruHitObject
    {
        public new readonly Cluster HitObject;
        private StarPiece starPiece;

        private bool done;

        private int currentRepeat;

        private Enemy enemy;

        public DrawableCluster(Cluster cluster, VitaruPlayfield playfield) : base(cluster, playfield)
        {
            AlwaysPresent = true;

            HitObject = cluster;

            Size = new Vector2(30);
            Position = HitObject.Position;
            Alpha = 0;
            HitObject.PositionChanged += _ =>
            {
                Position = HitObject.Position;
                //TODO: Move bullets too?
            };

            if (!cluster.IsSlider && !cluster.IsSpinner)
            {
                double endTime = HitObject.StartTime + HitObject.TimePreempt * 2 - HitObject.TimeFadein;
                HitObject.EndTime = endTime;
            }

            if (cluster.IsSlider)
            {
                foreach (SampleInfo info in cluster.GetRepeatSamples(0))
                {
                    SymcolSkinnableSound sound;
                    SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                    AddInternal(sound);
                }
            }
            else
            {
                foreach (SampleInfo info in cluster.BetterSamples)
                {
                    SymcolSkinnableSound sound;
                    SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                    AddInternal(sound);
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            if (HitObject.IsSlider)
            {
                double completionProgress = MathHelper.Clamp((VitaruPlayfield.Current - HitObject.StartTime) / HitObject.Duration, 0, 1);
                int repeat = HitObject.SpanAt(completionProgress);

                if (Started && !done)
                {
                    starPiece.Position = HitObject.PositionAt(completionProgress);
                    if (!(Gamemode is DodgeGamemode))
                        enemy.Position = HitObject.PositionAt(completionProgress);
                }

                if (repeat > currentRepeat)
                {
                    PlayBetterRepeatSamples(repeat + 1);
                    currentRepeat = repeat;
                }

                if (HitObject.EndTime <= VitaruPlayfield.Current && Started && !done)
                    done = true;
            }
        }

        protected void PlayBetterRepeatSamples(int repeat)
        {
            PlayBetterSamples();

            foreach (SymcolSkinnableSound sound in SymcolSkinnableSounds)
                sound.Delete();

            SymcolSkinnableSounds = new List<SymcolSkinnableSound>();

            foreach (SampleInfo info in HitObject.GetRepeatSamples(repeat))
            {
                SymcolSkinnableSound sound;

                HitObject.SampleControlPoint = HitObject.GetSampleControlPoint(repeat);

                SymcolSkinnableSounds.Add(sound = GetSkinnableSound(info));
                AddInternal(sound);
            }
        }

        protected override void Preempt()
        {
            base.Preempt();

            foreach (var o in HitObject.NestedHitObjects)
            {
                if (o is Bullet b)
                {
                    if (DrawableBullet.BoundryHacks || Gamemode is DodgeGamemode)
                    {
                        b.Angle += getPlayerAngle() - (float)Math.PI / 2;
                        b.SliderType = b.SliderType;
                    }

                    if (b.ShootPlayer)
                        b.Position = VitaruPlayfield.PlayerPosition;

                    DrawableBullet drawableBullet = Gamemode.GetDrawableBullet(b, VitaruPlayfield);
                    CurrentPlayfield.Add(drawableBullet);
                    AddNested(drawableBullet);
                }
                else if (o is Laser l)
                {
                    if (DrawableBullet.BoundryHacks || Gamemode is DodgeGamemode)
                        l.Angle += getPlayerAngle() - (float)Math.PI / 2;

                    DrawableLaser drawableLaser = Gamemode.GetDrawableLaser(l, VitaruPlayfield);
                    CurrentPlayfield.Add(drawableLaser);
                    AddNested(drawableLaser);
                }
            }

            CurrentPlayfield.Add(starPiece = new StarPiece
            {
                Alpha = 0,
                Masking = true,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.Centre,
                Size = Size,
                Colour = AccentColour
            });

            if (!(Gamemode is DodgeGamemode))
            {
                CurrentPlayfield.Add(enemy = new Enemy(VitaruPlayfield, this)
                {
                    Alpha = 0,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    Depth = 5,
                    //MaxHealth = cluster.EnemyHealth,
                    Team = HitObject.Team,
                });

                enemy.Position = getClusterStartPosition();
                enemy.FadeIn(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt))
                    .MoveTo(HitObject.Position, HitObject.TimePreempt, Easing.OutSine);
            }
            else
            {
                starPiece.FadeInFromZero(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt))
                    .MoveTo(HitObject.Position, HitObject.TimePreempt);
            }

            this.FadeInFromZero(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt));

            starPiece.Position = getClusterStartPosition();

            Position = HitObject.Position;
            Size = new Vector2(64);
        }

        protected override void Start()
        {
            base.Start();

            if (!HitObject.IsSlider && !HitObject.IsSpinner)
                PlayBetterSamples();
            else
                PlayBetterRepeatSamples(1);
        }

        protected override void End()
        {
            base.End();

            if (HitObject.IsSpinner)
                PlayBetterSamples();

            if (!(Gamemode is DodgeGamemode))
                enemy.MoveTo(getClusterStartPosition(), HitObject.TimeUnPreempt * 2, Easing.InQuad)
                    .ScaleTo(new Vector2(0.5f), HitObject.TimeUnPreempt, Easing.InQuad)
                    .FadeOut(HitObject.TimeUnPreempt, Easing.InQuad);

            this.FadeOut(HitObject.TimePreempt / 2)
                .MoveTo(getClusterStartPosition(), HitObject.TimePreempt * 2, Easing.InQuad);

            starPiece.FadeOut(HitObject.TimePreempt / 2)
                .ScaleTo(new Vector2(0.1f), HitObject.TimePreempt / 2);
        }

        protected override void UnPreempt()
        {
            base.UnPreempt();
            Dispose();
        }

        public void Death(Enemy enemy)
        {
            ApplyResult(r =>
            {
                VitaruJudgementResult v = (VitaruJudgementResult)r;
                v.Type = HitResult.Great;
                v.VitaruJudgement.BonusScore = true;
            });
            if (VitaruPlayfield.Current < HitObject.StartTime && !Started)
            {
                enemy.FadeOut(HitObject.StartTime - VitaruPlayfield.Current);
                starPiece.FadeInFromZero(HitObject.StartTime - VitaruPlayfield.Current);
            }
            else if (VitaruPlayfield.Current < HitObject.EndTime && Started)
            {
                enemy.FadeOutFromOne(200)
                     .ScaleTo(new Vector2(1.5f), 200);
                starPiece.FadeInFromZero(200);
            }
            else if (VitaruPlayfield.Current >= HitObject.EndTime)
            {
                enemy.ClearTransforms();
                enemy.FadeOutFromOne(200);
            }
        }

        private Vector2 getClusterStartPosition()
        {
            if (HitObject.Position.X <= 384f / 2 && HitObject.Position.Y <= 512f / 2)
                return HitObject.Position - new Vector2(384f / 2, 512f / 2);
            if (HitObject.Position.X > 384f / 2 && HitObject.Position.Y <= 512f / 2)
                return new Vector2(HitObject.Position.X + 384f / 2, HitObject.Position.Y - 512f / 2);
            if (HitObject.Position.X > 384f / 2 && HitObject.Position.Y > 512f / 2)
                return HitObject.Position + new Vector2(384f / 2, 512f / 2);

            return new Vector2(HitObject.Position.X - 384f / 2, HitObject.Position.Y + 512f / 2);
        }

        private float getPlayerAngle()
        {
            if (VitaruPlayfield.Player == null)
                return 0;
            return (float)Math.Atan2(VitaruPlayfield.PlayerPosition.Y - Position.Y, VitaruPlayfield.PlayerPosition.X - Position.X);
        }

        protected override void Dispose(bool isDisposing)
        {
            starPiece.Dispose();
            starPiece = null;

            if (!(Gamemode is DodgeGamemode))
            {
                enemy.Dispose();
                enemy = null;
            }

            VitaruPlayfield.Remove(this);
            base.Dispose(isDisposing);
        }
    }
}
