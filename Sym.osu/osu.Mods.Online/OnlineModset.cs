using osu.Core;
using osu.Core.Config;
using osu.Core.Containers.Shawdooow;
using osu.Core.OsuMods;
using osu.Core.Wiki;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Logging;
using osu.Game;
using osu.Game.Screens;
using osu.Mods.Online.Base;
using osu.Mods.Online.Wiki;
using osuTK;
using osuTK.Graphics;

namespace osu.Mods.Online
{
    public class OnlineModset : OsuModSet
    {
        public static OsuNetworkingHandler OsuNetworkingHandler;

        private readonly Bindable<AutoJoin> auto = SymcolOsuModSet.SymcolConfigManager.GetBindable<AutoJoin>(SymcolSetting.Auto);
        private readonly Bindable<string> ipBindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<string>(SymcolSetting.SavedIP);
        private readonly Bindable<int> portBindable = SymcolOsuModSet.SymcolConfigManager.GetBindable<int>(SymcolSetting.SavedPort);

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

        public override WikiSet GetWikiSet() => new OnlineWiki();

        public override OsuScreen GetScreen() => new OnlineMenu();

        public override void LoadComplete(OsuGame game)
        {
            base.LoadComplete(game);

            switch (auto.Value)
            {
                case AutoJoin.Join:
                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value
                    };

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
                case AutoJoin.Host:
                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value
                    };

                    OsuNetworkingHandler.Add(new OsuServerNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value
                    });

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to local server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
            }
        }
    }
}
