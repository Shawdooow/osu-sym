﻿using OpenTK;
using osu.Framework.Graphics;
using OpenTK.Graphics;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.Textures;
using osu.Framework.Allocation;
using osu.Framework.Graphics.Containers;
using osu.Game.Graphics.Containers;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Extensions.Color4Extensions;
using Symcol.Core.GameObjects;
using osu.Framework.Platform;
using osu.Game.Rulesets.Vitaru.Objects.Drawables;
using osu.Game.Rulesets.Vitaru.Characters.Pieces;
using System;
using osu.Game.Rulesets.Vitaru.Characters.TouhosuPlayers.Abilities;

namespace osu.Game.Rulesets.Vitaru.Characters
{
    public abstract class Character : BeatSyncedContainer, ITuneable
    {
        #region Fields
        public override bool HandleMouseInput => false;

        protected static bool HitDetection;

        protected Seal Seal { get; private set; }

        protected Container KiaiContainer { get; set; }
        protected Sprite KiaiStillSprite { get; set; }
        protected Sprite KiaiRightSprite { get; set; }
        protected Sprite KiaiLeftSprite { get; set; }

        protected Container SoulContainer { get; set; }
        protected Sprite StillSprite { get; set; }
        protected Sprite RightSprite { get; set; }
        protected Sprite LeftSprite { get; set; }

        public abstract double MaxHealth { get; }

        public double Health { get; private set; }

        protected abstract string CharacterName { get; }

        public Container CurrentPlayfield { get; set; }

        public bool Untuned
        {
            get => untuned;
            set
            {
                if (value == untuned) return;

                untuned = value;

                if (value)
                {
                    VitaruPlayfield.Gamefield.Remove(this);
                    VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Add(this);
                    CurrentPlayfield = VitaruPlayfield.VitaruInputManager.BlurredPlayfield;
                }
                else
                {
                    VitaruPlayfield.VitaruInputManager.BlurredPlayfield.Remove(this);
                    VitaruPlayfield.Gamefield.Add(this);
                    CurrentPlayfield = VitaruPlayfield.Gamefield;
                }
            }

        }

        private bool untuned;

        public virtual Color4 PrimaryColor { get; } = Color4.Green;

        public virtual Color4 SecondaryColor { get; } = Color4.LightBlue;

        public virtual Color4 ComplementaryColor { get; } = Color4.LightGreen;

        protected virtual float HitboxWidth { get; } = 4;

        public bool Dead { get; protected set; }

        protected readonly VitaruPlayfield VitaruPlayfield;

        public Action<bool> OnDispose;

        public int Team { get; set; }
        protected CircularContainer VisibleHitbox;
        public SymcolHitbox Hitbox;
        protected float LastX;
        #endregion

        protected Character(VitaruPlayfield vitaruPlayfield)
        {
            VitaruPlayfield = vitaruPlayfield;
            CurrentPlayfield = VitaruPlayfield.Gamefield;
        }

        /// <summary>
        /// Does animations to better give the illusion of movement (could likely be cleaned up)
        /// </summary>
        protected virtual void MovementAnimations()
        {
            if (LeftSprite.Texture == null && RightSprite != null)
            {
                LeftSprite.Texture = RightSprite.Texture;
                LeftSprite.Size = new Vector2(-RightSprite.Size.X, RightSprite.Size.Y);
            }
            if (KiaiLeftSprite.Texture == null && KiaiRightSprite != null)
            {
                KiaiLeftSprite.Texture = KiaiRightSprite.Texture;
                KiaiLeftSprite.Size = new Vector2(-KiaiRightSprite.Size.X, KiaiRightSprite.Size.Y);
            }
            if (Position.X > LastX)
            {
                if (LeftSprite?.Texture != null)
                    LeftSprite.Alpha = 0;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 1;
                if (StillSprite?.Texture != null)
                    StillSprite.Alpha = 0;
                if (KiaiLeftSprite?.Texture != null)
                    KiaiLeftSprite.Alpha = 0;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 1;
                if (KiaiStillSprite?.Texture != null)
                    KiaiStillSprite.Alpha = 0;
            }
            else if (Position.X < LastX)
            {
                if (LeftSprite?.Texture != null)
                    LeftSprite.Alpha = 1;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 0;
                if (StillSprite?.Texture != null)
                    StillSprite.Alpha = 0;
                if (KiaiLeftSprite?.Texture != null)
                    KiaiLeftSprite.Alpha = 1;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 0;
                if (KiaiStillSprite?.Texture != null)
                    KiaiStillSprite.Alpha = 0;
            }
            else
            {
                if (LeftSprite?.Texture != null)
                    LeftSprite.Alpha = 0;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 0;
                if (StillSprite?.Texture != null)
                    StillSprite.Alpha = 1;
                if (KiaiLeftSprite?.Texture != null)
                    KiaiLeftSprite.Alpha = 0;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 0;
                if (KiaiStillSprite?.Texture != null)
                    KiaiStillSprite.Alpha = 1;
            }
            LastX = Position.X;
        }

