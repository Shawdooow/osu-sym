using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Containers;
using osu.Game.Graphics.Sprites;
using osu.Game.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.Graphics.Containers;
using Symcol.osu.Mods.Caster.Pieces;

namespace Symcol.osu.Mods.Caster.CasterScreens.Pieces
{
    public class TeamBox : SymcolContainer
    {
        private readonly Box box;
        private readonly Triangles triangles;

        private readonly FillFlowContainer<Player> players;
        private readonly FillFlowContainer<Team> teams;

        private readonly OsuScrollContainer bottomScrollContainer;
        private readonly OsuScrollContainer teamScrollContainer;

        public TeamBox(CasterControlPanel controlPanel)
        {
            SymcolClickableContainer addPlayer;
            SymcolClickableContainer addTeam;
            SettingsDropdown<Team> teamsDropdown;

            OsuColour osu = new OsuColour();

            RelativeSizeAxes = Axes.Both;
            Size = new Vector2(0.48f, 0.96f);

            Masking = true;
            CornerRadius = 8;

            BorderColour = Color4.White;
            BorderThickness = 4;

            Children = new Drawable[]
            {
                box = new Box
                {
                    RelativeSizeAxes = Axes.Both,
                    Colour = osu.Gray5,
                },
                triangles = new Triangles
                {
                    RelativeSizeAxes = Axes.Both,
                    ColourLight = osu.Gray1,
                    ColourDark = osu.Gray8,
                    TriangleScale = 2,
                },
                teamScrollContainer = new OsuScrollContainer
                {
                    Anchor = Anchor.TopCentre,
                    Origin = Anchor.TopCentre,

                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.96f, 0.48f),
                    Position = new Vector2(0, 6),

                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,

                            Children = new Drawable[]
                            {
                                new SymcolContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,

                                    Children = new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Colour = osu.Yellow,
                                            Text = "Teams",
                                            TextSize = 40
                                        },
                                        addTeam = new SymcolClickableContainer
                                        {
                                            Anchor = Anchor.CentreRight,
                                            Origin = Anchor.Centre,

                                            Position = new Vector2(-12, 0),
                                            Size = new Vector2(24),
                                            Rotation = 45,
                                            Action = () => teams.Add(new Team("New Team", teams, controlPanel.Editable)),

                                            Child = new SpriteIcon
                                            {
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                RelativeSizeAxes = Axes.Both,
                                                Icon = FontAwesome.fa_osu_cross_o,
                                                Colour = Color4.Cyan
                                            }
                                        }
                                    }
                                },
                                teams = new FillFlowContainer<Team>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                }
                            }
                        }
                    }
                },
                bottomScrollContainer = new OsuScrollContainer
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,

                    RelativeSizeAxes = Axes.Both,
                    Size = new Vector2(0.96f, 0.88f),
                    Position = new Vector2(0, -6),

                    Children = new Drawable[]
                    {
                        new FillFlowContainer
                        {
                            RelativeSizeAxes = Axes.X,
                            AutoSizeAxes = Axes.Y,

                            Children = new Drawable[]
                            {
                                new SymcolContainer
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                   
                                    Children = new Drawable[]
                                    {
                                        new OsuSpriteText
                                        {
                                            Colour = osu.Yellow,
                                            Text = "Players",
                                            TextSize = 40
                                        },
                                        addPlayer = new SymcolClickableContainer
                                        {
                                            Anchor = Anchor.CentreRight,
                                            Origin = Anchor.Centre,

                                            Alpha = 0,
                                            Position = new Vector2(-12, 0),
                                            Size = new Vector2(24),
                                            Rotation = 45,
                                            Action = () => players.Add(new Player("New Player", 0, players, controlPanel.Editable)),

                                            Child = new SpriteIcon
                                            {
                                                Anchor = Anchor.Centre,
                                                Origin = Anchor.Centre,
                                                RelativeSizeAxes = Axes.Both,
                                                Icon = FontAwesome.fa_osu_cross_o,
                                                Colour = Color4.Cyan
                                            }
                                        }
                                    }
                                },
                                players = new FillFlowContainer<Player>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,
                                }
                            }
                        }
                    }
                },
                teamsDropdown = new SettingsDropdown<Team>
                {
                    LabelText = "Team",
                    Position = new Vector2(0, 6),
                    Bindable = new Bindable<Team>()
                }
            };

            //Filler info for testing
            Team symcol = new Team("Symcol", teams, controlPanel.Editable);
            symcol.Players.Add(new Player("Shawdooow", 7726082, players, controlPanel.Editable));
            teams.Child = symcol;

            teamsDropdown.Bindable.ValueChanged += team =>
            {
                players.Children = new Player[]{};

                foreach (Player p in team.Players)
                    players.Add(p);
            };

            controlPanel.Stage.ValueChanged += stage =>
            {
                if (controlPanel.Cup == "None") return;
                if (controlPanel.Year == "None") return;
                if (stage == "None") return;
            };

            controlPanel.Editable.ValueChanged += edit =>
            {
                teamsDropdown.Alpha = edit ? 0 : 1;
                addPlayer.Alpha = edit ? 1 : 0;
                teamScrollContainer.Alpha = edit ? 1 : 0;
                bottomScrollContainer.Height = edit ? 0.48f : 0.88f;

                if (!edit)
                {
                    List<KeyValuePair<string, Team>> ts = new List<KeyValuePair<string, Team>>();

                    foreach (Team t in teams)
                    {
                        Team r = new Team(t.TeamName, teams, controlPanel.Editable);

                        foreach (Player p in t.Players)
                        {
                            Player n = new Player(p.Username, p.ID, players, controlPanel.Editable);
                            r.Players.Add(n);
                        }

                        ts.Add(new KeyValuePair<string, Team>(t.TeamName, r));
                    }

                    if (teamsDropdown.Bindable.Value != null)
                        foreach (KeyValuePair<string, Team> pair in ts)
                            if (pair.Value.TeamName == teamsDropdown.Bindable.Value.TeamName)
                            {
                                pair.Value.Players = new List<Player>();
                                foreach (Player p in players)
                                {
                                    Player n = new Player(p.Username, p.ID, players, controlPanel.Editable);
                                    pair.Value.Players.Add(n);
                                }
                            }

                    teamsDropdown.Items = ts;
                }
                else
                {
                    teams.Children = new Team[]{};
                    foreach (KeyValuePair<string, Team> pair in teamsDropdown.Items)
                        teams.Add(new Team(pair.Value.TeamName, teams, controlPanel.Editable));
                }
            };
            controlPanel.Editable.TriggerChange();
        }

        private class Player : EditableOsuSpriteText
        {
            public string Username;

            public int ID;

            public Player(string username, int id, FillFlowContainer<Player> players, Bindable<bool> bindable)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                Editable.BindTo(bindable);

                Username = username;
                ID = id;

                Text = username;
                TextSize = 32;

                OsuSpriteText.Position = new Vector2(18, 0);

                SpriteIcon icon = new SpriteIcon
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Icon = FontAwesome.fa_chevron_right,
                    Size = new Vector2(TextSize / 2),
                    Colour = Color4.White
                };

                OsuTextBox.Width = 0.42f;
                OsuTextBox idBox = new OsuTextBox
                {
                    Alpha = 0,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    Position = new Vector2(-20, 0),
                    RelativeSizeAxes = Axes.X,
                    Height = TextSize,
                    Width = 0.42f,
                    Text = id.ToString()
                };

                SymcolClickableContainer delete = new SymcolClickableContainer
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,

                    Size = new Vector2(TextSize / 2),

                    Action = () => players.Remove(this),

                    Child = new SpriteIcon
                    {
                        RelativeSizeAxes = Axes.Both,
                        Icon = FontAwesome.fa_osu_cross_o,
                        Colour = Color4.Red
                    }
                };

                Add(idBox);
                Add(icon);
                Add(delete);

                OsuTextBox.OnCommit += (commit, ree) => { Username = commit.Text; };

                idBox.OnCommit += (commit, ree) =>
                {
                    try
                    {
                        int i = Int32.Parse(commit.Text);
                        ID = i;

                        string u = $@"https://osu.ppy.sh/users/{i}";

                        OsuSpriteText.Tooltip = u;
                        OsuSpriteText.Action = () => { Process.Start(u); };
                    }
                    catch { Logger.Log(commit.Text + " is not a valid user id!", LoggingTarget.Runtime, LogLevel.Error);}
                };

                Editable.ValueChanged += edit =>
                {
                    icon.Alpha = edit ? 0 : 1;
                    idBox.Alpha = edit ? 1 : 0;
                    delete.Alpha = edit ? 1 : 0;

                    if (!edit)
                    {
                        Username = OsuTextBox.Text;

                        try
                        {
                            int i = Int32.Parse(idBox.Text);
                            ID = i;

                            string u = $@"https://osu.ppy.sh/users/{i}";

                            OsuSpriteText.Tooltip = u;
                            OsuSpriteText.Action = () => { Process.Start(u); };
                        }
                        catch
                        {
                            Logger.Log(OsuTextBox.Text + " is not a valid user id!", LoggingTarget.Runtime, LogLevel.Error);
                            idBox.Text = ID.ToString();
                        }
                    }
                };
                Editable.TriggerChange();

                string url = $@"https://osu.ppy.sh/users/{id}";

                OsuSpriteText.Tooltip = url;
                OsuSpriteText.Action = () => { Process.Start(url); };
            }
        }

        private class Team : EditableOsuSpriteText
        {
            public string TeamName;

            public List<Player> Players = new List<Player>();

            public Team(string name, FillFlowContainer<Team> teams, Bindable<bool> bindable)
            {
                TeamName = name;

                if (bindable != null)
                    Editable.BindTo(bindable);

                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                Text = name;
                TextSize = 32;

                Add(new SymcolClickableContainer
                {
                    Anchor = Anchor.CentreRight,
                    Origin = Anchor.CentreRight,

                    Size = new Vector2(TextSize / 2),

                    Action = () => teams.Remove(this),

                    Child = new SpriteIcon
                    {
                        RelativeSizeAxes = Axes.Both,
                        Icon = FontAwesome.fa_osu_cross_o,
                        Colour = Color4.Red
                    }
                });

                OsuTextBox.Current.ValueChanged += team => { TeamName = team; };
            }
        }
    }
}
