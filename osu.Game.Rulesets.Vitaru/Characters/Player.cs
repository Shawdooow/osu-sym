﻿using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using System;
using System.Collections.Generic;
using static osu.Game.Rulesets.Vitaru.UI.Cursor.GameplayCursor;

namespace osu.Game.Rulesets.Vitaru.Characters
{
    public abstract class Player : Character, IKeyBindingHandler<VitaruAction>
    {
        #region Fields
        protected readonly Gamemodes CurrentGameMode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        public const double DefaultHealth = 100;

        public static readonly Color4 DefaultColor = Color4.Yellow;

        public override double MaxHealth => DefaultHealth;

        public override Color4 PrimaryColor => DefaultColor;

        public int ScoreZone = 100;

        public double SpeedMultiplier = 1;

        public Dictionary<VitaruAction, bool> Actions = new Dictionary<VitaruAction, bool>();

        //(MinX,MaxX,MinY,MaxY)
        protected Vector4 PlayerBounds
        {
            get
            {
                if (CurrentGameMode == Gamemodes.Touhosu)
                    return new Vector4(0, 512, 0, 820);
                else
                    return new Vector4(0, 512, 0, 820);
            }
        }

        protected readonly Container Cursor;

        private double lastQuarterBeat = -1;
        private double nextHalfBeat = -1;
        private double nextQuarterBeat = -1;
        private double beatLength = 1000;
        #endregion

        public Player(VitaruPlayfield playfield) : base(playfield)
        {
            Actions[VitaruAction.Up] = false;
            Actions[VitaruAction.Down] = false;
            Actions[VitaruAction.Left] = false;
            Actions[VitaruAction.Right] = false;
            Actions[VitaruAction.Slow] = false;
            Actions[VitaruAction.Shoot] = false;

            VitaruPlayfield.GameField.Add(Cursor = new Container
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.Centre,
                Size = new Vector2(4),
                CornerRadius = 2,
                Alpha = 0.2f,
                Masking = true,

                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        protected override void LoadAnimationSprites(TextureStore textures, Storage storage)
        {
            base.LoadAnimationSprites(textures, storage);

            RightSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "Right", storage);
            KiaiRightSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "KiaiRight", storage);
        }

        protected override void OnNewBeat(int beatIndex, TimingControlPoint timingPoint, EffectControlPoint effectPoint, TrackAmplitudes amplitudes)
        {
            base.OnNewBeat(beatIndex, timingPoint, effectPoint, amplitudes);

            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            beatLength = timingPoint.BeatLength;

            onHalfBeat();
            lastQuarterBeat = Time.Current;
            nextHalfBeat = Time.Current + timingPoint.BeatLength / 2;
            nextQuarterBeat = Time.Current + timingPoint.BeatLength / 4;

            const double beat_in_time = 60;

            Seal.Sign.ScaleTo(1 - 0.02f * amplitudeAdjust, beat_in_time, Easing.Out);
            using (Seal.Sign.BeginDelayedSequence(beat_in_time))
                Seal.Sign.ScaleTo(1, beatLength * 2, Easing.OutQuint);

            if (effectPoint.KiaiMode && CurrentGameMode != Gamemodes.Touhosu)
            {
                Seal.Sign.FadeTo(0.25f * amplitudeAdjust, beat_in_time, Easing.Out);
                using (Seal.Sign.BeginDelayedSequence(beat_in_time))
                    Seal.Sign.FadeOut(beatLength);
            }

            if (effectPoint.KiaiMode && SoulContainer.Alpha == 1)
            {
                if (!Dead && CurrentGameMode != Gamemodes.Gravaru)
                {
                    KiaiContainer.FadeInFromZero(timingPoint.BeatLength / 4);
                    SoulContainer.FadeOutFromOne(timingPoint.BeatLength / 4);
                }

                if (CurrentGameMode != Gamemodes.Touhosu)
                    Seal.Sign.FadeTo(0.15f, timingPoint.BeatLength / 4);
            }
            if (!effectPoint.KiaiMode && KiaiContainer.Alpha == 1)
            {
                if (!Dead && CurrentGameMode != Gamemodes.Gravaru)
                {
                    SoulContainer.FadeInFromZero(timingPoint.BeatLength);
                    KiaiContainer.FadeOutFromOne(timingPoint.BeatLength);
                }

                if (CurrentGameMode != Gamemodes.Touhosu)
                    Seal.Sign.FadeTo(0f, timingPoint.BeatLength);
            }
        }

