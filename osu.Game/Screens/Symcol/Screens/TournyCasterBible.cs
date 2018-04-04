using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Screens.Symcol.Pieces;
using System;

namespace osu.Game.Screens.Symcol.Screens
{
    public class TournyCasterBible : Screen
    {
        private CasterBibleFileSystem casterBibleFileSystem;

        private readonly Bindable<Country> country1 = new Bindable<Country>() { Default = Country.UnitedStates };
        private readonly Bindable<Country> country2 = new Bindable<Country>() { Default = Country.UnitedStates };

        private Container leftContainer;
        private Box leftBox;
        private Triangles leftTriangles;
        private TeamBox leftTeam;

        private Container rightContainer;
        private Box rightBox;
        private Triangles rightTriangles;
        private TeamBox rightTeam;

        [BackgroundDependencyLoader]
        private void load(Storage storage)
        {
            casterBibleFileSystem = new CasterBibleFileSystem(storage);

            Children = new Drawable[]
            {
                leftContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.5f,
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Masking = true,

                    Children = new Drawable[]
                    {
                        leftBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = seedColor(9)
                        },
                        leftTriangles = new Triangles
                        {
                            RelativeSizeAxes = Axes.Both,
                            ColourLight = seedColor(9).Lighten(0.3f),
                            ColourDark = seedColor(9).Darken(0.3f),
                            TriangleScale = 4,
                        },
                        leftTeam = new TeamBox(casterBibleFileSystem, country1)
                    }
                },
                rightContainer = new Container
                {
                    RelativeSizeAxes = Axes.Both,
                    Width = 0.5f,
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,
                    Masking = true,

                    Children = new Drawable[]
                    {
                        rightBox = new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Colour = seedColor(1)
                        },
                        rightTriangles = new Triangles
                        {
                            RelativeSizeAxes = Axes.Both,
                            ColourLight = seedColor(1).Lighten(0.3f),
                            ColourDark = seedColor(1).Darken(0.3f),
                            TriangleScale = 4,
                        },
                        rightTeam = new TeamBox(casterBibleFileSystem, country2)
                    }
                }
            };

            country1.ValueChanged += (value) =>
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

                            bool seed = false;

                            foreach (string countryArgArg in countryArgsArgs)
                            {
                                if (countryArgArg == "Seed")
                                    seed = true;
                                else if (seed)
                                {
                                    try
                                    {
                                        leftBox.Colour = seedColor(Int32.Parse(countryArgArg));
                                        leftTriangles.ColourLight = seedColor(Int32.Parse(countryArgArg)).Lighten(0.3f);
                                        leftTriangles.ColourDark = seedColor(Int32.Parse(countryArgArg)).Darken(0.3f);
                                    }
                                    catch
                                    {
                                        Logger.Log("Failed to parse seed! - " + countryArgArg, LoggingTarget.Database, LogLevel.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            };
            country1.TriggerChange();
            country2.ValueChanged += (value) =>
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

                            bool seed = false;

                            foreach (string countryArgArg in countryArgsArgs)
                            {
                                if (countryArgArg == "Seed")
                                    seed = true;
                                else if (seed)
                                {
                                    try
                                    {
                                        rightBox.Colour = seedColor(Int32.Parse(countryArgArg));
                                        rightTriangles.ColourLight = seedColor(Int32.Parse(countryArgArg)).Lighten(0.3f);
                                        rightTriangles.ColourDark = seedColor(Int32.Parse(countryArgArg)).Darken(0.3f);
                                    }
                                    catch
                                    {
                                        Logger.Log("Failed to parse seed! - " + countryArgArg, LoggingTarget.Database, LogLevel.Error);
                                    }
                                }
                            }
                        }
                    }
                }
            };
            country2.TriggerChange();
        }

        private Color4 seedColor(int seed)
        {
            OsuColour color = new OsuColour();

            if (seed <= 8)
                return color.Green;
            else if (seed <= 16)
                return color.Yellow;
            else if (seed <= 24)
                return color.Red;
            else
                return color.Gray0;
        }
    }
}
