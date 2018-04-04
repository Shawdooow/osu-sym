using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;

namespace osu.Game.Screens.Symcol.Pieces
{
    public class TeamBox : Container
    {
        private readonly OsuTextFlowContainer players;
        private readonly OsuTextFlowContainer notes;

        public TeamBox(CasterBibleFileSystem casterBibleFileSystem, Bindable<Country> countree)
        {
            RelativeSizeAxes = Axes.Both;

            Anchor = Anchor.Centre;
            Origin = Anchor.Centre;

            Width = 0.9f;
            Height = 0.9f;

            Masking = true;
            CornerRadius = 16;

            Children = new Drawable[]
            {
                new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.8f
                },
                new OsuSpriteText
                {
                    Position = new Vector2(16, 60),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    TextSize = 32,
                    Text = "Team Members:"
                },
                players = new OsuTextFlowContainer(t => { t.TextSize = 20; })
                {
                    Position = new Vector2(16, 100),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = "ree\nree\nree"
                },
                new OsuSpriteText
                {
                    Position = new Vector2(16, 240),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    TextSize = 32,
                    Text = "Notes:"
                },
                notes = new OsuTextFlowContainer(t => { t.TextSize = 20; })
                {
                    Position = new Vector2(16, 280),
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    RelativeSizeAxes = Axes.X,
                    AutoSizeAxes = Axes.Y,
                    Text = "ree\nree\nree"
                },
                new BetterSettingsEnumDropdown<Country>
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,
                    Position = new Vector2(0, 10),
                    Bindable = countree
                },
            };

            countree.ValueChanged += (value) =>
            {
                string[] countrys = casterBibleFileSystem.GetFileContent().Split('.');

                foreach (string country in countrys)
                {
                    string[] countryArgs = country.Split('/');
                    bool isCountry = false;

                    foreach (string countryArg in countryArgs)
                    {
                        if (countryArg == "\r\n" + value.ToString())
                            isCountry = true;
                        else if (isCountry)
                        {
                            string[] countryArgsArgs = countryArg.Split('=');

                            bool player = false;
                            bool note = false;

                            foreach (string countryArgArg in countryArgsArgs)
                            {
                                if (countryArgArg == "Players")
                                    player = true;
                                else if (player)
                                {
                                    string s = "";
                                    string[] playersArgs = countryArgArg.Split(',');

                                    foreach (string playerArg in playersArgs)
                                    {
                                        s = s + playerArg + "\n";
                                    }

                                    players.Text = s;
                                }

                                if (countryArgArg == "Notes")
                                    note = true;
                                else if (note)
                                {
                                    notes.Text = countryArgArg;
                                }
                            }
                        }
                    }
                }
            };
            countree.TriggerChange();
        }

        private class BetterSettingsEnumDropdown<T> : SettingsEnumDropdown<T>
        {
            protected override Drawable CreateControl() => new BetterOsuEnumDropdown<T>
            {
                Margin = new MarginPadding { Top = 5 },
                RelativeSizeAxes = Axes.X,
            };

            private class BetterOsuEnumDropdown<T> : OsuEnumDropdown<T>
            {
                public BetterOsuEnumDropdown()
                {
                    Menu.MaxHeight = 160;
                }
            }
        }
    }
}
