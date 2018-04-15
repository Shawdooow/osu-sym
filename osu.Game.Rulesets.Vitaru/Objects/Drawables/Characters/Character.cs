using OpenTK;
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

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters
{
    public abstract class Character : BeatSyncedContainer
    {
        #region Fields
        protected Sprite Sign { get; private set; }

        protected Container KiaiContainer { get; private set; }
        protected Sprite KiaiStillSprite { get; private set; }
        protected Sprite KiaiRightSprite { get; private set; }
        protected Sprite KiaiLeftSprite { get; private set; }

        protected Container SoulContainer { get; private set; }
        protected Sprite StillSprite { get; private set; }
        protected Sprite RightSprite { get; private set; }
        protected Sprite LeftSprite { get; private set; }

        public abstract double MaxHealth { get; }

        public double Health { get; private set; }

        protected abstract string CharacterName { get; }

        protected abstract Color4 CharacterColor { get; }

        public virtual float HitboxWidth { get; } = 4;

        public bool Dead { get; protected set; }

        protected readonly VitaruPlayfield VitaruPlayfield;

        public int Team { get; set; }
        protected CircularContainer VisibleHitbox;
        public SymcolHitbox Hitbox;
        public bool CanHeal = false;
        protected float LastX;
        #endregion

        protected Character(VitaruPlayfield vitaruPlayfield)
        {
            VitaruPlayfield = vitaruPlayfield;
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
                if (LeftSprite.Texture != null)
                    LeftSprite.Alpha = 0;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 1;
                if (StillSprite.Texture != null)
                    StillSprite.Alpha = 0;
                if (KiaiLeftSprite.Texture != null)
                    KiaiLeftSprite.Alpha = 0;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 1;
                if (KiaiStillSprite.Texture != null)
                    KiaiStillSprite.Alpha = 0;
            }
            else if (Position.X < LastX)
            {
                if (LeftSprite.Texture != null)
                    LeftSprite.Alpha = 1;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 0;
                if (StillSprite.Texture != null)
                    StillSprite.Alpha = 0;
                if (KiaiLeftSprite.Texture != null)
                    KiaiLeftSprite.Alpha = 1;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 0;
                if (KiaiStillSprite.Texture != null)
                    KiaiStillSprite.Alpha = 0;
            }
            else
            {
                if (LeftSprite.Texture != null)
                    LeftSprite.Alpha = 0;
                if (RightSprite?.Texture != null)
                    RightSprite.Alpha = 0;
                if (StillSprite.Texture != null)
                    StillSprite.Alpha = 1;
                if (KiaiLeftSprite.Texture != null)
                    KiaiLeftSprite.Alpha = 0;
                if (KiaiRightSprite?.Texture != null)
                    KiaiRightSprite.Alpha = 0;
                if (KiaiStillSprite.Texture != null)
                    KiaiStillSprite.Alpha = 1;
            }
            LastX = Position.X;
        }

        protected override void Update()
        {
            base.Update();

            if (Health <= 0 && !Dead)
                Death();

            foreach (Drawable draw in VitaruPlayfield.BulletField)
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

            MovementAnimations();
        }

        /// <summary>
        /// Gets called just before hit detection
        /// </summary>
        protected virtual void ParseBullet(DrawableBullet bullet) { }

        protected virtual void LoadAnimationSprites(TextureStore textures, Storage storage) { }

        /// <summary>
        /// Child loading for all Characters (Enemies, Player, Bosses)
        /// </summary>
        [BackgroundDependencyLoader]
        private void load(TextureStore textures, Storage storage)
        {
            Health = MaxHealth;
            //Drawable stuff loading
            Origin = Anchor.Centre;
            Anchor = Anchor.TopLeft;
            Children = new Drawable[]
            {
                Sign = new Sprite
                {
                    Alpha = 0,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Colour = CharacterColor,
                },
                SoulContainer = new Container
                {
                    Colour = CharacterColor,
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 1,
                    Children = new Drawable[]
                    {
                        StillSprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 1,
                        },
                        RightSprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                        LeftSprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                    }
                },
                KiaiContainer = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Alpha = 0,
                    Children = new Drawable[]
                    {
                        KiaiStillSprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 1,
                        },
                        KiaiRightSprite = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Alpha = 0,
                        },
                        KiaiLeftSprite = new Sprite
                        {
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
                    BorderColour = CharacterColor,
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
                        Colour = CharacterColor.Opacity(0.5f)
                    }
                }
            };

            Add(Hitbox = new SymcolHitbox(new Vector2(HitboxWidth)) { Team = Team });

            if (CharacterName == "player" || CharacterName == "enemy")
                KiaiContainer.Colour = CharacterColor;

            StillSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName, storage);
            KiaiStillSprite.Texture = VitaruSkinElement.LoadSkinElement(CharacterName + "Kiai", storage);
            Sign.Texture = VitaruSkinElement.LoadSkinElement("sign", storage);
            LoadAnimationSprites(textures, storage);
        }

        /// <summary>
        /// Removes "damage"
        /// </summary>
        /// <param name="damage"></param>
        public virtual double Hurt(float damage)
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
        public virtual double Heal(float health)
        {
            if (Health <= 0 && health > 0)
                Revive();

            Health += health;

            if (Health > MaxHealth)
                Health = MaxHealth;

            return Health;
        }

        public virtual void Death()
        {
            Dead = true;
            Expire();
        }

        public virtual void Revive()
        {
            Dead = false;
        }
    }
}
