using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Extensions.Color4Extensions;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Framework.Input;
using osu.Framework.Logging;
using osu.Framework.MathUtils;
using osu.Framework.Platform;
using osu.Framework.Screens;
using osu.Game.Beatmaps;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Overlays.Settings;
using osu.Game.Rulesets.Objects;
using osu.Game.Rulesets.Objects.Types;
using osu.Game.Screens.Symcol.CasterBible.Pieces;
using osu.Game.Screens.Symcol.Pieces;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace osu.Game.Screens.Symcol.CasterBible
{
    public class TournyCasterBible : OsuScreen
    {
        private Storage storage;

        private BeatmapManager beatmaps;

        private float headerOffset => header.Position.Y + header.DrawHeight;

        private Header header;

        private Container screen;

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
        private void load(Storage storage, BeatmapManager beatmaps)
        {
            this.storage = storage;
            this.beatmaps = beatmaps;

            Children = new Drawable[]
            {
                screen = new Container
                {
                    RelativeSizeAxes = Axes.Both
                },
                header = new Header(),
            };

            header.CurrentBibleScreen.ValueChanged += (value) =>
            {
                switch (value)
                {
                    case BibleScreen.Teams:
                        initializeTeamsScreen();
                        break;
                    case BibleScreen.MapPool:
                        initializeMapPool();
                        break;
                    case BibleScreen.MatchResults:
                        initializeMatchResults();
                        break;
                }
            };
            header.CurrentBibleScreen.TriggerChange();
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
                return color.Gray5;
        }

        private void initializeMapPool()
        {
            CasterBibleFileSystem casterBibleFileSystem = new CasterBibleFileSystem(storage.GetStorageForDirectory("Bible"), "teams.mango");

            List<KeyValuePair<string, string>> cups = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("None", "None") };

            SettingsDropdown<string> cup;
            Bindable<string> currentCup = new Bindable<string>();

            SettingsDropdown<string> year;
            Bindable<string> currentYear = new Bindable<string>();

            SettingsDropdown<string> stage;
            Bindable<string> currentStage = new Bindable<string>();

            FillFlowContainer mapPoolFlow;

            screen.Children = new Drawable[]
            {
                new Container
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,

                    RelativeSizeAxes = Axes.Both,
                    Height = 0.96f,
                    Width = 0.8f,

                    Children = new Drawable[]
                    {
                        new ScrollContainer
                        {
                            RelativeSizeAxes = Axes.Both,

                            Child = mapPoolFlow = new FillFlowContainer()
                        }
                    }
                },
                new Container
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,

                    RelativeSizeAxes = Axes.Both,
                    Height = 0.96f,
                    Width = 0.2f,

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            RelativeSizeAxes = Axes.Both,
                            Alpha = 0.8f,
                            Colour = Color4.Black
                        },
                        new FillFlowContainer
                        {
                            Direction = FillDirection.Vertical,
                            Spacing = new Vector2(0, 5),
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,

                            Children = new Drawable[]
                            {
                                cup = new SettingsDropdown<string>
                                {
                                    LabelText = "Current Cup"
                                },
                                year = new SettingsDropdown<string>
                                {
                                    LabelText = "Current Year"
                                },
                                stage = new SettingsDropdown<string>
                                {
                                    LabelText = "Current Stage"
                                }
                            }
                        }
                    }
                },
            };

            try
            {
                foreach (string cupName in casterBibleFileSystem.Storage.GetDirectories("Cups"))
                {
                    string[] args = cupName.Split('\\');
                    cups.Add(new KeyValuePair<string, string>(args.Last(), args.Last()));
                }

                cup.Items = cups.Distinct().ToList();
                cup.Bindable = currentCup;

                currentCup.ValueChanged += (c) =>
                {
                    try
                    {
                        List<KeyValuePair<string, string>> years = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("None", "None") };

                        Storage cupStorage = casterBibleFileSystem.Storage.GetStorageForDirectory("Bible\\Cups\\" + c);

                        foreach (string yearName in cupStorage.GetDirectories(""))
                        {
                            string[] args = yearName.Split('\\');
                            years.Add(new KeyValuePair<string, string>(args.Last(), args.Last()));
                        }

                        year.Items = years.Distinct().ToList();
                        year.Bindable = currentYear;

                        currentYear.ValueChanged += (y) =>
                        {
                            try
                            {
                                List<KeyValuePair<string, string>> stages = new List<KeyValuePair<string, string>> { new KeyValuePair<string, string>("None", "None") };
                                Storage yearStorage = cupStorage.GetStorageForDirectory("Bible\\Cups\\" + c + "\\" + y);

                                foreach (string stageName in yearStorage.GetDirectories(""))
                                {
                                    string[] args = stageName.Split('\\');
                                    stages.Add(new KeyValuePair<string, string>(args.Last(), args.Last()));
                                }

                                stage.Items = stages.Distinct().ToList();
                                stage.Bindable = currentStage;

                                currentStage.ValueChanged += (s) =>
                                {
                                    try
                                    {
                                        Storage stageStorage = cupStorage.GetStorageForDirectory("Bible\\Cups\\" + c + "\\" + y + "\\" + s);

                                        casterBibleFileSystem = new CasterBibleFileSystem(stageStorage, "maps.mango");

                                        mapPoolFlow = new FillFlowContainer
                                        {
                                            RelativeSizeAxes = Axes.X,
                                            AutoSizeAxes = Axes.Y,
                                            Direction = FillDirection.Vertical
                                        };

                                        string[] maps = casterBibleFileSystem.GetFileContent().Split('.');

                                        foreach (string map in maps)
                                        {
                                            string[] mapArgs = map.Split('|');

                                            int mapSetID = 0;
                                            int mapID = 0;
                                            Mods mod = Mods.NoMod;
                                            string information = "";

                                            foreach (string mapArg in mapArgs)
                                            {
                                                string[] mapArgArgs = mapArg.Split('=');

                                                bool setId = false;
                                                bool id = false;
                                                bool m = false;
                                                bool info = false;

                                                foreach (string mapArgArg in mapArgArgs)
                                                {
                                                    try
                                                    {
                                                        if (mapArgArg == "BeatmapSetID" || mapArgArg == "\r\nBeatmapSetID")
                                                            setId = true;
                                                        if (id)
                                                            mapSetID = Int32.Parse(mapArgArg);
                                                    }
                                                    catch
                                                    {
                                                        Logger.Log("Failed to parse BeatmapSetID: " + mapArgArg, LoggingTarget.Database, LogLevel.Error);
                                                    }

                                                    try
                                                    {
                                                        if (mapArgArg == "BeatmapID" || mapArgArg == "\r\nBeatmapID")
                                                            id = true;
                                                        if (id)
                                                            mapID = Int32.Parse(mapArgArg);
                                                    }
                                                    catch
                                                    {
                                                        Logger.Log("Failed to parse BeatmapID: " + mapArgArg, LoggingTarget.Database, LogLevel.Error);
                                                    }

                                                    try
                                                    {
                                                        if (mapArgArg == "Mod")
                                                            m = true;
                                                        if (m)
                                                            switch (mapArgArg)
                                                            {
                                                                case "NM":
                                                                case "NoMod":
                                                                    mod = Mods.NoMod;
                                                                    break;
                                                                case "HD":
                                                                case "Hidden":
                                                                    mod = Mods.Hidden;
                                                                    break;
                                                                case "HR":
                                                                case "HardRock":
                                                                    mod = Mods.HardRock;
                                                                    break;
                                                                case "DT":
                                                                case "DoubleTime":
                                                                    mod = Mods.DoubleTime;
                                                                    break;
                                                                case "FM":
                                                                case "FreeMod":
                                                                    mod = Mods.FreeMod;
                                                                    break;
                                                                case "TB":
                                                                case "TieBreaker":
                                                                    mod = Mods.TieBreaker;
                                                                    break;
                                                            }
                                                    }
                                                    catch
                                                    {
                                                        Logger.Log("Failed to parse Mod: " + mapArgArg, LoggingTarget.Database, LogLevel.Error);
                                                    }

                                                    try
                                                    {
                                                        if (mapArgArg == "Information")
                                                            info = true;
                                                        if (info)
                                                            information = mapArgArg;
                                                    }
                                                    catch
                                                    {
                                                        Logger.Log("Failed to parse Information: " + mapArgArg, LoggingTarget.Database, LogLevel.Error);
                                                    }
                                                }
                                            }

                                            foreach (BeatmapSetInfo beatmapSet in beatmaps.GetAllUsableBeatmapSets())
                                                if (beatmapSet.OnlineBeatmapSetID == mapSetID)
                                                {
                                                    bool k = false;
                                                    foreach (BeatmapInfo beatmap in beatmapSet.Beatmaps)
                                                        if (beatmap.OnlineBeatmapID == mapID)
                                                        {
                                                            mapPoolFlow.Add(new MapDetails(beatmaps.GetWorkingBeatmap(beatmap, Beatmap.Value)));
                                                            k = true;
                                                            break;
                                                        }

                                                    if (k)
                                                        break;
                                                }
                                        }
                                    }
                                    catch { }
                                };
                            }
                            catch { }
                        };
                    }
                    catch { }
                };
            }
            catch { }
        }

        private void initializeMatchResults()
        {
            screen.Children = new Drawable[]
            {

            };
        }

        private void initializeTeamsScreen()
        {
            CasterBibleFileSystem casterBibleFileSystem = new CasterBibleFileSystem(storage.GetStorageForDirectory("Bible"), "teams.mango");

            screen.Children = new Drawable[]
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

                                        leftBox.Colour = Color4.Blue;
                                        leftTriangles.ColourLight = Color4.Blue.Lighten(0.3f);
                                        leftTriangles.ColourDark = Color4.Blue.Darken(0.3f);
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

                                        rightBox.Colour = Color4.Blue;
                                        rightTriangles.ColourLight = Color4.Blue.Lighten(0.3f);
                                        rightTriangles.ColourDark = Color4.Blue.Darken(0.3f);
                                    }
                                }
                            }
                        }
                    }
                }
            };
            country2.TriggerChange();
        }

        private class MapDetails : ClickableContainer
        {
            private Sprite beatmapBG;
            private SpriteText name;
            private SpriteText artist;
            private SpriteText difficulty;
            private SpriteText time;

            private Box dim;

            public MapDetails(WorkingBeatmap workingBeatmap)
            {
                draw();

                HitObject lastObject = workingBeatmap.Beatmap.HitObjects.LastOrDefault();
                double endTime = (lastObject as IHasEndTime)?.EndTime ?? lastObject?.StartTime ?? 0;

                beatmapBG.Texture = workingBeatmap.Background;
                name.Text = workingBeatmap.BeatmapSetInfo.Metadata.Title;
                artist.Text = "By: " + workingBeatmap.BeatmapSetInfo.Metadata.Artist;
                difficulty.Text = workingBeatmap.BeatmapInfo.Version + " (" + Math.Round(workingBeatmap.BeatmapInfo.StarDifficulty, 2) + " stars) mapped by " + workingBeatmap.BeatmapInfo.Metadata.AuthorString;
                time.Text = getBPMRange(workingBeatmap.Beatmap) + " bpm for " + TimeSpan.FromMilliseconds(endTime - workingBeatmap.Beatmap.HitObjects.First().StartTime).ToString(@"m\:ss");

                BorderColour = getColour(workingBeatmap.BeatmapInfo);
                EdgeEffect = new EdgeEffectParameters
                {
                    Radius = 16,
                    Type = EdgeEffectType.Shadow,
                    Colour = getColour(workingBeatmap.BeatmapInfo).Opacity(0.2f)
                };
                Action = () => Process.Start("https://osu.ppy.sh/beatmapsets/" + workingBeatmap.BeatmapSetInfo.OnlineBeatmapSetID);
            }

            public MapDetails(int onlineBeatmapSetID)
            {
                draw();
                name.Text = "Missing Map!";
                artist.Text = "Click to open in Browser";
                Action = () => Process.Start("https://osu.ppy.sh/beatmapsets/" + onlineBeatmapSetID);
            }

            public MapDetails(bool invalid)
            {
                draw();
                name.Text = "Invalid / No Map Selected!";
                artist.Text = "Akward. . .";
                Action = () => Process.Start("https://osu.ppy.sh/home");
            }

            private void draw()
            {
                Anchor = Anchor.Centre;
                Origin = Anchor.Centre;

                RelativeSizeAxes = Axes.X;

                Width = 0.95f;
                Height = 100f;

                Masking = true;
                BorderColour = Color4.LightBlue;
                BorderThickness = 4;
                CornerRadius = 10;

                EdgeEffect = new EdgeEffectParameters
                {
                    Radius = 16,
                    Type = EdgeEffectType.Shadow,
                    Colour = Color4.LightBlue.Opacity(0.2f)
                };

                Children = new Drawable[]
                {
                beatmapBG = new Sprite
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    RelativeSizeAxes = Axes.Both,
                    FillMode = FillMode.Fill,
                },
                dim = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = Color4.Black,
                    Alpha = 0.6f
                },
                name = new SpriteText
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Position = new Vector2(10, 0),
                    Font = @"Exo2.0-SemiBoldItalic",
                    TextSize = 40
                },
                artist = new SpriteText
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Position = new Vector2(10, 38),
                    Font = @"Exo2.0-MediumItalic",
                    TextSize = 24
                },
                difficulty = new SpriteText
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Position = new Vector2(10, 64),
                    Font = "Exo2.0-Bold",
                    TextSize = 16
                },
                time = new SpriteText
                {
                    Anchor = Anchor.TopLeft,
                    Origin = Anchor.TopLeft,
                    Position = new Vector2(10, 84),
                    TextSize = 16
                }
                };
            }

            protected override bool OnHover(InputState state)
            {
                dim.FadeTo(0.4f, 200);

                return base.OnHover(state);
            }

            protected override void OnHoverLost(InputState state)
            {
                base.OnHoverLost(state);

                dim.FadeTo(0.6f, 200);
            }

            //"Borrowed" stuff
            private string getBPMRange(Beatmap beatmap)
            {
                double bpmMax = beatmap.ControlPointInfo.BPMMaximum;
                double bpmMin = beatmap.ControlPointInfo.BPMMinimum;

                if (Precision.AlmostEquals(bpmMin, bpmMax))
                    return $"{bpmMin:0}";

                return $"{bpmMin:0}-{bpmMax:0} (mostly {beatmap.ControlPointInfo.BPMMode:0})";
            }

            private enum DifficultyRating
            {
                Easy,
                Normal,
                Hard,
                Insane,
                Expert,
                ExpertPlus
            }

            private DifficultyRating getDifficultyRating(BeatmapInfo beatmap)
            {
                if (beatmap == null)
                    throw new ArgumentNullException(nameof(beatmap));

                var rating = beatmap.StarDifficulty;

                if (rating < 1.5) return DifficultyRating.Easy;
                if (rating < 2.25) return DifficultyRating.Normal;
                if (rating < 3.75) return DifficultyRating.Hard;
                if (rating < 5.25) return DifficultyRating.Insane;
                if (rating < 6.75) return DifficultyRating.Expert;
                return DifficultyRating.ExpertPlus;
            }

            private Color4 getColour(BeatmapInfo beatmap)
            {
                OsuColour palette = new OsuColour();
                switch (getDifficultyRating(beatmap))
                {
                    case DifficultyRating.Easy:
                        return palette.Green;
                    default:
                    case DifficultyRating.Normal:
                        return palette.Blue;
                    case DifficultyRating.Hard:
                        return palette.Yellow;
                    case DifficultyRating.Insane:
                        return palette.Pink;
                    case DifficultyRating.Expert:
                        return palette.Purple;
                    case DifficultyRating.ExpertPlus:
                        return palette.Gray0;
                }
            }
        }
    }
}
