#region usings

using System.Threading;
using osu.Core;
using osu.Core.Config;
using osu.Core.Containers.Shawdooow;
using osu.Core.OsuMods;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Framework.Bindables;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Framework.Platform;
using osu.Framework.Threading;
using osu.Game;
using osu.Game.Screens;
using osu.Mods.Online.Base;
using osu.Mods.Online.Settings;
using osu.Mods.Online.Wiki;
using osuTK;
using osuTK.Graphics;

#endregion

namespace osu.Mods.Online
{
    public class OnlineModset : OsuModSet
    {
        public static OsuNetworkingHandler OsuNetworkingHandler;
        internal static OsuServerNetworkingHandler Server;

        private readonly Bindable<AutoJoin> auto = SymManager.SymConfigManager.GetBindable<AutoJoin>(SymSetting.Auto);
        private readonly Bindable<string> ipBindable = SymManager.SymConfigManager.GetBindable<string>(SymSetting.SavedIP);
        private readonly Bindable<int> portBindable = SymManager.SymConfigManager.GetBindable<int>(SymSetting.SavedPort);

        public override SymcolButton GetMenuButton() => new SymcolButton
        {
            ButtonText = "Online",
            Origin = Anchor.Centre,
            Anchor = Anchor.Centre,
            ButtonColorTop = Color4.Black,
            ButtonColorBottom = Color4.LightGray,
            Size = 90,
            Position = new Vector2(220, 20),
        };

        public override ModSubSection GetSettings() => new OnlineSettingsSubsection();

        public override WikiSet GetWikiSet() => new OnlineWiki();

        public override OsuScreen GetScreen() => new OnlineMenu();

        public override void Init(OsuGame game, GameHost host)
        {
            base.Init(game, host);

            SymSection.OnPurge += storage =>
            {
                if (storage.ExistsDirectory("server")) storage.DeleteDirectory("server");
                if (storage.ExistsDirectory("online")) storage.DeleteDirectory("online");
            };
        }

        public override void LoadComplete(OsuGame game, GameHost host)
        {
            base.LoadComplete(game, host);

            switch (auto.Value)
            {
                case AutoJoin.Join:
                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value,
                    };

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += h => Logger.Log("Connected to server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
                case AutoJoin.Host:
                    Server = new OsuServerNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value,
                        //Udp = true,
                    };

                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value,
                    };

                    OsuNetworkingHandler.Add(Server);

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += h => Logger.Log("Connected to local server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
            }
        }
    }
}
