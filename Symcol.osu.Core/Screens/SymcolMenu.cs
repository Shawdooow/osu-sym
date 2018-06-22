using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Screens;
using osu.Game.Screens.Menu;
using OpenTK;
using OpenTK.Graphics;
using Symcol.osu.Core.Containers;
using Symcol.osu.Core.Evast;
using Symcol.osu.Core.KoziLord;
using Symcol.osu.Core.Shawdooow;

namespace Symcol.osu.Core.Screens
{
    public class SymcolMenu : BeatmapScreen
    {
        private const int animation_duration = 600;

        protected override float BackgroundBlur => 10;

        public static OsuScreen LegacyRulesetMultiplayerScreen;
        public static OsuScreen Lobby;

        private readonly OsuLogo logo;
        private readonly Container<SymcolButton> buttonsContainer;

        public SymcolMenu()
        {
            Children = new Drawable[]
            {
                new MenuSideFlashes(),
                buttonsContainer = new Container<SymcolButton>
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    Children = new[]
                    {
                        /*
                        new SymcolButton
                        {
                            ButtonName = "osu!Talk",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = new Color4(33 , 58 , 79 , 255),
                            ButtonColorBottom = new Color4(17 , 31 , 42 , 255),
                            ButtonSize = 60,
                            Action = delegate { Push(new OsuTalkMenu()); },
                            ButtonPosition = new Vector2(250 , 175),
                        },
                        */
                        new SymcolButton
                        {
                            ButtonName = "Thumbnail",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.Black,
                            ButtonColorBottom = Color4.Yellow,
                            ButtonSize = 90,
                            //Action = delegate { Push(new ShawdooowLazerLiveWallpaper()); },
                            ButtonPosition = new Vector2(170 , 190),
                        },
                        /*
                        new SymcolButton
                        {
                            ButtonName = "Offset",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.Black,
                            ButtonColorBottom = Color4.White,
                            ButtonSize = 90,
                            Action = delegate { Push(new SymcolOffsetTicker()); },
                            ButtonPosition = new Vector2(-10 , -190),
                        },
                        new SymcolButton
                        {
                            ButtonName = "Pokeosu",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.DarkOrange,
                            ButtonColorBottom = Color4.Orange,
                            ButtonSize = 75,
                            Action = delegate { Push(new PokeosuMenu()); },
                            ButtonPosition = new Vector2(200 , 100),
                        },
                        */
                        new SymcolButton
                        {
                            ButtonName = "Map Mixer",
                            ButtonFontSizeMultiplier = 0.8f,
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.Purple,
                            ButtonColorBottom = Color4.HotPink,
                            ButtonSize = 120,
                            Action = delegate { Push(new SymcolMapMixer()); },
                            ButtonPosition = new Vector2(-200 , -150),
                        },
                        /*
                        new SymcolButton
                        {
                            ButtonName = "Play",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.DarkGreen,
                            ButtonColorBottom = Color4.Green,
                            ButtonSize = 130,
                            Action = delegate { Push(new PlaySongSelect()); },
                            ButtonPosition = new Vector2(300 , -20),
                        },
                        */
                        new SymcolButton
                        {
                            ButtonName = "Legacy MP",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = OsuColour.FromHex("#d8d8d8"),
                            ButtonColorBottom = OsuColour.FromHex("#3a3a3a"),
                            ButtonSize = 60,
                            Action = delegate { Push(LegacyRulesetMultiplayerScreen); },
                            ButtonPosition = new Vector2(280 , -180),
                        },
                        new SymcolButton
                        {
                            ButtonName = "Multi",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.LimeGreen,
                            ButtonColorBottom = Color4.Yellow,
                            ButtonSize = 120,
                            Action = delegate { Push(Lobby); },
                            ButtonPosition = new Vector2(180 , -100),
                        },
                        new SymcolButton
                        {
                            ButtonName = "Caster Bible",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.Yellow,
                            ButtonColorBottom = Color4.Green,
                            ButtonSize = 100,
                            //Action = delegate { Push(new TournyCasterBible()); },
                            ButtonPosition = new Vector2(40 , -200),
                        },

                        new SymcolButton
                        {
                            ButtonName = "Evast's",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = OsuColour.FromHex("#7b30ff"),
                            ButtonColorBottom = OsuColour.FromHex("#d230ff"),
                            ButtonSize = 90,
                            Action = delegate { Push(new MoreScreen()); },
                            ButtonPosition = new Vector2(20 , 200),
                        },
                        new SymcolButton
                        {
                            ButtonName = "Jacob's",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.Purple,
                            ButtonColorBottom = Color4.Magenta,
                            ButtonSize = 100,
                            Action = delegate { Push(new KoziScreen()); },
                            ButtonPosition = new Vector2(-250, -20),

                        },
                        /*
                        new SymcolButton
                        {
                            ButtonName = "Edit",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.DarkGoldenrod,
                            ButtonColorBottom = Color4.Gold,
                            ButtonSize = 90,
                            Action = delegate { Push(new Editor()); },
                            ButtonPosition = new Vector2(250 , -150),
                        },*/
                        new SymcolButton
                        {
                            ButtonName = "Tests",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.DarkCyan,
                            ButtonColorBottom = Color4.Cyan,
                            ButtonSize = 100,
                            //Action = delegate { Push(new SymcolTestScreen()); },
                            ButtonPosition = new Vector2(-150 , 200),
                        },

                        new SymcolButton
                        {
                            ButtonName = "Back",
                            Origin = Anchor.Centre,
                            Anchor = Anchor.Centre,
                            ButtonColorTop = Color4.DarkRed,
                            ButtonColorBottom = Color4.Red,
                            ButtonSize = 80,
                            Action = Exit,
                            ButtonPosition = new Vector2(-350 , 300),
                        },
                    },
                },
                logo = new OsuLogo
                {
                    Scale = new Vector2(1.25f),
                    Action = () => open(logo),
                    Origin = Anchor.Centre,
                    Anchor = Anchor.Centre,
                }
            };
        }

        private bool open(Container container)
        {
            logo.Action = () => close(container);
            container.ScaleTo(new Vector2(0.5f), animation_duration, Easing.InOutBack);

            foreach(var button in buttonsContainer)
                button.MoveTo(button.ButtonPosition, animation_duration, Easing.InOutBack);
            return true;
        }

        private bool close(Container container)
        {
            logo.Action = () => open(container);
            container.ScaleTo(new Vector2(1.25f), animation_duration, Easing.InOutBack);

            foreach (var button in buttonsContainer)
                button.MoveTo(Vector2.Zero, animation_duration, Easing.InOutBack);
            return true;
        }

        protected override void OnEntering(Screen last)
        {
            base.OnEntering(last);
            Content.FadeInFromZero(250);
        }
        
        protected override void OnResuming(Screen last)
        {
            base.OnResuming(last);
            Content.FadeIn(250);
            Content.ScaleTo(1, 250, Easing.OutSine);
        }

        protected override void OnSuspending(Screen next)
        {
            base.OnSuspending(next);
            Content.ScaleTo(1.1f, 250, Easing.InSine);
            Content.FadeOut(250);
        }

        protected override bool OnExiting(Screen next)
        {
            Content.FadeOut(100);
            return base.OnExiting(next);
        }
    }
}
