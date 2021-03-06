﻿#region usings

using System;
using System.Collections.Generic;
using osu.Framework.Allocation;
using osu.Framework.Audio.Track;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Textures;
using osu.Framework.Platform;
using osu.Game.Beatmaps.ControlPoints;
using osu.Game.Rulesets.Vitaru.ChapterSets;
using osu.Game.Rulesets.Vitaru.ChapterSets.Dodge;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Playfields;
using osu.Game.Rulesets.Vitaru.Ruleset.Debug;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects;
using osu.Game.Rulesets.Vitaru.Ruleset.HitObjects.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Input;
using osu.Game.Rulesets.Vitaru.Ruleset.Settings;
using osu.Mods.Online.Base;
using osu.Mods.Online.Base.Packets;
using osuTK;
using osuTK.Graphics;
using Sym.Base.Extentions;
using Sym.Networking.NetworkingHandlers.Peer;
using Sym.Networking.Packets;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers
{
    public class DrawableVitaruPlayer : DrawableCharacter
    {
        #region Fields
        protected readonly ChapterSet ChapterSet = ChapterStore.GetChapterSet(VitaruSettings.VitaruConfigManager.Get<string>(VitaruSetting.Gamemode));

        protected readonly GraphicsOptions PlayerVisuals = VitaruSettings.VitaruConfigManager.Get<GraphicsOptions>(VitaruSetting.PlayerVisuals);

        private readonly DebugConfiguration configuration = VitaruSettings.VitaruConfigManager.Get<DebugConfiguration>(VitaruSetting.DebugConfiguration);

        private readonly AutoType auto = VitaruSettings.VitaruConfigManager.Get<AutoType>(VitaruSetting.AutoType);

        private readonly Bindable<double> directionBindable = new Bindable<double>();

        protected internal VitaruInputContainer VitaruInputContainer { get; private set; }

        public readonly VitaruPlayer Player;

        protected override string CharacterName => Player.Name;

        public override double MaxHealth => Player.MaxHealth;

        public override Color4 PrimaryColor => Player.PrimaryColor;

        public override Color4 SecondaryColor => Player.SecondaryColor;

        public override Color4 ComplementaryColor => Player.TrinaryColor;

        public double SpeedMultiplier = 1;

        protected Dictionary<VitaruAction, bool> Actions = new Dictionary<VitaruAction, bool>();

        protected readonly Container Cursor;

        /// <summary>
        /// How will we be controled?
        /// </summary>
        public ControlType ControlType { get; set; }

        protected OsuUser User { get; private set; }

        protected OsuNetworkingHandler OsuNetworkingHandler { get; private set; }

        protected bool HealthHacks { get; private set; }

        //Is reset after healing applied
        public double HealingMultiplier = 1;

        private double nextShoot = -1;

        private double lastQuarterBeat = -1;
        private double nextHalfBeat = -1;
        private double nextQuarterBeat = -1;
        protected double BeatLength = 1000;

        protected List<HealingProjectile> HealingProjectiles { get; private set; } = new List<HealingProjectile>();

        protected const double HEALING_FALL_OFF = 0.85d;

        private const double field_of_view = 360;

        private const double healing_range = 64;
        private const double healing_min = 0.5d;
        private const double healing_max = 2d;
        #endregion

        public DrawableVitaruPlayer(VitaruPlayfield playfield, VitaruPlayer player) : base(playfield)
        {
            Player = player;

            Add(VitaruInputContainer = new VitaruInputContainer());

            VitaruInputContainer.Pressed += action =>
            {
                if (ControlType >= ControlType.Net)
                    return false;
                return Pressed(action);
            };
            VitaruInputContainer.Released += action =>
            {
                if (ControlType >= ControlType.Net)
                    return false;
                return Released(action);
            };

            foreach (VitaruAction action in (VitaruAction[])Enum.GetValues(typeof(VitaruAction)))
                Actions[action] = false;

            VitaruPlayfield.Gamefield.Add(Cursor = new Container
            {
                Anchor = Anchor.TopLeft,
                Origin = Anchor.Centre,
                Size = new Vector2(4),
                CornerRadius = 2,
                Alpha = 0f,
                Masking = true,
                AlwaysPresent = true,

                Child = new Box
                {
                    RelativeSizeAxes = Axes.Both
                }
            });
        }

        protected internal virtual void SetNetworking(OsuNetworkingHandler osuNetworkingHandler, OsuUser user)
        {
            OsuNetworkingHandler = osuNetworkingHandler;
            User = user;

            OsuNetworkingHandler.OnPacketReceive += OnPacketReceive;
        }

        protected virtual void OnPacketReceive(PacketInfo<Host> info)
        {
            switch (info.Packet)
            {
                case Vector2Packet vec2:
                    if (vec2.ID == User.ID && ControlType == ControlType.Net)
                    {
                        switch (vec2.Name)
                        {
                            case "ppos":
                                Position = vec2.Vector2;
                                break;
                            case "cpos":
                                Cursor.Position = vec2.Vector2;
                                break;
                        }
                    }
                    break;
                case ValueArrayPacket<VitaruAction, bool> act:
                    if (act.ID == User.ID && ControlType == ControlType.Net)
                    {
                        switch (act.Name)
                        {
                            case "act":
                                if (Actions[act.Value])
                                    Released(act.Value);
                                else
                                    Pressed(act.Value);

                                //TODO: so much could be done here to improve this...
                                foreach (VitaruAction action in (VitaruAction[])Enum.GetValues(typeof(VitaruAction)))
                                    Actions[action] = act.Array[(int)action];

                                break;
                        }
                    }
                    break;
                case ValuePacket<double> value:
                    if (value.ID == User.ID && ControlType == ControlType.Net)
                        switch (value.Name)
                        {
                            case "hp":
                                Health = value.Value;
                                break;
                        }
                    break;
            }
        }

        private double last_fast_update = double.MinValue;
        private const double fast_update_rate = 60;

        private double last_slow_update = double.MinValue;
        private const double slow_update_rate = 5;

        protected virtual void UpdateOnline()
        {
            if (Time.Current < last_fast_update + 1000 / fast_update_rate) return;
            last_fast_update = Time.Current;

            OnFastUpdate();

            if (Time.Current < last_slow_update + 1000 / slow_update_rate) return;
            last_slow_update = Time.Current;

            OnSlowUpdate();
        }

        protected virtual void OnFastUpdate()
        {
            OsuNetworkingHandler.SendToServer(new Vector2Packet
            {
                Vector2 = Position,
                ID = OsuNetworkingHandler.OsuUser.ID,
                Name = "ppos"
            });
            OsuNetworkingHandler.SendToServer(new Vector2Packet
            {
                Vector2 = Cursor.Position,
                ID = OsuNetworkingHandler.OsuUser.ID,
                Name = "cpos"
            });
        }

        protected virtual void OnSlowUpdate()
        {
            OsuNetworkingHandler.SendToServer(new ValuePacket<double>
            {
                Value = Health,
                ID = OsuNetworkingHandler.OsuUser.ID,
                Name = "hp"
            });
        }

        protected virtual void SendActionPacket(VitaruAction action)
        {
            if (ControlType == ControlType.Net || OsuNetworkingHandler == null) return;

            bool[] array = new bool[(int)VitaruAction.Pull + 1];

            foreach (KeyValuePair<VitaruAction, bool> pair in Actions)
                array[(int)pair.Key] = pair.Value;

            OsuNetworkingHandler.SendToServer(new ValueArrayPacket<VitaruAction, bool>
            {
                Value = action,
                Array = array,
                ID = OsuNetworkingHandler.OsuUser.ID,
                Name = "act",
            });
        }

        [BackgroundDependencyLoader]
        private void load()
        {
            if (ControlType == ControlType.Player)
            {
                HITDETECTION = true;
                switch (configuration)
                {
                    case DebugConfiguration.General:
                        DebugToolkit.GeneralDebugItems.Add(new DebugAction(() =>
                        {
                            HITDETECTION = !HITDETECTION;
                            VitaruPlayfield.Cheated = true;
                        }) { Text = "Toggle BulletParsing" });
                        DebugToolkit.GeneralDebugItems.Add(new DebugAction(() =>
                        {
                            HealthHacks = !HealthHacks;
                            VitaruPlayfield.Cheated = true;
                        }) { Text = "Health Hacks" });
                        break;
                    case DebugConfiguration.AI:
                        VitaruPlayfield.Cheated = true;
                        DebugToolkit.AIDebugItems.Add(new DebugAction(() => { HealthHacks = !HealthHacks; }) { Text = "Health Hacks" });
                        DebugToolkit.AIDebugItems.Add(new DebugStat<double>(directionBindable) { Text = "Direction" });
                        break;
                }
            }
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
            float amplitudeAdjust = Math.Min(1, 0.4f + amplitudes.Maximum);

            BeatLength = timingPoint.BeatLength;

            OnHalfBeat();
            lastQuarterBeat = VitaruPlayfield.Current;
            nextHalfBeat = VitaruPlayfield.Current + timingPoint.BeatLength / 2;
            nextQuarterBeat = VitaruPlayfield.Current + timingPoint.BeatLength / 4;

            const double beat_in_time = 60;

            if (effectPoint.KiaiMode && !(ChapterSet is TouhosuChapterSet))
            {
                Seal.Sign.FadeTo(0.25f * amplitudeAdjust, beat_in_time, Easing.Out);
                using (Seal.Sign.BeginDelayedSequence(beat_in_time))
                    Seal.Sign.FadeOut(BeatLength);
            }

            if (effectPoint.KiaiMode && SoulContainer.Alpha == 1 && PlayerVisuals == GraphicsOptions.Old)
            {
                if (!Dead)
                {
                    KiaiContainer.FadeInFromZero(timingPoint.BeatLength / 4);
                    SoulContainer.FadeOutFromOne(timingPoint.BeatLength / 4);
                }

                if (!(ChapterSet is TouhosuChapterSet))
                    Seal.Sign.FadeTo(0.15f, timingPoint.BeatLength / 4);
            }
            if (!effectPoint.KiaiMode && KiaiContainer.Alpha == 1 && PlayerVisuals == GraphicsOptions.Old)
            {
                if (!Dead)
                {
                    SoulContainer.FadeInFromZero(timingPoint.BeatLength);
                    KiaiContainer.FadeOutFromOne(timingPoint.BeatLength);
                }

                if (!(ChapterSet is TouhosuChapterSet))
                    Seal.Sign.FadeTo(0f, timingPoint.BeatLength);
            }
        }

        protected virtual void OnHalfBeat()
        {
            nextHalfBeat = -1;
        }

        protected virtual void OnQuarterBeat()
        {
            lastQuarterBeat = nextQuarterBeat;
            nextQuarterBeat += BeatLength / 4;

            if (HealingProjectiles.Count > 0)
            {
                double fallOff = 1;

                for (int i = 0; i < HealingProjectiles.Count - 1; i++)
                    fallOff *= HEALING_FALL_OFF;

                foreach (HealingProjectile healingBullet in HealingProjectiles)
                {
                    Heal(GetBulletHealingMultiplier(healingBullet.EdgeDistance) * fallOff * HealingMultiplier);
                }
                HealingProjectiles = new List<HealingProjectile>();
                HealingMultiplier = 1;

                if (!(ChapterSet is TouhosuChapterSet))
                {
                    Seal.Sign.Alpha = 0.2f;
                    Seal.Sign.FadeOut(BeatLength / 4);
                }
            }
        }
        #endregion

        protected override void Update()
        {
            base.Update();

            if (HealthHacks)
                Heal(999999);

            Position = GetNewPlayerPosition(0.4d);

            if (ControlType >= ControlType.Puppet)
            {
                DrawableCharacter closestCharacter = null;
                double closestCharaterDistance = double.MaxValue;

                foreach (Drawable draw in CurrentPlayfield)
                    if (draw is DrawableCharacter character)
                    {
                        if (character.Team != Team)
                        {
                            double distance = Math.Sqrt(Math.Pow(character.X, 2) + Math.Pow(character.Y, 2));

                            if (distance < 0)
                                distance *= -1;

                            if (distance < closestCharaterDistance)
                            {
                                closestCharacter = character;
                                closestCharaterDistance = distance;
                            }
                        }
                    }

                if (closestCharacter != null)
                    Cursor.Position = closestCharacter.Position;
            }
            else if (ControlType == ControlType.Player && Parent != null)
                Cursor.Position = VitaruPlayfield.MousePos + new Vector2(6);

            if (nextHalfBeat <= VitaruPlayfield.Current && nextHalfBeat != -1)
                OnHalfBeat();

            if (nextQuarterBeat <= VitaruPlayfield.Current && nextQuarterBeat != -1)
                OnQuarterBeat();

            if ((VitaruPlayfield.Current >= nextShoot || VitaruPlayfield.Current <= nextShoot - BeatLength) && Actions[VitaruAction.Shoot])
                PatternWave();

            if (ControlType == ControlType.Player && OsuNetworkingHandler != null)
                UpdateOnline();
        }

        protected override void ParseProjectile(DrawableProjectile projectile)
        {
            base.ParseProjectile(projectile);

            Vector2 relativePos = projectile.Hitbox.ToSpaceOfOtherDrawable(Vector2.Zero, Hitbox);
            relativePos += new Vector2(projectile.Hitbox.Width / 8 + Hitbox.Width / 8);

            double distance = Math.Sqrt(Math.Pow(relativePos.X, 2) + Math.Pow(relativePos.Y, 2));
            double edgeDistance = distance - (projectile.Hitbox.Width / 2 + Hitbox.Width / 2);

            if (edgeDistance < 64 && projectile.HitObject.Team != Team)
            {
                bool add = true;
                foreach (HealingProjectile healingProjectile in HealingProjectiles)
                    if (healingProjectile.DrawableProjectile == projectile)
                    {
                        healingProjectile.EdgeDistance = edgeDistance;
                        add = false;
                    }

                if (add)
                    HealingProjectiles.Add(new HealingProjectile(projectile, edgeDistance));
            }

            if (ChapterSet is DodgeChapterSet)
                edgeDistance *= 1.5f;

            if (edgeDistance < projectile.MinDistance)
                projectile.MinDistance = edgeDistance;
        }

        protected class HealingProjectile
        {
            public readonly DrawableProjectile DrawableProjectile;

            public double EdgeDistance { get; set; }

            public HealingProjectile(DrawableProjectile projectile, double distance)
            {
                DrawableProjectile = projectile;
                EdgeDistance = distance;
            }
        }

        protected virtual Vector2 GetNewPlayerPosition(double playerSpeed)
        {
            Vector2 playerPosition = Position;

            double yTranslationDistance = playerSpeed * Clock.ElapsedFrameTime * SpeedMultiplier;
            double xTranslationDistance = playerSpeed * Clock.ElapsedFrameTime * SpeedMultiplier;

            if (ControlType >= ControlType.Puppet)
            {
                Actions[VitaruAction.Up] = false;
                Actions[VitaruAction.Down] = false;
                Actions[VitaruAction.Left] = false;
                Actions[VitaruAction.Right] = false;
                Actions[VitaruAction.Slow] = false;
                Actions[VitaruAction.Shoot] = false;

                Auto();

                Actions[VitaruAction.Shoot] = true;

                VisibleHitbox.Alpha = Actions[VitaruAction.Slow] ? 1 : 0;
            }

            if (Actions[VitaruAction.Slow])
            {
                xTranslationDistance /= 2d;
                yTranslationDistance /= 2d;
            }

            if (Actions[VitaruAction.Up])
                playerPosition.Y -= (float)yTranslationDistance;
            if (Actions[VitaruAction.Left])
                playerPosition.X -= (float)xTranslationDistance;
            if (Actions[VitaruAction.Down])
                playerPosition.Y += (float)yTranslationDistance;
            if (Actions[VitaruAction.Right])
                playerPosition.X += (float)xTranslationDistance;

            if (!VitaruPlayfield.BOUNDLESS)
            {
                playerPosition = Vector2.ComponentMin(playerPosition, ChapterSet.PlayfieldBounds.Zw);
                playerPosition = Vector2.ComponentMax(playerPosition, ChapterSet.PlayfieldBounds.Xy);
            }

            return playerPosition;
        }

        protected virtual void Auto()
        {
            if (auto == AutoType.Oldest)
            {
                DrawableBullet closestBullet = null;
                float closestBulletEdgeDitance = float.MaxValue;
                float closestBulletAngle = 0;

                DrawableBullet secondClosestBullet = null;
                float secondClosestBulletEdgeDitance = float.MaxValue;
                float secondClosestBulletAngle = 0;

                //bool bulletBehind = false;
                float behindBulletEdgeDitance = float.MaxValue;
                float behindBulletAngle = 0;

                foreach (Drawable draw in CurrentPlayfield)
                    if (draw is DrawableBullet)
                    {
                        DrawableBullet bullet = draw as DrawableBullet;
                        if (bullet.HitObject.Team != Team && bullet.Hitbox != null)
                        {
                            Vector2 relativePos = bullet.Hitbox.ToSpaceOfOtherDrawable(Vector2.Zero, Hitbox);
                            relativePos += new Vector2(bullet.Hitbox.Width / 8 + Hitbox.Width / 8);
                            double distance = Math.Sqrt(Math.Pow(relativePos.X, 2) + Math.Pow(relativePos.Y, 2));
                            double edgeDistance = distance - (bullet.Hitbox.Width / 2 + Hitbox.Width / 2);
                            double angleToBullet = MathHelper.RadiansToDegrees(Math.Atan2(bullet.Position.Y - Position.Y, bullet.Position.X - Position.X)) + 90 + Rotation;

                            if (closestBulletAngle < 360 - field_of_view | closestBulletAngle < -field_of_view && closestBulletAngle > field_of_view | closestBulletAngle > 360 + field_of_view)
                                if (closestBullet.Position.X > Position.X && bullet.Position.X < Position.X || closestBullet.Position.X < Position.X && bullet.Position.X > Position.X)
                                {
                                    //bulletBehind = true;
                                    behindBulletEdgeDitance = (float)edgeDistance;
                                    behindBulletAngle = (float)angleToBullet;
                                }

                            if (edgeDistance < closestBulletEdgeDitance)
                            {
                                secondClosestBullet = closestBullet;
                                secondClosestBulletEdgeDitance = closestBulletEdgeDitance;
                                secondClosestBulletAngle = closestBulletAngle;

                                closestBullet = bullet;
                                closestBulletEdgeDitance = (float)edgeDistance;
                                closestBulletAngle = (float)angleToBullet;
                            }
                        }
                    }

                if (closestBulletEdgeDitance <= 8)
                {
                    if (closestBulletEdgeDitance <= 8 && closestBulletEdgeDitance >= 4)
                    {
                        Actions[VitaruAction.Down] = true;
                        Actions[VitaruAction.Slow] = true;
                    }

                    if ((closestBulletAngle > 360 - field_of_view || closestBulletAngle > -field_of_view) && (closestBulletAngle < field_of_view || closestBulletAngle < 360 + field_of_view)
                                                                                                          && secondClosestBulletEdgeDitance - closestBulletEdgeDitance >= 1)
                    {
                        if (closestBullet != null && closestBullet.X < Position.X)
                            Actions[VitaruAction.Right] = true;
                        else
                            Actions[VitaruAction.Left] = true;
                    }
                }
                else
                {
                    if (Position.X > VitaruPlayfield.DrawWidth - 250)
                        Actions[VitaruAction.Left] = true;
                    else if (Position.X < 250)
                        Actions[VitaruAction.Right] = true;

                    if (Position.Y < 640)
                        Actions[VitaruAction.Down] = true;
                    else if (Position.Y > 680)
                        Actions[VitaruAction.Up] = true;
                }
            }
            else if (auto == AutoType.Old)
            {
                List<IndexedBullet> bullets = new List<IndexedBullet>();
                foreach (Drawable draw in CurrentPlayfield)
                    if (draw is DrawableBullet bullet && bullet.HitObject.Team != Team)
                        bullets.Add(new IndexedBullet(bullet, this));

                restart:
                foreach (IndexedBullet bullet in bullets)
                {
                    //Ignore bullets that wont be near us for a bit
                    if (bullet.EdgeDistance / bullet.DrawableBullet.HitObject.Speed > 200)
                    {
                        bullets.Remove(bullet);
                        goto restart;
                    }

                    //TODO: Ignore bullets that aren't aimmed at us
                    if (false)//bullet.DrawableBullet.Bullet.Angle < (bullet.AngleRadian + Math.PI) - 10 || bullet.DrawableBullet.Bullet.Angle > (bullet.AngleRadian + Math.PI) + 10)
                    {
#pragma warning disable CS0162 // Unreachable code detected
                        bullets.Remove(bullet);
#pragma warning restore CS0162 // Unreachable code detected
                        goto restart;
                    }
                }

                const double fov = Math.PI / 2;

                double direction = 8;
                double leastWeight = double.MaxValue;
                bool bulletNearby = false;

                for (int i = 0; i < 8; i++)
                {
                    List<IndexedBullet> visibleBullets = new List<IndexedBullet>();

                    double a = i * (Math.PI / 4);

                    //Get bullets that we see looking this direction
                    foreach (IndexedBullet bullet in bullets)
                        if (bullet.AngleRadian >= a - fov / 2 && bullet.AngleRadian <= a + fov / 2)
                            visibleBullets.Add(bullet);

                    if (visibleBullets.Count > 0)
                        bulletNearby = true;

                    double weight = 0;

                    foreach (IndexedBullet bullet in visibleBullets)
                        weight = (100 - bullet.EdgeDistance + weight) / 2;

                    if (weight < leastWeight)
                    {
                        direction = i;
                        leastWeight = weight;
                    }
                }

                if (bulletNearby)
                {
                    if (direction == 7 || direction == 8 || direction == 0 || direction == 1)
                        Actions[VitaruAction.Up] = true;
                    if (direction == 1 || direction == 2 || direction == 3)
                        Actions[VitaruAction.Right] = true;
                    if (direction == 3 || direction == 4 || direction == 5)
                        Actions[VitaruAction.Down] = true;
                    if (direction == 5 || direction == 6 || direction == 7)
                        Actions[VitaruAction.Left] = true;

                    directionBindable.Value = direction;
                }
                else
                {
                    if (Position.X > VitaruPlayfield.DrawWidth - 250)
                        Actions[VitaruAction.Left] = true;
                    else if (Position.X < 250)
                        Actions[VitaruAction.Right] = true;

                    if (Position.Y < 640)
                        Actions[VitaruAction.Down] = true;
                    else if (Position.Y > 680)
                        Actions[VitaruAction.Up] = true;

                    Actions[VitaruAction.Slow] = true;
                }
            }
            else
            {

            }
        }

        protected virtual double GetBulletHealingMultiplier(double value) => SymcolMath.Scale(value, 0, healing_range, healing_min, healing_max);

        protected override void Death()
        {
            //base.Death();
        }

        #region Shooting Handling
        protected virtual DrawableBullet BulletAddRad(float speed, float angle, Color4 color, float size, float damage)
        {
            DrawableBullet b;
            VitaruPlayfield.Gamefield.Add(b = new DrawableBullet(new Bullet
            {
                PatternStartTime = VitaruPlayfield.Current,
                Position = Position,
                Angle = angle,
                Speed = speed,
                Diameter = size,
                Damage = damage,
                ColorOverride = color,
                Team = Team,
                Dummy = true,
                SliderType = SliderType.Straight,
                Distance = 1000,
                
            }, VitaruPlayfield){ Position = Position });
            b.Untuned = Untuned;
            return b;
        }

        protected virtual void PatternWave()
        {
            nextShoot = VitaruPlayfield.Current + BeatLength / 2f;
            const int numberbullets = 3;
            float directionModifier = -0.2f;

            float cursorAngle = 0;

            if (Actions[VitaruAction.Slow])
            {
                cursorAngle = MathHelper.RadiansToDegrees((float)Math.Atan2(Cursor.Position.Y - Position.Y, Cursor.Position.X - Position.X)) + 90 + Rotation;
                directionModifier = -0.1f;
            }

            for (int i = 1; i <= numberbullets; i++)
            {
                float size;
                float damage;
                Color4 color;

                if (i % 2 == 0)
                {
                    size = 28;
                    damage = 24;
                    color = PrimaryColor;
                }
                else
                {
                    size = 20;
                    damage = 18;
                    color = SecondaryColor;
                }

                //-90 = up
                BulletAddRad(1, MathHelper.DegreesToRadians(-90 + cursorAngle) + directionModifier, color, size, damage);

                if (Actions[VitaruAction.Slow])
                    directionModifier += 0.1f;
                else
                    directionModifier += 0.2f;
            }
        }
        #endregion

        #region Input Handling

        protected virtual bool Pressed(VitaruAction action)
        {
            if (action == VitaruAction.Slow)
                VisibleHitbox.FadeInFromZero(200);

            //Mouse Stuff
            if (action == VitaruAction.Shoot)
                PatternWave();

            Actions[action] = true;

            SendActionPacket(action);

            return true;
        }

        protected virtual bool Released(VitaruAction action)
        {
            if (action == VitaruAction.Slow)
                VisibleHitbox.FadeOutFromOne(200);

            Actions[action] = false;

            SendActionPacket(action);

            return true;
        }
        #endregion

        protected override void Dispose(bool isDisposing)
        {
            VitaruInputContainer = null;
            if (OsuNetworkingHandler != null) OsuNetworkingHandler.OnPacketReceive -= OnPacketReceive;
            OsuNetworkingHandler = null;
            base.Dispose(isDisposing);
        }

        //TODO: have the playfield do this once for everyone?
        protected class IndexedBullet
        {
            public readonly DrawableBullet DrawableBullet;

            public readonly DrawableVitaruPlayer Player;

            public readonly Vector2 RelativePosition;

            public readonly double Distance;

            public readonly double EdgeDistance;

            public readonly double AngleDegree;

            public readonly double AngleRadian;

            public IndexedBullet(DrawableBullet bullet, DrawableVitaruPlayer player)
            {
                DrawableBullet = bullet;
                Player = player;

                RelativePosition = bullet.Hitbox.ToSpaceOfOtherDrawable(Vector2.Zero, player.Hitbox);
                RelativePosition += new Vector2(bullet.Hitbox.Width / 8 + player.Hitbox.Width / 8);
                Distance = Math.Sqrt(Math.Pow(RelativePosition.X, 2) + Math.Pow(RelativePosition.Y, 2));
                EdgeDistance = Distance - (bullet.Hitbox.Width / 2 + player.Hitbox.Width / 2);
                AngleRadian = (float)Math.Atan2(bullet.Position.Y - player.Position.Y, bullet.Position.X - player.Position.X) + Math.PI / 2 + MathHelper.DegreesToRadians(player.Rotation);
                AngleDegree = MathHelper.RadiansToDegrees(AngleRadian);
            }
        }
    }

    public enum ControlType
    {
        Player,
        Net,
        Puppet,
        Auto,
    }

    public enum AutoType
    {
        Oldest,
        Old,
        New
    }
}

