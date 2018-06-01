using OpenTK;
using osu.Framework.Graphics;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.Objects.Drawables.Pieces;
using osu.Game.Rulesets.Objects.Types;
using osu.Framework.Allocation;
using osu.Framework.Audio;
using osu.Game.Rulesets.Vitaru.Characters;
using osu.Game.Skinning;
using osu.Game.Audio;
using System.Collections.Generic;
using System.Linq;
using Symcol.Rulesets.Core.Skinning;

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

        private List<SymcolSkinnableSound> samples = new List<SymcolSkinnableSound>();
        private List<SymcolSkinnableSound> repeatSamples = new List<SymcolSkinnableSound>();

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

            if (!pattern.IsSlider)
            {
                string bank = "normal";

                if (pattern.Drum)
                    bank = "drum";
                else if (pattern.Soft)
                    bank = "soft";

                samples.Add(new SymcolSkinnableSound(new SampleInfo
                {
                    Bank = bank,
                    Name = "hitnormal",
                    Volume = pattern.Volume > 0 ? pattern.Volume : HitObject.SampleControlPoint.SampleVolume,
                    Namespace = SampleNamespace
                }));

                if (pattern.Whistle)
                    samples.Add(new SymcolSkinnableSound(new SampleInfo
                    {
                        Bank = bank,
                        Name = "hitwhistle",
                        Volume = pattern.Volume > 0 ? pattern.Volume : HitObject.SampleControlPoint.SampleVolume,
                        Namespace = SampleNamespace
                    }));
                if (pattern.Finish)
                    samples.Add(new SymcolSkinnableSound(new SampleInfo
                    {
                        Bank = bank,
                        Name = "hitfinish",
                        Volume = pattern.Volume > 0 ? pattern.Volume : HitObject.SampleControlPoint.SampleVolume,
                        Namespace = SampleNamespace
                    }));
                if (pattern.Clap)
                    samples.Add(new SymcolSkinnableSound(new SampleInfo
                    {
                        Bank = bank,
                        Name = "hitclap",
                        Volume = pattern.Volume > 0 ? pattern.Volume : HitObject.SampleControlPoint.SampleVolume,
                        Namespace = SampleNamespace
                    }));

                foreach (SkinnableSound sound in samples)
                    Add(sound);
            }

            if (pattern.IsSlider)
            {
                endTime = this.pattern.EndTime + HitObject.TimePreempt * 2 - HitObject.TimeFadein;

                restart:
                foreach (SampleInfo info in pattern.BetterRepeatSamples.First())
                {
                    SymcolSkinnableSound sound;
                    repeatSamples.Add(sound = new SymcolSkinnableSound(new SampleInfo
                    {
                        Bank = info.Bank,
                        Name = info.Name,
                        Volume = info.Volume,
                        Namespace = SampleNamespace
                    }));
                    Add(sound);
                    pattern.BetterRepeatSamples.First().Remove(info);
                    goto restart;
                }
                pattern.BetterRepeatSamples.Remove(pattern.BetterRepeatSamples.First());
            }
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

                //TODO: make Dean not so loud
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
                    {
                        foreach (SkinnableSound sound in repeatSamples)
                        {
                            sound.Play();
                            Remove(sound);
                        }

                        restart:
                        foreach (SampleInfo info in pattern.BetterRepeatSamples.First())
                        {
                            SymcolSkinnableSound newSound;
                            repeatSamples.Add(newSound = new SymcolSkinnableSound(new SampleInfo
                            {
                                Bank = info.Bank,
                                Name = info.Name,
                                Volume = info.Volume,
                                Namespace = SampleNamespace
                            }));
                            Add(newSound);
                            pattern.BetterRepeatSamples.First().Remove(info);
                            goto restart;
                        }
                        pattern.BetterRepeatSamples.Remove(pattern.BetterRepeatSamples.First());
                    }
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
                if (o is Bullet b)
                {
                    if (DrawableBullet.BoundryHacks)// && (pattern.PatternID == 2 || pattern.PatternID == 3))
                    {
                        b.BulletAngle += getPlayerAngle() - Math.PI / 2;
                        b.SliderType = b.SliderType;
                    }

                    DrawableBullet drawableBullet = new DrawableBullet(b, VitaruPlayfield);
                    VitaruPlayfield.GameField.Add(drawableBullet);
                    AddNested(drawableBullet);
                }
            }

            if (VitaruPlayfield.Boss != null)
                VitaruPlayfield.Boss.Free = true;

            if (!pattern.IsSlider)
                foreach (SkinnableSound sound in samples)
                {
                    sound.Play();
                    Remove(sound);
                }
            else
                foreach (SkinnableSound sound in repeatSamples)
                {
                    sound.Play();
                    Remove(sound);
                }
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

        public override void Delete()
        {
            foreach (SymcolSkinnableSound sound in samples)
                sound.Delete();
            foreach (SymcolSkinnableSound sound in repeatSamples)
                sound.Delete();

            base.Delete();
        }

        private Vector2 getPatternStartPosition()
        {
            if (pattern.Position.X <= 384f / 2 && pattern.Position.Y <= 512f / 2)
                return pattern.Position - new Vector2(384f / 2, 512f / 2);
            else if (pattern.Position.X > 384f / 2 && pattern.Position.Y <= 512f / 2)
                return new Vector2(pattern.Position.X + 384f / 2, pattern.Position.Y - 512f / 2);
            else if (pattern.Position.X > 384f / 2 && pattern.Position.Y > 512f / 2)
                return pattern.Position + new Vector2(384f / 2, 512f / 2);
            else
                return new Vector2(pattern.Position.X - 384f / 2, pattern.Position.Y + 512f / 2);
        }

        private double getPlayerAngle()
        {
            return Math.Atan2((VitaruPlayfield.Player.Position.Y - Position.Y), (VitaruPlayfield.Player.Position.X - Position.X));
        }
    }
}
