using OpenTK;
using osu.Framework.Graphics;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using System;
using osu.Framework.Configuration;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Game.Overlays.Settings;
using OpenTK.Graphics;
using Symcol.Core.Networking.NetworkingHandlers;
using Symcol.Core.Networking.Packets;
using Symcol.osu.Core;
using Symcol.osu.Core.Config;
using Symcol.osu.Core.Screens.Evast;
using Symcol.osu.Mods.Multi.Networking;

namespace Symcol.osu.Mods.Multi.Screens
{
    public class ConnectToServer : BeatmapScreen
    {
        public override string Title => "Connect To Server";

        protected OsuNetworkingClientHandler OsuNetworkingClientHandler;

        public readonly SettingsButton HostGameButton;
        public readonly SettingsButton JoinGameButton;

        public readonly Container NewGame;

        protected readonly TextBox LocalPort;
        protected readonly TextBox LocalIp;
        protected readonly TextBox HostPort;
        protected readonly TextBox HostIp;

        private readonly Bindable<string> hostip = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.HostIP);
        private readonly Bindable<string> localip = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.LocalIP);
        private readonly Bindable<int> hostport = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.HostPort);
        private readonly Bindable<int> localport = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.LocalPort);

        public ConnectToServer()
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
                    //Action = HostGame
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
                NewGame = new Container
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
                        LocalPort = new TextBox
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Position = new Vector2(-12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            Text = "25570"
                        },
                        LocalIp = new TextBox
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Position = new Vector2(12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            Text = "Local IP Address"
                        }
                    }
                },
                NewGame = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,

                    Position = new Vector2(0, 40),
                    Size = new Vector2(400, 60),

                    CornerRadius = 10,
                    BorderColour = Color4.White,
                    BorderThickness = 6,

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = ColourInfo.GradientVertical(Color4.DarkBlue, Color4.Blue),
                            RelativeSizeAxes = Axes.Both
                        },
                        HostPort = new TextBox
                        {
                            Anchor = Anchor.CentreRight,
                            Origin = Anchor.CentreRight,
                            Position = new Vector2(-12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            Text = "25570"
                        },
                        HostIp = new TextBox
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Position = new Vector2(12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            Text = "Host's IP Address"
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            LocalIp.Text = localip;
            LocalPort.Text = localport.Value.ToString();
            HostIp.Text = hostip;
            HostPort.Text = hostport.Value.ToString();
        }

        protected override bool OnExiting(Screen next)
        {
            if (OsuNetworkingClientHandler != null)
            {
                Remove(OsuNetworkingClientHandler);
                OsuNetworkingClientHandler.Dispose();
            }

            return base.OnExiting(next);
        }

        protected virtual void JoinServer()
        {
            if (OsuNetworkingClientHandler == null)
            {
                Add(OsuNetworkingClientHandler = new OsuNetworkingClientHandler
                {
                    Address = LocalIp.Text + ":" + LocalPort.Text
                });
                OsuNetworkingClientHandler.OnConnectedToHost += host => Connected();
            }
            OsuNetworkingClientHandler.Connect();
        }

        protected virtual void Connected()
        {
            Remove(OsuNetworkingClientHandler);
            Push(new Lobby(OsuNetworkingClientHandler));
        }

        protected override void Dispose(bool isDisposing)
        {
            localip.Value = LocalIp.Text;
            localport.Value = Int32.Parse(LocalPort.Text);
            hostip.Value = HostIp.Text;
            hostport.Value = Int32.Parse(HostPort.Text);

            base.Dispose(isDisposing);
        }
    }
}

