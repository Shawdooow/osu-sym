﻿using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
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
using Symcol.osu.Mods.Caster.CasterScreens.TeamsPieces.Drawables;
using System.Linq;

namespace Symcol.osu.Mods.Caster.CasterScreens.TeamsPieces
{
    public class TeamBox : SymcolContainer
    {
        private readonly Box box;
        private readonly Triangles triangles;

        private readonly FillFlowContainer<DrawablePlayer> players;
        private readonly FillFlowContainer<DrawableTeam> teams;

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
                                            Action = () => teams.Add(new DrawableTeam(new Team
                                            {
                                                Players = new List<Player> { new Player() }
                                            }, teams, controlPanel.Editable)),

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
                                teams = new FillFlowContainer<DrawableTeam>
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
                                            Action = () => players.Add(new DrawablePlayer(new Player(), players, controlPanel.Editable)),

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
                                players = new FillFlowContainer<DrawablePlayer>
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
            Team symcol = new Team
            {
                Name = "Symcol",
                Players = new List<Player>
                {
                    new Player
                    {
                        Name = "Shawdooow",
                        PlayerID = 7726082
                    }
                }
            };
            teams.Child = new DrawableTeam(symcol, teams, controlPanel.Editable);

            teamsDropdown.Bindable.ValueChanged += team =>
            {
                players.Children = new DrawablePlayer[]{};

                foreach (Player p in team.Players)
                    players.Add(new DrawablePlayer(p, players, controlPanel.Editable));
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
                    List<KeyValuePair<string, Team>> ts = new List<KeyValuePair<string, Team>>
                    {
                        new KeyValuePair<string, Team>("None", new Team { Name = "None" })
                    };

                    foreach (DrawableTeam t in teams)
                        if (t.Team.Name != "None")
                            ts.Add(new KeyValuePair<string, Team>(t.Team.Name, t.Team));

                    if (teamsDropdown.Bindable.Value != null)
                        foreach (KeyValuePair<string, Team> team in ts)
                            if (team.Value.Name == teamsDropdown.Bindable.Value.Name)
                            {
                                team.Value.Players = new List<Player>();
                                foreach (DrawablePlayer player in players)
                                    team.Value.Players.Add(player.Player);
                            }

                    teamsDropdown.Items = ts;
                    if (teamsDropdown.Bindable.Value == null)
                        teamsDropdown.Bindable.Value = ts.First().Value;
                }
                else
                {
                    teams.Children = new DrawableTeam[]{};
                    foreach (KeyValuePair<string, Team> pair in teamsDropdown.Items)
                        teams.Add(new DrawableTeam(pair.Value, teams, controlPanel.Editable));
                }
            };
            controlPanel.Editable.TriggerChange();
        }
    }
}