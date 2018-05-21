using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Audio;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Framework.Audio.Sample;
using osu.Game.Rulesets.Vitaru.Characters;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawablePattern : DrawableVitaruHitObject
    {
        private readonly Gamemodes gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

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
        }

        [BackgroundDependencyLoader]
        private void load(AudioManager audio)
        {
            this.audio = audio;
        }

        protected override void Update()
        {
            base.Update();

            if (VitaruPlayfield.Boss != null && VitaruPlayfield.Boss.Free && Time.Current >= HitObject.StartTime - pattern.TimePreempt)
            {
                double moveTime = HitObject.StartTime - Time.Current;
                VitaruPlayfield.Boss.MoveTo(Position, moveTime < 100 ? 100 : moveTime, Easing.OutSine);
                VitaruPlayfield.Boss.Free = false;

                if (VitaruPlayfield.Boss.Alpha == 1 && BulletPiece.ExclusiveTestingHax)
                    VitaruRuleset.VitaruAudio.Sample.Get($"skeletron").Play();
            }

            if (pattern.IsSlider)
            {
                double completionProgress = MathHelper.Clamp((Time.Current - pattern.StartTime) / pattern.Duration, 0, 1);
                int repeat = pattern.SpanAt(completionProgress);

                if (Started && !done)
                {
                    starPiece.Position = pattern.PositionAt(completionProgress);
                    if (gamemode != Gamemodes.Dodge)
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

            if (gamemode != Gamemodes.Dodge)
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
        }

        protected override void Start()
        {
            base.Start();

            foreach (Bullet bullet in pattern.GetBullets())
                pattern.AddNested(bullet);

            //Load the bullets
            foreach (var o in pattern.NestedHitObjects)
            {
                Bullet b = (Bullet)o;
                DrawableBullet drawableBullet = new DrawableBullet(b, VitaruPlayfield);
                VitaruPlayfield.GameField.Add(drawableBullet);
                AddNested(drawableBullet);
            }

            if (VitaruPlayfield.Boss != null)
                VitaruPlayfield.Boss.Free = true;

            PlaySamples();
        }

        protected override void End()
        {
            base.End();

            if (gamemode != Gamemodes.Dodge)
                enemy.MoveTo(getPatternStartPosition(), HitObject.TimePreempt * 2, Easing.InQuint)
                    .Delay(HitObject.TimePreempt * 2 - HitObject.TimeFadein)
                    .ScaleTo(new Vector2(0.5f), HitObject.TimeFadein, Easing.InQuint)
                    .FadeOut(HitObject.TimeFadein, Easing.InQuint);

            this.FadeOut(HitObject.TimePreempt / 2)
                .MoveTo(getPatternStartPosition(), HitObject.TimePreempt * 2, Easing.InQuint);

            starPiece.FadeOut(HitObject.TimePreempt / 2)
                .ScaleTo(new Vector2(0.1f), HitObject.TimePreempt / 2);
        }

        protected override void Unload()
        {
            base.Unload();

            if (gamemode != Gamemodes.Dodge)
                enemy.Delete();

            starPiece.Delete();

            if (Editor)
                Expire();
            else
                Delete();
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
    }
}
