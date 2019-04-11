#region usings

using osu.Framework.Allocation;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Effects;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.MathUtils;
using osu.Framework.Platform;
using osu.Game.Graphics.Sprites;
using osu.Game.Rulesets.Vitaru.ChapterSets.Touhosu.Chapters.Scarlet.Characters.Drawables;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.TouhosuPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Characters.VitaruPlayers;
using osu.Game.Rulesets.Vitaru.Ruleset.Containers.Gameplay;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Game.Rulesets.Vitaru.Ruleset.Characters.Pieces
{
    public class Seal : Container
    {
        public Container Sign { get; private set; }
        public Sprite SignSprite { get; private set; }

        private CircularContainer characterSigil;

        private OsuSpriteText rightValue;
        private OsuSpriteText leftValue;

        private CircularProgress health;
        private CircularProgress energy;

        private DrawableCharacter character;

        public Seal(DrawableCharacter character)
        {
            this.character = character;
        }

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            if (character is DrawableVitaruPlayer v)
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
                            SignSprite = new Sprite
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
                    new Container
                    {
                        Position = new Vector2(-30, 0),
                        Anchor = Anchor.CentreLeft,
                        Origin = Anchor.CentreRight,

                        Child = (leftValue = new OsuSpriteText
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreRight,
                            Colour = v.ComplementaryColor,
                            Font = "Venera",
                            TextSize = 16,
                            Alpha = 0.75f,
                        }).WithEffect(new GlowEffect
                        {
                            Colour = Color4.Transparent,
                            PadExtent = true,
                        }),
                    },
                    new Container
                    {
                        Position = new Vector2(30, 0),
                        Anchor = Anchor.CentreRight,
                        Origin = Anchor.CentreLeft,

                        Child = (rightValue = new OsuSpriteText
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreLeft,
                            Colour = v.SecondaryColor,
                            Font = "Venera",
                            TextSize = 16,
                            Alpha = 0.75f,
                        }).WithEffect(new GlowEffect
                        {
                            Colour = Color4.Transparent,
                            PadExtent = true,
                        }),
                    },
                };
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

            if (character is DrawableVitaruPlayer v)
            {
                if (v is DrawableTouhosuPlayer t)
                {
                    Sign.Alpha = (float)t.Energy / (float)(t.TouhosuPlayer.MaxEnergy * 2);
                    energy.Current.Value = t.Energy / t.TouhosuPlayer.MaxEnergy;
                }

                health.Current.Value = v.Health / v.MaxHealth;

                switch (v.Player.Name)
                {
                    case "Sakuya Izayoi":
                        DrawableSakuya s = v as DrawableSakuya;
                        leftValue.Text = s.SetRate.ToString();
                        break;
                }
            }
        }

        protected override void Dispose(bool isDisposing)
        {
            character = null;
            base.Dispose(isDisposing);
        }
    }
}