        protected override void Update()
        {
            base.Update();

            if (Health <= 0 && !Dead)
                Death();

            if (HitDetection)
            {
                foreach (Drawable draw in CurrentPlayfield)
                {
                    DrawableBullet bullet = draw as DrawableBullet;
                    if (bullet?.Hitbox != null)
                    {
                        ParseBullet(bullet);
                        if (Hitbox.HitDetect(Hitbox, bullet.Hitbox))
                        {
                            Hurt(bullet.Bullet.BulletDamage);
                            bullet.Bullet.BulletDamage = 0;
                            bullet.Hit = true;
                        }
                    }

                    DrawableSeekingBullet seekingBullet = draw as DrawableSeekingBullet;
                    if (seekingBullet?.Hitbox != null)
                    {
                        if (Hitbox.HitDetect(Hitbox, seekingBullet.Hitbox))
                        {
                            Hurt(seekingBullet.SeekingBullet.BulletDamage);
                            seekingBullet.SeekingBullet.BulletDamage = 0;
                            seekingBullet.Hit = true;
                        }
                    }

                    DrawableLaser laser = draw as DrawableLaser;
                    if (laser?.Hitbox != null)
                    {
                        if (Hitbox.HitDetect(Hitbox, laser.Hitbox))
                        {
                            Hurt(laser.Laser.LaserDamage * (1000 / (float)Clock.ElapsedFrameTime));
                            laser.Hit = true;
                        }
                    }
                }
            }

            MovementAnimations();
        }

        /// <summary>
        /// Gets called just before hit detection
        /// </summary>
        protected virtual void ParseBullet(DrawableBullet bullet) { }

        protected virtual void LoadAnimationSprites(TextureStore textures, Storage storage)
        {
            StillSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName, storage);
            KiaiStillSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "Kiai", storage);
        }

        /// <summary>
        /// Child loading for all Characters (Enemies, Player, Bosses)
        /// </summary>
        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            Health = MaxHealth;

            Anchor = Anchor.TopLeft;
            Origin = Anchor.Centre;

            //TODO: Temp?
            Size = new Vector2(64);

            AddRange(new Drawable[]
            {
                Seal = new Seal(this),
                SoulContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = PrimaryColor,
                    Alpha = 1,
                    Children = new Drawable[]
                    {
                        StillSprite = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 1,
                        },
                        RightSprite = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                        LeftSprite = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                    }
                },
                KiaiContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0,
                    Children = new Drawable[]
                    {
                        KiaiStillSprite = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 1,
                        },
                        KiaiRightSprite = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                        KiaiLeftSprite = new Sprite
                        {
                            RelativeSizeAxes = Axes.Both,
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                    }
                },
                VisibleHitbox = new CircularContainer
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0,
                    Size = new Vector2(HitboxWidth + HitboxWidth / 4),
                    BorderColour = PrimaryColor,
                    BorderThickness = HitboxWidth / 4,
                    Masking = true,

                    Child = new Box
                    {
                        RelativeSizeAxes = Axes.Both
                    },
                    EdgeEffect = new EdgeEffectParameters
                    {

                        Radius = HitboxWidth / 2,
                        Type = EdgeEffectType.Shadow,
                        Colour = PrimaryColor.Opacity(0.5f)
                    }
                }
            });

            Add(Hitbox = new SymcolHitbox(new Vector2(HitboxWidth)) { Team = Team });

            if (CharacterName == "player" || CharacterName == "enemy")
                KiaiContainer.Colour = PrimaryColor;

            LoadAnimationSprites(textures, storage);
        }

        /// <summary>
        /// Removes "damage"
        /// </summary>
        /// <param name="damage"></param>
        public virtual double Hurt(double damage)
        {
            Health -= damage;

            if (Health < 0)
            {
                Health = 0;
                Death();
            }

            return Health;
        }

        /// <summary>
        /// Adds "health"
        /// </summary>
        /// <param name="health"></param>
        public virtual double Heal(double health)
        {
            if (Health <= 0 && health > 0)
                Revive();

            Health += health;

            if (Health > MaxHealth)
                Health = MaxHealth;

            return Health;
        }

        protected virtual void Death()
        {
            Dead = true;
            Delete();
        }

        public virtual void Delete()
        {
            if (Parent is Container p)
                p.Remove(this);

            Dispose();
        }

        protected override void Dispose(bool isDisposing)
        {
            OnDispose?.Invoke(isDisposing);
            base.Dispose(isDisposing);
        }

        protected virtual void Revive()
        {
            Dead = false;
        }
    }
}
