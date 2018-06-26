using System.Collections.Generic;
using System.Diagnostics;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.Sprites;
using osu.Game.Graphics;
using osu.Game.Graphics.Backgrounds;
using osu.Game.Graphics.Sprites;
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
            public readonly string Username;

            public readonly int ID;

            public Player(string username, int id, Bindable<bool> bindable = null)
            {
                RelativeSizeAxes = Axes.X;
                AutoSizeAxes = Axes.Y;

                if (bindable != null)
                    Editable.BindTo(bindable);

                Username = username;
                ID = id;

                Text = "  " + username;
                TextSize = 32;

                Add(new SpriteIcon
                {
                    Anchor = Anchor.CentreLeft,
                    Origin = Anchor.CentreLeft,
                    Icon = FontAwesome.fa_chevron_right,
                    Size = new Vector2(TextSize / 2),
                    Colour = Color4.White
                });

                string url = $@"https://osu.ppy.sh/users/{id}";

                OsuSpriteText.Tooltip = url;
                OsuSpriteText.Action = () => { Process.Start(url); };
            }
        }
    }
}
