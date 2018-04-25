using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Extensions.Color4Extensions;
using OpenTK;
using osu.Game.Rulesets.Vitaru.UI;
using osu.Framework.Allocation;
using osu.Framework.Platform;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.MathUtils;
using osu.Framework.Graphics.Effects;
using osu.Game.Graphics;

namespace osu.Game.Rulesets.Vitaru.Objects.Drawables.Characters.Pieces
{
    public class Seal : Container
    {
        public Container Sign { get; private set; }

        private CircularContainer characterSigil;

        private CircularProgress health;
        private CircularProgress energy;

        private Character character;

        private Sprite gear1;
        private Sprite gear2;
        private Sprite gear3;
        private Sprite gear4;
        private Sprite gear5;

        public Seal(Character character)
        {
            this.character = character;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            if (character is VitaruPlayer v)
            {
                Color4 lightColor = v.PrimaryColor.Lighten(0.5f);
                Color4 darkColor = v.PrimaryColor.Darken(0.5f);

                Size = new Vector2(90);

                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                AlwaysPresent = true;

                Children = new Drawable[]
                {
                    Sign = new Container
                    {
                        RelativeSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,

                        Size = new Vector2(0.6f),

                        Alpha = 0,
                        AlwaysPresent = true,

                        Children = new Drawable[]
                        {
                            characterSigil = new CircularContainer
                            {
                                RelativeSizeAxes = Axes.Both,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Masking = true,
                            },
                            new Sprite
                            {
                                RelativeSizeAxes = Axes.Both,
                                Size = new Vector2(2f),

                                Colour = v.PrimaryColor,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = VitaruSkinElement.LoadSkinElement("seal", storage),
                            }
                        }
                    },
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.2f,
                        Size = new Vector2(1.5f),
                        Padding = new MarginPadding(-Blur.KernelSize(5)),
                        Child = (health = new CircularProgress
                        {
                            RelativeSizeAxes = Axes.Both,
                            InnerRadius = 0.05f,
                            Colour = v.ComplementaryColor
                        }).WithEffect(new GlowEffect
                        {
                            Colour = v.ComplementaryColor,
                            Strength = 2,
                            PadExtent = true
                        }),
                    },
                    new Container
                    {
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,
                        RelativeSizeAxes = Axes.Both,
                        Alpha = 0.2f,
                        Size = new Vector2(1.75f),
                        Padding = new MarginPadding(-Blur.KernelSize(5)),
                        Child = (energy = new CircularProgress
                        {
                            RelativeSizeAxes = Axes.Both,
                            InnerRadius = 0.05f,
                            Colour = v.SecondaryColor
                        }).WithEffect(new GlowEffect
                        {
                            Colour = v.SecondaryColor,
                            Strength = 2,
                            PadExtent = true
                        }),
                    },
                };

                switch (v.PlayableCharacter)
                {
                    default:
                        break;
                    case SelectableCharacters.SakuyaIzayoi:
                        characterSigil.Children = new Drawable[]
                        {
                            gear1 = new Sprite
                            {
                                Colour = lightColor,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = VitaruSkinElement.LoadSkinElement("gearSmall", storage),
                                Position = new Vector2(-41, 10),
                            },
                            gear2 = new Sprite
                            {
                                Colour = v.PrimaryColor,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = VitaruSkinElement.LoadSkinElement("gearMedium", storage),
                                Position = new Vector2(-4, 16),
                            },
                            gear3 = new Sprite
                            {
                                Colour = darkColor,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = VitaruSkinElement.LoadSkinElement("gearLarge", storage),
                                Position = new Vector2(-16, -34),
                            },
                            gear4 = new Sprite
                            {
                                Colour = v.PrimaryColor,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = VitaruSkinElement.LoadSkinElement("gearMedium", storage),
                                Position = new Vector2(35, -40),
                            },
                            gear5 = new Sprite
                            {
                                Colour = lightColor,
                                Anchor = Anchor.Centre,
                                Origin = Anchor.Centre,
                                Texture = VitaruSkinElement.LoadSkinElement("gearSmall", storage),
                                Position = new Vector2(33, 8),
                            },
                        };
                        break;
                }
            }
            else
            {
                Scale = new Vector2(0.6f);

                AutoSizeAxes = Axes.Both;
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                AlwaysPresent = true;

                Children = new Drawable[]
                {
                    Sign = new Container
                    {
                        AutoSizeAxes = Axes.Both,
                        Anchor = Anchor.Centre,
                        Origin = Anchor.Centre,

                        Alpha = 0,

                        Child = new Sprite
                        {
                            Anchor = Anchor.Centre,
                            Origin = Anchor.Centre,
                            Colour = character.PrimaryColor,
                            Texture = VitaruSkinElement.LoadSkinElement("sign", storage)
                        }
                    }
                };
            }
        }

        protected override void Update()
        {
            base.Update();

            Sign.RotateTo((float)(-Clock.CurrentTime / 1000 * 90) * 0.1f);

            if (character is VitaruPlayer v)
            {
                Sign.Alpha = (float)v.Energy / (float)(v.MaxEnergy * 2);

                health.Current.Value = v.Health / v.MaxHealth;
                energy.Current.Value = v.Energy / v.MaxEnergy;

                switch (v.PlayableCharacter)
                {
                    default:
                        break;
                    case SelectableCharacters.SakuyaIzayoi:
                        float speed = 0.25f;
                        gear1.RotateTo((float)(Clock.CurrentTime / 1000 * 90) * 1.25f * speed);
                        gear2.RotateTo((float)(-Clock.CurrentTime / 1000 * 90) * 1.1f * speed);
                        gear3.RotateTo((float)(Clock.CurrentTime / 1000 * 90) * speed);
                        gear4.RotateTo((float)(-Clock.CurrentTime / 1000 * 90) * 1.1f * speed);
                        gear5.RotateTo((float)(Clock.CurrentTime / 1000 * 90) * 1.25f * speed);
                        break;
                }
            }
        }
    }
}
