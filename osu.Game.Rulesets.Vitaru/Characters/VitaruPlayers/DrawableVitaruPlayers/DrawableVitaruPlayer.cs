using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Input.Bindings;
using osu.Framework.Platform;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Vitaru.Debug;
using osu.Game.Rulesets.Vitaru.Multi;
using osu.Game.Rulesets.Vitaru.Objects;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Settings;
using osu.Game.Rulesets.Vitaru.UI;
using Symcol.Core.Networking;
using System;
using System.Collections.Generic;
using static osu.Game.Rulesets.Vitaru.UI.Cursor.GameplayCursor;

namespace osu.Game.Rulesets.Vitaru.Characters.VitaruPlayers.DrawableVitaruPlayers
{
    public class DrawableVitaruPlayer : Character, IKeyBindingHandler<VitaruAction>
    {
        #region Fields
        protected readonly Gamemodes Gamemode = VitaruSettings.VitaruConfigManager.GetBindable<Gamemodes>(VitaruSetting.GameMode);

        protected readonly GraphicsOptions PlayerVisuals = VitaruSettings.VitaruConfigManager.GetBindable<GraphicsOptions>(VitaruSetting.PlayerVisuals);

        public readonly VitaruPlayer Player;

        protected override string CharacterName => Player.FileName;

        public override double MaxHealth => Player.MaxHealth;

        public override Color4 PrimaryColor => Player.PrimaryColor;

        public override Color4 SecondaryColor => Player.SecondaryColor;

        public override Color4 ComplementaryColor => Player.ComplementaryColor;

        public int ScoreZone = 100;

        public double SpeedMultiplier = 1;

        public Dictionary<VitaruAction, bool> Actions = new Dictionary<VitaruAction, bool>();

        //(MinX,MaxX,MinY,MaxY)
        protected Vector4 PlayerBounds
        {
            get
            {
                if (Gamemode == Gamemodes.Touhosu)
                    return new Vector4(0, 512, 0, 820);
                else
                    return new Vector4(0, 512, 0, 820);
            }
        }

        protected readonly Container Cursor;

        protected readonly VitaruNetworkingClientHandler VitaruNetworkingClientHandler;

        /// <summary>
        /// Are we a slave over the net?
        /// </summary>
        public bool Puppet;

        protected bool HealthHacks { get; private set; }

        public string PlayerID;

        private double lastQuarterBeat = -1;
        private double nextHalfBeat = -1;
        private double nextQuarterBeat = -1;
        private double beatLength = 1000;

        protected readonly List<HealingBullet> HealingBullets = new List<HealingBullet>();

        protected const double Healing_FallOff = 0.85d;

        private const double healing_range = 64;
        private const double healing_min = 0.5d;
        private const double healing_max = 2d;
        #endregion

        public DrawableVitaruPlayer(VitaruPlayfield playfield, VitaruPlayer player, VitaruNetworkingClientHandler vitaruNetworkingClientHandler) : base(playfield)
        {
            Player = player;
            VitaruNetworkingClientHandler = vitaruNetworkingClientHandler;

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

        [BackgroundDependencyLoader]
        private void load()
        {
            if (!Puppet)
                DebugToolkit.DebugItems.Add(new DebugAction(() => { HealthHacks = !HealthHacks; }) { Text = "Health Hacks" });
        }

        protected override void LoadAnimationSprites(TextureStore textures, Storage storage)
        {
            base.LoadAnimationSprites(textures, storage);

            RightSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "Right", storage);
            KiaiRightSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "KiaiRight", storage);
        }

        #region Beat Handling
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

            if (effectPoint.KiaiMode && Gamemode != Gamemodes.Touhosu)
            {
                Seal.Sign.FadeTo(0.25f * amplitudeAdjust, beat_in_time, Easing.Out);
                using (Seal.Sign.BeginDelayedSequence(beat_in_time))
                    Seal.Sign.FadeOut(beatLength);
            }

            if (effectPoint.KiaiMode && SoulContainer.Alpha == 1 && PlayerVisuals != GraphicsOptions.StandardV2)
            {
                if (!Dead && Gamemode != Gamemodes.Gravaru)
                {
                    KiaiContainer.FadeInFromZero(timingPoint.BeatLength / 4);
                    SoulContainer.FadeOutFromOne(timingPoint.BeatLength / 4);
                }

                if (Gamemode != Gamemodes.Touhosu)
                    Seal.Sign.FadeTo(0.15f, timingPoint.BeatLength / 4);
            }
            if (!effectPoint.KiaiMode && KiaiContainer.Alpha == 1 && PlayerVisuals != GraphicsOptions.StandardV2)
            {
                if (!Dead && Gamemode != Gamemodes.Gravaru)
                {
                    SoulContainer.FadeInFromZero(timingPoint.BeatLength);
                    KiaiContainer.FadeOutFromOne(timingPoint.BeatLength);
                }

                if (Gamemode != Gamemodes.Touhosu)
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

            if (HealingBullets.Count > 0)
            {
                double fallOff = 1;

                for (int i = 0; i < HealingBullets.Count - 1; i++)
                    fallOff *= Healing_FallOff;

                restart:
                foreach (HealingBullet HealingBullet in HealingBullets)
                {
                    Heal(GetBulletHealingMultiplier(HealingBullet.EdgeDistance) * fallOff);
                    HealingBullets.Remove(HealingBullet);
                    goto restart;
                }

                if (Gamemode != Gamemodes.Touhosu)
                {
                    Seal.Sign.Alpha = 0.2f;
                    Seal.Sign.FadeOut(beatLength / 4);
                }
            }
        }
        #endregion

