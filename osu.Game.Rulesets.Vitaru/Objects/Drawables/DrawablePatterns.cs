using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using System.Linq;
using osu.Game.Skinning;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Audio;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawablePattern : DrawableVitaruHitObject
    {
        private readonly VitaruGamemode currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);

        public static int PatternCount;
        private readonly Pattern pattern;
        private StarPiece starPiece;

        private bool done;

        private int currentRepeat;

        private readonly double endTime;
        
        private Enemy enemy;

        private AudioManager audio;

        public DrawablePattern(Pattern pattern, VitaruPlayfield playfield) : base(pattern, playfield)
        {
            AlwaysPresent = true;

            this.pattern = pattern;

            Size = new Vector2(30);
            Position = this.pattern.Position;
            Alpha = 0;
            HitObject.PositionChanged += _ => Position = HitObject.Position;

            if (!pattern.IsSlider && !pattern.IsSpinner)
            {
                endTime = this.pattern.StartTime + HitObject.TimePreempt * 2 - HitObject.TimeFadein;
                this.pattern.EndTime = endTime;
            }
            else if (pattern.IsSlider)
                endTime = this.pattern.EndTime + HitObject.TimePreempt * 2 - HitObject.TimeFadein;

            PatternCount++;
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            this.audio = audio;
        }

        protected override void Update()
        {
            base.Update();

            if (pattern.IsSlider)
            {
                double completionProgress = MathHelper.Clamp((Time.Current - pattern.StartTime) / pattern.Duration, 0, 1);
                int repeat = pattern.SpanAt(completionProgress);

                if (Started && !done)
                {
                    starPiece.Position = pattern.PositionAt(completionProgress);
                    if (currentGameMode != VitaruGamemode.Dodge)
                        enemy.Position = pattern.PositionAt(completionProgress);
                }

                if (repeat > currentRepeat)
                {
                    if (repeat < pattern.RepeatCount + 2)
                        PlaySamples();
                    currentRepeat = repeat;
                }

                if (pattern.EndTime <= Time.Current && Started && !done)
                    done = true;
            }
        }

        protected override void Load()
        {
            base.Load();

            VitaruPlayfield.GameField.Add(starPiece = new StarPiece
            {
                Alpha = 0,
                Masking = true,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.Centre,
                Size = Size,
                Colour = AccentColour
            });

            if (currentGameMode != VitaruGamemode.Dodge)
            {
                //load the enemy
                VitaruPlayfield.GameField.Add(enemy = new Enemy(VitaruPlayfield, pattern, this)
                {
                    Alpha = 0,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    Depth = 5,
                    //MaxHealth = pattern.EnemyHealth,
                    Team = 1,
                });

                if (pattern.IsSpinner)
                    enemy.Abstraction = 2;
                else if (!pattern.IsSlider)
                    enemy.Abstraction = 1;

                enemy.Position = getPatternStartPosition();
                enemy.FadeIn(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt))
                    .MoveTo(pattern.Position, HitObject.TimePreempt);
            }
            else
            {
                starPiece.FadeInFromZero(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt))
                    .MoveTo(pattern.Position, HitObject.TimePreempt);
            }

            this.FadeInFromZero(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt));

            starPiece.Position = getPatternStartPosition();


            Position = pattern.Position;
            Size = new Vector2(64);

            //Load the bullets
            foreach (var o in pattern.NestedHitObjects)
            {
                Bullet b = (Bullet)o;
                DrawableBullet drawableBullet = new DrawableBullet(b, this, VitaruPlayfield);
                VitaruPlayfield.GameField.Add(drawableBullet);
                AddNested(drawableBullet);
            }
        }

        protected override void Start()
        {
            base.Start();

            PlaySamples();
        }

        protected override void End()
        {
            base.End();

            if (currentGameMode != VitaruGamemode.Dodge)
                enemy.MoveTo(getPatternStartPosition(), HitObject.TimePreempt * 2, Easing.InQuint)
                    .Delay(HitObject.TimePreempt * 2 - HitObject.TimeFadein)
                    .ScaleTo(new Vector2(0.5f), HitObject.TimeFadein, Easing.InQuint)
                    .FadeOut(HitObject.TimeFadein, Easing.InQuint)
                    .Expire();

            this.FadeOut(HitObject.TimePreempt / 2)
                .MoveTo(getPatternStartPosition(), HitObject.TimePreempt * 2, Easing.InQuint)
                .Expire();

            starPiece.FadeOut(HitObject.TimePreempt / 2)
                .ScaleTo(new Vector2(0.1f), HitObject.TimePreempt / 2)
                .Expire();
        }

        protected override void Unload()
        {
            base.Unload();

            if (currentGameMode != VitaruGamemode.Dodge)
            {
                VitaruPlayfield.GameField.Remove(enemy);
                enemy.Dispose();
            }

            VitaruPlayfield.GameField.Remove(starPiece);
            starPiece.Dispose();

            Expire();
        }

        public void PlaySamples(int repeat)
        {
            foreach(SampleInfo info in pattern.RepeatSamples[repeat])
            {
                SampleChannel sample = audio.Sample.Get($"Gameplay\\{info.Bank}-{info.Name}");
                sample?.Play();
            }
        }

        private Vector2 getPatternStartPosition()
        {
            Vector2 patternStartPosition;

            if (pattern.Position.X <= 384f / 2 && pattern.Position.Y <= 512f / 2)
                patternStartPosition = pattern.Position - new Vector2(384f / 2, 512f / 2);
            else if (pattern.Position.X > 384f / 2 && pattern.Position.Y <= 512f / 2)
                patternStartPosition = new Vector2(pattern.Position.X + 384f / 2, pattern.Position.Y - 512f / 2);
            else if (pattern.Position.X > 384f / 2 && pattern.Position.Y > 512f / 2)
                patternStartPosition = pattern.Position + new Vector2(384f / 2, 512f / 2);
            else
                patternStartPosition = new Vector2(pattern.Position.X - 384f / 2, pattern.Position.Y + 512f / 2);

            return patternStartPosition;
        }

        protected override void Dispose(bool isDisposing)
        {
            PatternCount--;
            base.Dispose(isDisposing);
        }
    }
}
