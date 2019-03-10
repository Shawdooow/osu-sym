using System;
using osu.Core;
using osu.Core.Config;
using osu.Core.Screens.Evast;
using osu.Framework.Allocation;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Logging;
using osu.Framework.Screens;
using osu.Game;
using osu.Game.Overlays.Settings;
using osuTK;
using osuTK.Graphics;

namespace osu.Mods.Online.Base.Screens
{
    public class Options : BeatmapScreen
    {
        public readonly SettingsButton HostGameButton;
        public readonly SettingsButton JoinGameButton;

        public readonly Container NewServer;

        protected readonly TextBox PortBox;
        protected readonly TextBox IpBox;

        private readonly Bindable<string> ipBindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.SavedIP);
        private readonly Bindable<int> portBindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.SavedPort);

        private OsuGame game;

        public Options()
        {
            AlwaysPresent = true;
            RelativeSizeAxes = Axes.Both;

            Children = new Drawable[]
            {
                HostGameButton = new SettingsButton
                {
                    Anchor = Anchor.BottomLeft,
                    Origin = Anchor.BottomLeft,
                    Position = new Vector2(12, -12),
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Host Server",
                    Action = HostServer
                },
                JoinGameButton = new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Position = new Vector2(-12, -12),
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Connect To Server",
                    Action = JoinServer
                },
                NewServer = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,

                    Position = new Vector2(0, -40),
                    Size = new Vector2(400, 60),

                    CornerRadius = 10,
                    BorderColour = Color4.White,
                    BorderThickness = 6,

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = ColourInfo.GradientVertical(Color4.DarkGreen, Color4.Green),
                            RelativeSizeAxes = Axes.Both
                        },
                        PortBox = new TextBox
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Position = new Vector2(-12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            PlaceholderText = "Port",
                        },
                        IpBox = new TextBox
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Position = new Vector2(12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            PlaceholderText = "IP"
                        }
                    }
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu) => game = osu;

        protected override void LoadComplete()
        {
            base.LoadComplete();

            IpBox.Text = ipBindable;
            PortBox.Text = portBindable.Value.ToString();
        }

        protected virtual void JoinServer()
        {
            ipBindable.Value = IpBox.Text;

            try { portBindable.Value = Int32.Parse(PortBox.Text); }
            catch { portBindable.Value = 25590; }

            if (OnlineModset.OsuNetworkingHandler != null)
            {
                game.Remove(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.Dispose();
            }

            try
            {
                OnlineModset.OsuNetworkingHandler = new OsuNetworkingHandler
                {
                    Address = ipBindable.Value + ":" + portBindable.Value
                };

                game.Add(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to server", LoggingTarget.Network, LogLevel.Debug);
                OnlineModset.OsuNetworkingHandler.Connect();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to create Networking Handler!", LoggingTarget.Network);
            }
        }

        protected virtual void HostServer()
        {
            ipBindable.Value = IpBox.Text;

            try { portBindable.Value = Int32.Parse(PortBox.Text); }
            catch { portBindable.Value = 25590; }

            if (OnlineModset.OsuNetworkingHandler != null)
            {
                game.Remove(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.Dispose();
            }

            try
            {
                OnlineModset.OsuNetworkingHandler = new OsuNetworkingHandler
                {
                    Address = ipBindable.Value + ":" + portBindable.Value
                };

                OnlineModset.OsuNetworkingHandler.Add(new OsuServerNetworkingHandler
                {
                    Address = ipBindable.Value + ":" + portBindable.Value
                });

                game.Add(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to local server", LoggingTarget.Network, LogLevel.Debug);
                OnlineModset.OsuNetworkingHandler.Connect();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to create Networking Handler!", LoggingTarget.Network);
            }
        }

        public override bool OnExiting(IScreen next)
        {
            ipBindable.Value = IpBox.Text;

            try { portBindable.Value = Int32.Parse(PortBox.Text); }
            catch { portBindable.Value = 25590; }

            return base.OnExiting(next);
        }
    }
}