        protected override void Update()
        {
            base.Update();

            if (HealthHacks)
                Heal(999999);

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

            if (!Puppet)
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
            {
                bool add = true;
                foreach (HealingBullet HealingBullet in HealingBullets)
                    if (HealingBullet.DrawableBullet == bullet)
                    {
                        HealingBullet.EdgeDistance = edgeDistance;
                        add = false;
                    }

                if (add)
                    HealingBullets.Add(new HealingBullet(bullet, edgeDistance));
            }

            if (Gamemode == Gamemodes.Dodge)
                edgeDistance *= 1.5f;

            if (edgeDistance <= 64 && bullet.ScoreZone < 300)
                bullet.ScoreZone = 300;
            else if (edgeDistance <= 128 && bullet.ScoreZone < 100)
                bullet.ScoreZone = 100;
        }

        protected class HealingBullet
        {
            public readonly DrawableBullet DrawableBullet;

            public double EdgeDistance { get; set; }

            public HealingBullet(DrawableBullet bullet, double distance)
            {
                DrawableBullet = bullet;
                EdgeDistance = distance;
            }
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

        protected double GetBulletHealingMultiplier(double value)
        {
            double scale = (healing_max - healing_min) / (0 - healing_range);
            return healing_min + ((value - healing_range) * scale);
        }

        protected override void Death()
        {
            //base.Death();
        }

        #region Shooting Handling
        private void bulletAddRad(double speed, double angle, Color4 color, double size, double damage)
        {
            DrawableBullet drawableBullet;

            VitaruPlayfield.GameField.Add(drawableBullet = new DrawableBullet(new Bullet
            {
                StartTime = Time.Current,
                Position = Position,
                BulletAngle = angle,
                BulletSpeed = speed,
                BulletDiameter = size,
                BulletDamage = damage,
                ColorOverride = color,
                Team = Team,
                DummyMode = true,
                SliderType = SliderType.Variable,
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
            double size = 16;
            double damage = 18;

            double cursorAngle = 0;

            if (Actions[VitaruAction.Slow])
            {
                cursorAngle = (MathHelper.RadiansToDegrees(Math.Atan2((Cursor.Position.Y - Position.Y), (Cursor.Position.X - Position.X))) + 90 + Rotation) - 12;
                directionModifier = 0.1d;
            }

            for (int i = 1; i <= numberbullets; i++)
            {
                if (i % 2 == 0)
                {
                    size = 20;
                    damage = 24;
                    color = PrimaryColor;
                }
                else
                {
                    size = 12;
                    damage = 18;
                    color = SecondaryColor;
                }

                //-90 = up
                bulletAddRad(1, MathHelper.DegreesToRadians(-90 + cursorAngle) + directionModifier, color, size, damage);

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

        #region Networking Handling
        private void sendPacket(VitaruAction pressedAction = VitaruAction.None, VitaruAction releasedAction = VitaruAction.None)
        {
            if (VitaruNetworkingClientHandler != null && !Puppet)
            {
                VitaruPlayerInformation playerInformation = new VitaruPlayerInformation
                {
                    Character = Player.FileName,
                    CursorX = Cursor.Position.X,
                    CursorY = Cursor.Position.Y,
                    PlayerX = Position.X,
                    PlayerY = Position.Y,
                    PlayerID = PlayerID,
                    PressedAction = pressedAction,
                    ReleasedAction = releasedAction,
                };

                ClientInfo clientInfo = new ClientInfo
                {
                    IP = VitaruNetworkingClientHandler.ClientInfo.IP,
                    Port = VitaruNetworkingClientHandler.ClientInfo.Port
                };

                VitaruInMatchPacket packet = new VitaruInMatchPacket(clientInfo) { PlayerInformation = playerInformation };

                VitaruNetworkingClientHandler.SendToHost(packet);
                VitaruNetworkingClientHandler.SendToInGameClients(packet);
            }
        }

        private void packetReceived(Packet p)
        {
            if (p is VitaruInMatchPacket packet && Puppet)
            {
                VitaruNetworkingClientHandler.ShareWithOtherPeers(packet);

                if (packet.PlayerInformation.PlayerID == PlayerID)
                {
                    Position = new Vector2(packet.PlayerInformation.PlayerX, packet.PlayerInformation.PlayerY);
                    Cursor.Position = new Vector2(packet.PlayerInformation.CursorX, packet.PlayerInformation.CursorY);

                    if (packet.PlayerInformation.PressedAction != VitaruAction.None)
                        Pressed(packet.PlayerInformation.PressedAction);
                    if (packet.PlayerInformation.ReleasedAction != VitaruAction.None)
                        Released(packet.PlayerInformation.ReleasedAction);
                }
            }
        }
        #endregion

        public static DrawableVitaruPlayer GetDrawableVitaruPlayer(VitaruPlayfield playfield, string name, VitaruNetworkingClientHandler vitaruNetworkingClientHandler)
        {
            switch (name)
            {
                default:
                    return new DrawableVitaruPlayer(playfield, VitaruPlayer.GetVitaruPlayer(name), vitaruNetworkingClientHandler);
                case "Alex":
                    return new DrawableVitaruPlayer(playfield, VitaruPlayer.GetVitaruPlayer(name), vitaruNetworkingClientHandler);
            }
        }
    }
}