        private void onHalfBeat()
        {
            nextHalfBeat = -1;

            if (Actions[VitaruAction.Shoot])
                PatternWave();
        }

        private void onQuarterBeat()
        {
            lastQuarterBeat = nextQuarterBeat;
            nextQuarterBeat += beatLength / 4;

            if (CanHeal)
            {
                CanHeal = false;

                Heal(1d);

                if (CurrentGameMode != Gamemodes.Touhosu)
                {
                    Seal.Sign.Alpha = 0.2f;
                    Seal.Sign.FadeOut(beatLength / 4);
                }
            }
        }

        protected override void Update()
        {
            base.Update();

            foreach (Drawable draw in VitaruPlayfield.GameField.QuarterAbstraction)
                if (draw is DrawableBullet bullet && bullet.Hitbox != null)
                    ParseBullet(bullet);

            foreach (Drawable draw in VitaruPlayfield.GameField.HalfAbstraction)
                if (draw is DrawableBullet bullet && bullet.Hitbox != null)
                    ParseBullet(bullet);

            foreach (Drawable draw in VitaruPlayfield.GameField.FullAbstraction)
                if (draw is DrawableBullet bullet && bullet.Hitbox != null)
                    ParseBullet(bullet);

            Position = GetNewPlayerPosition(0.25d);
            Cursor.Position = VitaruCursor.CenterCircle.ToSpaceOfOtherDrawable(Vector2.Zero, Parent) + new Vector2(6);

            if (nextHalfBeat <= Time.Current && nextHalfBeat != -1)
                onHalfBeat();

            if (nextQuarterBeat <= Time.Current && nextQuarterBeat != -1)
                onQuarterBeat();
        }

        protected override void ParseBullet(DrawableBullet bullet)
        {
            base.ParseBullet(bullet);

            //Not sure why this offset is needed atm
            Vector2 object2Pos = bullet.ToSpaceOfOtherDrawable(Vector2.Zero, this) + new Vector2(6);
            float distance = (float)Math.Sqrt(Math.Pow(object2Pos.X, 2) + Math.Pow(object2Pos.Y, 2));
            float edgeDistance = distance - (bullet.Width / 2 + Hitbox.Width / 2);

            if (edgeDistance < 64 && bullet.Bullet.Team != Team)
                CanHeal = true;

            if (CurrentGameMode == Gamemodes.Dodge)
                edgeDistance *= 1.5f;

            if (edgeDistance <= 64 && bullet.ScoreZone < 300)
                bullet.ScoreZone = 300;
            else if (edgeDistance <= 128 && bullet.ScoreZone < 100)
                bullet.ScoreZone = 100;
        }

        protected virtual Vector2 GetNewPlayerPosition(double playerSpeed)
        {
            Vector2 playerPosition = Position;

            double yTranslationDistance = playerSpeed * Clock.ElapsedFrameTime * SpeedMultiplier;
            double xTranslationDistance = playerSpeed * Clock.ElapsedFrameTime * SpeedMultiplier;

            if (Actions[VitaruAction.Slow])
            {
                xTranslationDistance /= 2;
                yTranslationDistance /= 2;
            }

            if (Actions[VitaruAction.Up])
                playerPosition.Y -= (float)yTranslationDistance;
            if (Actions[VitaruAction.Left])
                playerPosition.X -= (float)xTranslationDistance;
            if (Actions[VitaruAction.Down])
                playerPosition.Y += (float)yTranslationDistance;
            if (Actions[VitaruAction.Right])
                playerPosition.X += (float)xTranslationDistance;

            playerPosition = Vector2.ComponentMin(playerPosition, PlayerBounds.Yw);
            playerPosition = Vector2.ComponentMax(playerPosition, PlayerBounds.Xz);

            return playerPosition;
        }

