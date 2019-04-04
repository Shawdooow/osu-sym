#region usings

using osu.Core;
using osu.Core.Config;
using osu.Core.Containers.Shawdooow;
using osu.Core.OsuMods;
using osu.Core.Settings;
using osu.Core.Wiki;
using osu.Framework.Configuration;
using osu.Framework.Graphics;
using osu.Framework.Logging;
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

        private readonly Bindable<AutoJoin> auto = SymcolOsuModSet.SymConfigManager.GetBindable<AutoJoin>(SymSetting.Auto);
        private readonly Bindable<string> ipBindable = SymcolOsuModSet.SymConfigManager.GetBindable<string>(SymSetting.SavedIP);
        private readonly Bindable<int> portBindable = SymcolOsuModSet.SymConfigManager.GetBindable<int>(SymSetting.SavedPort);

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

        //public override ModSubSection GetSettings() => new OnlineSettingsSubsection();

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
                        Address = ipBindable.Value + ":" + portBindable.Value,
                        //Tcp = true,
                    };

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
                case AutoJoin.Host:
                    OsuNetworkingHandler = new OsuNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value,
                        //Tcp = true,
                    };

                    OsuNetworkingHandler.Add(new OsuServerNetworkingHandler
                    {
                        Address = ipBindable.Value + ":" + portBindable.Value,
                        //Tcp = true,
                    });

                    game.Add(OsuNetworkingHandler);
                    OsuNetworkingHandler.OnConnectedToHost += host => Logger.Log("Connected to local server", LoggingTarget.Network, LogLevel.Debug);
                    OsuNetworkingHandler.Connect();
                    break;
            }

            SymSection.OnPurge += storage =>
            {
                if (storage.ExistsDirectory("server")) storage.DeleteDirectory("server");
                if (storage.ExistsDirectory("online")) storage.DeleteDirectory("online");
            };
        }
    }
}
