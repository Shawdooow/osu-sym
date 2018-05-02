using OpenTK;
using OpenTK.Graphics;
using osu.Framework.Graphics;
using osu.Framework.Graphics.Containers;
using osu.Framework.Graphics.Shapes;
using osu.Framework.Graphics.UserInterface;
using osu.Game.Overlays.Settings;
using osu.Game.Screens;
using System;
using osu.Framework.Screens;
using System.Collections.Generic;
using Symcol.Core.Networking;
using Symcol.Rulesets.Core.Multiplayer.Networking;
using osu.Framework.Configuration;
using Symcol.Rulesets.Core.Rulesets;

namespace Symcol.Rulesets.Core.Multiplayer.Screens
{
    public abstract class RulesetLobbyScreen : OsuScreen
    {
        public abstract RulesetMatchScreen MatchScreen { get; }

        public RulesetNetworkingClientHandler RulesetNetworkingClientHandler;

        public readonly SettingsButton HostGameButton;
        public readonly SettingsButton DirectConnectButton;
        public readonly SettingsButton JoinGameButton;

        public readonly Container NewGame;
        protected readonly TextBox HostPort;
        protected readonly TextBox Ip;

        public readonly Container JoinIP;

        private readonly Bindable<string> ip = SymcolSettingsSubsection.SymcolConfigManager.GetBindable<string>(SymcolSetting.IP);
        private readonly Bindable<int> port = SymcolSettingsSubsection.SymcolConfigManager.GetBindable<int>(SymcolSetting.Port);

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
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Host Game",
                    Action = HostGame
                },
                DirectConnectButton = new SettingsButton
                {
                    Anchor = Anchor.BottomCentre,
                    Origin = Anchor.BottomCentre,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Direct Connect",
                    Action = DirectConnect
                },
                JoinGameButton = new SettingsButton
                {
                    Anchor = Anchor.BottomRight,
                    Origin = Anchor.BottomRight,
                    RelativeSizeAxes = Axes.X,
                    Width = 0.3f,
                    Text = "Join Game"
                },
                NewGame = new Container
                {
                    Anchor = Anchor.Centre,
                    Origin = Anchor.Centre,
                    Masking = true,
                    Size = new Vector2(400, 60),

                    CornerRadius = 10,
                    BorderColour = Color4.White,
                    BorderThickness = 6,

                    Children = new Drawable[]
                    {
                        new Box
                        {
                            Colour = Color4.Blue,
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
                        Ip = new TextBox
                        {
                            Anchor = Anchor.CentreLeft,
                            Origin = Anchor.CentreLeft,
                            Position = new Vector2(12, 0),
                            RelativeSizeAxes = Axes.X,
                            Width = 0.42f,
                            Height = 20,
                            Text = "IP Address"
                        }
                    }
                }
            };
        }

        protected override void LoadComplete()
        {
            base.LoadComplete();

            Ip.Text = ip;
            HostPort.Text = port.Value.ToString();
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
            Add(RulesetNetworkingClientHandler = new RulesetNetworkingClientHandler(ClientType.Host, Ip.Text, Int32.Parse(HostPort.Text)));

            List<ClientInfo> list = new List<ClientInfo>();
            list.Add(RulesetNetworkingClientHandler.RulesetClientInfo);

            JoinMatch(list);
        }

        protected virtual void DirectConnect()
        {
            if (RulesetNetworkingClientHandler != null)
            {
                Remove(RulesetNetworkingClientHandler);
                RulesetNetworkingClientHandler.Dispose();
            }
            Add(RulesetNetworkingClientHandler = new RulesetNetworkingClientHandler(ClientType.Peer, Ip.Text, Int32.Parse(HostPort.Text)));

            RulesetNetworkingClientHandler.OnConnectedToHost += (p) => JoinMatch(p);
        }

        protected virtual void JoinMatch(List<ClientInfo> clientInfos)
        {
            Remove(RulesetNetworkingClientHandler);
            Push(MatchScreen);
        }

        protected override void Dispose(bool isDisposing)
        {
            ip.Value = Ip.Text;
            port.Value = Int32.Parse(HostPort.Text);

            base.Dispose(isDisposing);
        }
    }
}
