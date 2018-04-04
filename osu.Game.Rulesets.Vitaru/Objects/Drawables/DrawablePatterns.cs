using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Extensions.Color4Extensions;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables
{
    public class DrawablePattern : DrawableVitaruHitObject
    {
        private readonly VitaruGamemode currentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<VitaruGamemode>(VitaruSetting.GameMode);

        public static int PatternCount;
        private readonly Pattern pattern;
        private Container energyCircle;

        private bool done;

        private int currentRepeat;

        private readonly double endTime;
        
        private Enemy enemy;

        public DrawablePattern(Pattern pattern, VitaruPlayfield playfield) : base(pattern, playfield)
        {
            AlwaysPresent = true;

            this.pattern = pattern;

            if (!pattern.IsSlider && !pattern.IsSpinner)
            {
                endTime = this.pattern.StartTime + HitObject.TimePreempt * 2 - HitObject.TimeFadein;
                this.pattern.EndTime = endTime;
            }
            else if (pattern.IsSlider)
                endTime = this.pattern.EndTime + HitObject.TimePreempt * 2 - HitObject.TimeFadein;

            PatternCount++;
        }

        protected override void Update()
        {
            base.Update();

            if (pattern.IsSlider)
            {
                double completionProgress = MathHelper.Clamp((Time.Current - pattern.StartTime) / pattern.Duration, 0, 1);
                int repeat = pattern.RepeatAt(completionProgress);

                if (Started && !done)
                {
                    energyCircle.Position = pattern.PositionAt(completionProgress);
                    if (currentGameMode != VitaruGamemode.Dodge)
                        enemy.Position = pattern.PositionAt(completionProgress);
                }

                if (repeat > currentRepeat)
                {
                    if (repeat < pattern.RepeatCount + 1)
                        PlaySamples();
                    currentRepeat = repeat;
                }

                if (pattern.EndTime <= Time.Current && Started && !done)
                {
                    PlaySamples();
                    done = true;
                }
            }
        }

        protected override void Load()
        {
            base.Load();

            VitaruPlayfield.CharacterField.Add(energyCircle = new Container
            {
                Alpha = 0,
                Masking = true,
                Anchor = Anchor.TopLeft,
                Origin = Anchor.Centre,
                Size = new Vector2(30),
                CornerRadius = 30f / 2,
                BorderThickness = 10,
                BorderColour = AccentColour,

                Children = new Drawable[]
                {
                    new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    }
                },
                EdgeEffect = new EdgeEffectParameters
                {
                    Type = EdgeEffectType.Shadow,
                    Colour = AccentColour.Opacity(0.5f),
                    Radius = Width / 2,
                }
            });

            if (currentGameMode != VitaruGamemode.Dodge)
            {
                //load the enemy
                VitaruPlayfield.CharacterField.Add(enemy = new Enemy(VitaruPlayfield, pattern, this)
                {
                    Alpha = 0,
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.Centre,
                    Depth = 5,
                    MaxHealth = pattern.EnemyHealth,
                    Team = 1,
                });

                enemy.Position = getPatternStartPosition();
                enemy.FadeIn(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt))
                    .MoveTo(pattern.Position, HitObject.TimePreempt);
            }
            else
            {
                energyCircle.Alpha = 0;
                energyCircle.FadeIn(Math.Min(HitObject.TimeFadein * 2, HitObject.TimePreempt))
                    .MoveTo(pattern.Position, HitObject.TimePreempt);
            }

            energyCircle.Position = getPatternStartPosition();


            Position = pattern.Position;
            Size = new Vector2(64);

            //Load the bullets
            foreach (var o in pattern.NestedHitObjects)
            {
                Bullet b = (Bullet)o;
                DrawableBullet drawableBullet = new DrawableBullet(b, this, VitaruPlayfield);
                VitaruPlayfield.BulletField.Add(drawableBullet);
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

            this.MoveTo(getPatternStartPosition(), HitObject.TimePreempt * 2, Easing.InQuint)
                .Expire();

            energyCircle.FadeOut(HitObject.TimePreempt / 2)
                .ScaleTo(new Vector2(0.1f), HitObject.TimePreempt / 2)
                .Expire();
        }

        protected override void Unload()
        {
            base.Unload();

            VitaruPlayfield.CharacterField.Remove(enemy);
            VitaruPlayfield.CharacterField.Remove(energyCircle);

            enemy.Dispose();
            energyCircle.Dispose();

            Expire();
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