        protected override void Death()
        {
            //base.Death();
        }

        #region Shooting Handling
        private void bulletAddRad(double speed, double angle, Color4 color, SliderType type = SliderType.Straight)
        {
            DrawableBullet drawableBullet;

            VitaruPlayfield.GameField.Add(drawableBullet = new DrawableBullet(new Bullet
            {
                StartTime = Time.Current,
                Position = Position,
                BulletAngle = angle,
                BulletSpeed = speed,
                BulletDiameter = 16,
                BulletDamage = 20,
                ColorOverride = color,
                Team = Team,
                SliderType = type,
                Curviness = 0.25d,
                DummyMode = true,
                Abstraction = 3,
            }, VitaruPlayfield));

            //if (vampuric)
            //drawableBullet.OnHit = () => Heal(0.5f);
            drawableBullet.MoveTo(Position);
        }

        protected void PatternWave()
        {
            const int numberbullets = 3;
            double directionModifier = -0.2d;
            Color4 color = PrimaryColor;
            SliderType type;

            double cursorAngle = 0;

            if (Actions[VitaruAction.Slow])
            {
                cursorAngle = (MathHelper.RadiansToDegrees(Math.Atan2((Cursor.Position.Y - Position.Y), (Cursor.Position.X - Position.X))) + 90 + Rotation) - 12;
                directionModifier = 0.1d;
            }

            for (int i = 1; i <= numberbullets; i++)
            {
                if (i == 1)
                    type = SliderType.CurveRight;
                else if (i == 2)
                    type = SliderType.Straight;
                else
                    type = SliderType.CurveLeft;

                //-90 = up
                bulletAddRad(1, MathHelper.DegreesToRadians(-90 + cursorAngle) + directionModifier, color, type);

                if (Actions[VitaruAction.Slow])
                    directionModifier += 0.1d;
                else
                    directionModifier += 0.2d;
            }
        }
        #endregion

        #region Input Handling
        public override bool ReceiveMouseInputAt(Vector2 screenSpacePos) => true;

        public bool OnPressed(VitaruAction action)
        {
            if (true)//!Bot && !Puppet)
                return Pressed(action);
            //else
            //return false;
        }

        protected virtual bool Pressed(VitaruAction action)
        {
            //Keyboard Stuff
            if (action == VitaruAction.Up)
                Actions[VitaruAction.Up] = true;
            if (action == VitaruAction.Down)
                Actions[VitaruAction.Down] = true;
            if (action == VitaruAction.Left)
                Actions[VitaruAction.Left] = true;
            if (action == VitaruAction.Right)
                Actions[VitaruAction.Right] = true;
            if (action == VitaruAction.Slow)
            {
                Actions[VitaruAction.Slow] = true;
                VisibleHitbox.Alpha = 1;
            }

            //Mouse Stuff
            if (action == VitaruAction.Shoot)
                Actions[VitaruAction.Shoot] = true;

            //if (!Puppet)
            //sendPacket(action);

            return true;
        }

        public bool OnReleased(VitaruAction action)
        {
            if (true)//!Bot && !Puppet)
                return Released(action);
            else
                return false;
        }

        protected virtual bool Released(VitaruAction action)
        {
            //Keyboard Stuff
            if (action == VitaruAction.Up)
                Actions[VitaruAction.Up] = false;
            if (action == VitaruAction.Down)
                Actions[VitaruAction.Down] = false;
            if (action == VitaruAction.Left)
                Actions[VitaruAction.Left] = false;
            if (action == VitaruAction.Right)
                Actions[VitaruAction.Right] = false;
            if (action == VitaruAction.Slow)
            {
                Actions[VitaruAction.Slow] = false;
                VisibleHitbox.Alpha = 0;
            }
            //Mouse Stuff
            if (action == VitaruAction.Shoot)
                Actions[VitaruAction.Shoot] = false;

            //if (!Puppet)
            //sendPacket(VitaruAction.None, action);

            return true;
        }
        #endregion
    }
}