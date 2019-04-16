#region usings

using System;
using osu.Core;
using osu.Core.Config;
using osu.Core.Settings;
using osu.Framework.Allocation;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Game;
using osu.Game.Overlays.Settings;
using osu.Mods.Online.Base;

#endregion

namespace osu.Mods.Online.Settings
{
    public class OnlineSettingsSubsection : ModSubSection
    {
        protected override string Header => "Online";

        private OsuGame game;

        private readonly Bindable<string> ipBindable = SymManager.SymConfigManager.GetBindable<string>(SymSetting.SavedIP);
        private readonly Bindable<int> portBindable = SymManager.SymConfigManager.GetBindable<int>(SymSetting.SavedPort);
        private readonly Bindable<string> portString = new Bindable<string>
        {
            Default = "25590",
            Value = SymManager.SymConfigManager.Get<int>(SymSetting.SavedPort).ToString(),
        };

        public OnlineSettingsSubsection()
        {
            Children = new Drawable[]
            {
                new SettingsEnumDropdown<AutoJoin>
                {
                    LabelText = "Connect on launch?",
                    Bindable = SymManager.SymConfigManager.GetBindable<AutoJoin>(SymSetting.Auto)
                },
                new SettingsTextBox
                {
                    LabelText = "IP",
                    Bindable = ipBindable
                },
                new SettingsTextBox
                {
                    LabelText = "Port",
                    Bindable = portString
                },
                new SettingsButton
                {
                    Text = "Host Server",
                    Action = HostServer
                },
                new SettingsButton
                {
                    Text = "Connect To Server",
                    Action = JoinServer
                },
                new SettingsButton
                {
                    Text = "Import from Host's Stable Install",
                    Action = () => OnlineModset.OsuNetworkingHandler.Import()
                },
                new SettingsButton
                {
                    Text = "Toggle UDP (WILL PROBABLY CRASH!)",
                    Action = () => { OnlineModset.OsuNetworkingHandler.Udp = !OnlineModset.OsuNetworkingHandler.Udp; }
                },
            };

            portString.ValueChanged += value =>
            {
                try
                {
                    portBindable.Value = int.Parse(value.NewValue);
                }
                catch
                {
                    portString.Value = portBindable.Value.ToString();
                }
            };
        }

        [BackgroundDependencyLoader]
        private void load(OsuGame osu) => game = osu;

        protected virtual void JoinServer()
        {
            if (OnlineModset.OsuNetworkingHandler != null)
            {
                game.Remove(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.Dispose();
            }

            try
            {
                OnlineModset.OsuNetworkingHandler = new OsuNetworkingHandler
                {
                    Address = ipBindable.Value + ":" + portBindable.Value,
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
            if (OnlineModset.Server != null)
            {
                OnlineModset.OsuNetworkingHandler.Remove(OnlineModset.Server);
                OnlineModset.Server.Dispose();
            }

            if (OnlineModset.OsuNetworkingHandler != null)
            {
                game.Remove(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.Dispose();
            }

            try
            {
                OnlineModset.Server = new OsuServerNetworkingHandler
                {
                    Address = ipBindable.Value + ":" + portBindable.Value,
                    //Udp = true,
                };

                OnlineModset.OsuNetworkingHandler = new OsuNetworkingHandler
                {
                    Address = ipBindable.Value + ":" + portBindable.Value,
                };

                OnlineModset.OsuNetworkingHandler.Add(OnlineModset.Server);

                game.Add(OnlineModset.OsuNetworkingHandler);
                OnlineModset.OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to local server", LoggingTarget.Network, LogLevel.Debug);
                OnlineModset.OsuNetworkingHandler.Connect();
            }
            catch (Exception e)
            {
                Logger.Error(e, "Failed to create Networking Handler!", LoggingTarget.Network);
            }
        }
    }
}
