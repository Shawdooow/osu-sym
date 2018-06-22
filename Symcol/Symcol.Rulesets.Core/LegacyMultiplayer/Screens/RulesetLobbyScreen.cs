using System;
using System.Collections.Generic;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Colour;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Framework.Screens;
using osu.Game.Overlays.Settings;
using OpenTK;
using OpenTK.Graphics;
using Symcol.Core.LegacyNetworking;
using Symcol.osu.Core.Screens.Evast;
using Symcol.Rulesets.Core.LegacyMultiplayer.Networking;
using Symcol.Rulesets.Core.Rulesets;

namespace Symcol.Rulesets.Core.LegacyMultiplayer.Screens
{
    public abstract class RulesetLobbyScreen : BeatmapScreen
    {
        public abstract RulesetMatchScreen MatchScreen { get; }

        public RulesetNetworkingClientHandler RulesetNetworkingClientHandler;

        public readonly SettingsButton HostGameButton;
        public readonly SettingsButton JoinGameButton;

        public readonly Container NewGame;

        protected readonly TextBox LocalPort;
        protected readonly TextBox LocalIp;
        protected readonly TextBox HostPort;
        protected readonly TextBox HostIp;

        public readonly Container JoinIP;

        private readonly Bindable<string> hostip = SymcolSettingsSubsection.SymcolConfigManager.GetBindable<string>(SymcolSetting.HostIP);
        private readonly Bindable<string> localip = SymcolSettingsSubsection.SymcolConfigManager.GetBindable<string>(SymcolSetting.LocalIP);
        private readonly Bindable<int> hostport = SymcolSettingsSubsection.SymcolConfigManager.GetBindable<int>(SymcolSetting.HostPort);
        private readonly Bindable<int> localport = SymcolSettingsSubsection.SymcolConfigManager.GetBindable<int>(SymcolSetting.LocalPort);

        public RulesetLobbyScreen()
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
                    Text = "Host Game",
                    Action = HostGame
                },
                JoinGameButton = new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    Position = new Vector2(-12, -12),
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Join Game",
                    Action = JoinGame
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
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                RulesetNetworkingClientHandler.Dispose();
            }

            return base.OnExiting(next);
        }

        protected virtual void HostGame()
        {
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                RulesetNetworkingClientHandler.Dispose();
            }
            Add(RulesetNetworkingClientHandler = new RulesetNetworkingClientHandler(ClientType.Host, LocalIp.Text, Int32.Parse(LocalPort.Text)));

            List<ClientInfo> list = new List<ClientInfo>();
            list.Add(RulesetNetworkingClientHandler.RulesetClientInfo);

            JoinMatch(list);
        }

        protected virtual void JoinGame()
        {
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                RulesetNetworkingClientHandler.Dispose();
            }
            Add(RulesetNetworkingClientHandler = new RulesetNetworkingClientHandler(ClientType.Peer, HostIp.Text, Int32.Parse(HostPort.Text)));

            RulesetNetworkingClientHandler.OnConnectedToHost += (p) => JoinMatch(p);
        }

        protected virtual void JoinMatch(List<ClientInfo> clientInfos)
        {
            Remove(RulesetNetworkingClientHandler);
            Push(MatchScreen);
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
