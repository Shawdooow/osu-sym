using System;
using System.Diagnostics;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Logging;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
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

        public TeamBox(CasterControlPanel controlPanel)
        {
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
                    ColourDark = osu.Gray8
                },
                new ScrollContainer
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
                                new OsuSpriteText
                                {
                                    Colour = osu.Yellow,
                                    Text = "Players",
                                    TextSize = 40
                                },
                                players = new FillFlowContainer<Player>
                                {
                                    RelativeSizeAxes = Axes.X,
                                    AutoSizeAxes = Axes.Y,

                                    Child = new Player("Shawdooow", 7726082, controlPanel.Editable)
                                }
                            }
                        }
                    }
                },
                new SettingsDropdown<string>
                {
                    LabelText = "Team",
                    Position = new Vector2(0, 6)
                }
            };
        }

        private class Player : EditableOsuSpriteText
        {
            public string Username;

            public int ID;

            public Player(string username, int id, Bindable<bool> bindable = null)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                if (bindable != null)
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

                OsuTextBox.Width = 0.48f;
                OsuTextBox idBox = new OsuTextBox
                {
                    Alpha = 0,
                    Anchor = Anchor.TopRight,
                    Origin = Anchor.TopRight,
                    RelativeSizeAxes = Axes.X,
                    Height = TextSize,
                    Width = 0.48f,
                    Text = id.ToString()
                };

                Add(idBox);
                Add(icon);

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
    }
}
